﻿using Microsoft.EntityFrameworkCore;
using NotificationService.DataAccess.DataBase.Context;
using NotificationService.DataAccess.Http.Interfaces;
using NotificationService.DataAccess.Http.Realisations;
using NotificationService.Notification.Jobs;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
var dbConnectionString = builder.Configuration.GetConnectionString("Sqlite");
var managenmentConnectionString = builder.Configuration.GetConnectionString("ManagementService");
var tasksConnectionString = builder.Configuration.GetConnectionString("TasksService");

// Add services to the container.

builder.Services.AddDbContext<MessageContext>(opt =>
    opt.UseSqlite(dbConnectionString, b => b.MigrationsAssembly("NotificationService.DataAccess.DataBase")));

builder.Services.AddScoped<IManagementAccess, ManagementAccess>();
builder.Services.AddScoped<ITaskAccess, TaskAccess>();

builder.Services.AddHttpClient<IManagementAccess, ManagementAccess>(httpClient => { httpClient.BaseAddress = new Uri(managenmentConnectionString); });
builder.Services.AddHttpClient<ITaskAccess, TaskAccess>(httpClient => { httpClient.BaseAddress = new Uri(tasksConnectionString); });

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionScopedJobFactory();

    var jobKey = new JobKey("EmailSender");

    q.AddJob<EmailSender>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("taskReminder", "emailGroup")
                    .WithCronSchedule("0 0 8/6 ? * * *"));
                    //.WithCronSchedule("* * * ? * * *"));
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
