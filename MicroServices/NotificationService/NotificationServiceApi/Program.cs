using NotificationService.DataAccess.Http.Interfaces;
using NotificationService.DataAccess.Http.Realisations;
using NotificationService.Notification.Jobs;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IManagementRepository, ManagementRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionScopedJobFactory();

    var jobKey = new JobKey("EmailSender");

    q.AddJob<EmailSender>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("taskReminder", "emailGroup")
                    .WithCronSchedule("0 0 8/6 ? * * *"));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
