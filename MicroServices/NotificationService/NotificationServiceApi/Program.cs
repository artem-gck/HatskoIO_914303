using HealthChecks.UI.Client;
using MassTransit;
using Messages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NotificationService.DataAccess.DataBase.Context;
using NotificationService.DataAccess.DataBase.Interfaces;
using NotificationService.DataAccess.DataBase.Realisation;
using NotificationService.Messages;
using NotificationService.Notification.Jobs;
using NotificationService.Services;
using NotificationService.Services.MapperProfiles;
using NotificationServiceApi;
using NotificationServiceApi.MapperProfiles;
using NotificationServiceApi.Middlewares;
using Quartz;
using System.Reflection;
using MessageContext = NotificationService.DataAccess.DataBase.Context.MessageContext;

var builder = WebApplication.CreateBuilder(args);
var dbConnectionString = Environment.GetEnvironmentVariable("NotificationConnection") ?? builder.Configuration.GetConnectionString("NotificationConnection");
var connectionStringAzure = Environment.GetEnvironmentVariable("ServiceBus") ?? builder.Configuration.GetConnectionString("ServiceBus");
var identityString = Environment.GetEnvironmentVariable("IdentityPath") ?? builder.Configuration["IdentityPath"];

var newUserTopic = builder.Configuration["Topics:NewUser"];
var updateEmailUserTopic = builder.Configuration["Topics:UpdateEmailUser"];
var newTaskQueue = builder.Configuration["Queues:NewTask"];
var updateTaskQueue = builder.Configuration["Queues:UpdateTask"];
var deleteTaskQueue = builder.Configuration["Queues:DeleteTask"];
var subscriptionName = builder.Configuration["SubscriptionName"];

var clientHandler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
};

// Add services to the container.

builder.Services.AddDbContext<MessageContext>(opt =>
    opt.UseSqlServer(dbConnectionString, b => b.MigrationsAssembly("NotificationService.DataAccess.DataBase")));

builder.Services.AddAutoMapper(typeof(ControllerProfile), typeof(ServiceProfile));

builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddMassTransit(serviceCollectionConfigurator =>
{
    serviceCollectionConfigurator.AddConsumer<NewUserConsumer>();
    serviceCollectionConfigurator.AddConsumer<NewTaskConsumer>();
    serviceCollectionConfigurator.AddConsumer<UpdateTaskConsumer>();
    serviceCollectionConfigurator.AddConsumer<DeleteTaskConsumer>();
    serviceCollectionConfigurator.AddConsumer<UpdateEmailUserConsumer>();

    serviceCollectionConfigurator.AddBus(registrationContext => Bus.Factory.CreateUsingAzureServiceBus(configurator =>
    {
        configurator.Host(connectionStringAzure);

        configurator.Message<NewUserMessage>(m =>
        {
            m.SetEntityName(newUserTopic);
        });

        configurator.Message<UpdateEmailUserMessage>(m =>
        {
            m.SetEntityName(updateEmailUserTopic);
        });

        configurator.SubscriptionEndpoint<NewUserMessage>(subscriptionName, endpointConfigurator =>
        {
            endpointConfigurator.ConfigureConsumer<NewUserConsumer>(registrationContext);
        });

        configurator.SubscriptionEndpoint<UpdateEmailUserMessage>(subscriptionName, endpointConfigurator =>
        {
            endpointConfigurator.ConfigureConsumer<UpdateEmailUserConsumer>(registrationContext);
        });

        configurator.ReceiveEndpoint(newTaskQueue, endpointConfigurator =>
        {
            endpointConfigurator.ConfigureConsumer<NewTaskConsumer>(registrationContext);
        });

        configurator.ReceiveEndpoint(updateTaskQueue, endpointConfigurator =>
        {
            endpointConfigurator.ConfigureConsumer<UpdateTaskConsumer>(registrationContext);
        });

        configurator.ReceiveEndpoint(deleteTaskQueue, endpointConfigurator =>
        {
            endpointConfigurator.ConfigureConsumer<DeleteTaskConsumer>(registrationContext);
        });
    }));
});

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

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.Authority = identityString;
    options.RequireHttpsMetadata = false;
    options.Audience = "notification_api";
    options.BackchannelHttpHandler = clientHandler;
});

// adds an authorization policy to make sure the token is for scope 'api1'
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("NotificationScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "notification_api");
    });
});

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

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{identityString}/connect/authorize"),
                TokenUrl = new Uri($"{identityString}/connect/token"),
                Scopes = new Dictionary<string, string> { { "notification_api", "notification api" } }
            }
        }
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddHealthChecks().AddSqlite(dbConnectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setup =>
    {
        setup.SwaggerEndpoint($"/swagger/v1/swagger.json", "Version 1.0");
        setup.OAuthClientId("notification_api");
        setup.OAuthAppName("Notification api");
        //setup.OAuthScopeSeparator(" ");
        setup.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.ConfigureCustomExceptionMiddleware();
app.MapControllers();

app.Run();
