using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NotificationService.DataAccess.DataBase;
using NotificationService.DataAccess.DataBase.Context;
using NotificationService.DataAccess.Http.Interfaces;
using NotificationService.DataAccess.Http.Realisations;
using NotificationService.Notification.Jobs;
using NotificationService.Services;
using NotificationService.Services.MapperProfiles;
using NotificationServiceApi.MapperProfiles;
using NotificationServiceApi.Middlewares;
using Quartz;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var dbConnectionString = Environment.GetEnvironmentVariable("NotificationConnection") ?? builder.Configuration.GetConnectionString("NotificationConnection");
var managenmentConnectionString = Environment.GetEnvironmentVariable("ManagementConnection") ?? builder.Configuration.GetConnectionString("ManagementConnection");
var tasksConnectionString = Environment.GetEnvironmentVariable("TasksConnection") ?? builder.Configuration.GetConnectionString("TasksConnection");

var clientHandler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
};

var clientHandler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
};

// Add services to the container.

builder.Services.AddDbContext<MessageContext>(opt =>
    opt.UseSqlite(dbConnectionString, b => b.MigrationsAssembly("NotificationService.DataAccess.DataBase")));

builder.Services.AddAutoMapper(typeof(ControllerProfile), typeof(ServiceProfile));

builder.Services.AddScoped<IManagementAccess, ManagementAccess>();
builder.Services.AddScoped<ITaskAccess, TaskAccess>();

builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddHttpClient<IManagementAccess, ManagementAccess>(httpClient => { httpClient.BaseAddress = new Uri(managenmentConnectionString); }).ConfigurePrimaryHttpMessageHandler(() => clientHandler); ;
builder.Services.AddHttpClient<ITaskAccess, TaskAccess>(httpClient => { httpClient.BaseAddress = new Uri(tasksConnectionString); }).ConfigurePrimaryHttpMessageHandler(() => clientHandler); ;

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionScopedJobFactory();

    var jobKey = new JobKey("EmailSender");

    q.AddJob<EmailSender>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("taskReminder", "emailGroup")
                    .WithCronSchedule("0 0 8/6 ? * * *"));
                    //.WithCronSchedule("0/30 * * ? * * *"));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Notification API",
        Description = "An ASP.NET Core Web API for notification of user",
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddHealthChecks().AddSqlite(dbConnectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.ConfigureCustomExceptionMiddleware();
app.MapControllers();

app.Run();
