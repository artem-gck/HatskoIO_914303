using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TaskCrudService.Application.Realisation;
using TaskCrudService.Application.Realisation.MapperProfiles;
using TaskCrudService.Application.Services;
using TaskCrudService.Application.Services.Dto;
using TaskCrudService.Domain.Entities;
using TaskCrudService.Domain.Realisation;
using TaskCrudService.Domain.Realisation.Context;
using TaskCrudService.Domain.Services;
using TaskCrudService.MapperProfiles;
using TaskCrudService.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TaskConnection");

// Add services to the container.

builder.Services.AddHealthChecksUI()
                .AddInMemoryStorage();
builder.Services.AddHealthChecks()
                .AddSqlServer(connectionString);

builder.Services.AddAutoMapper(typeof(ApplicationProfile), typeof(ControllerProfile));

builder.Services.AddDbContext<TaskContext>(opt =>
    opt.UseSqlServer(connectionString, b => b.MigrationsAssembly("TaskCrudService.Domain.Realisation")));

builder.Services.AddScoped<IRepository<TaskEntity>, TasksRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IService<TaskDto>, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "TaskCrudService API",
        Description = "An ASP.NET Core Web API for managing tasks items"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCustomExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI();

app.Run();
