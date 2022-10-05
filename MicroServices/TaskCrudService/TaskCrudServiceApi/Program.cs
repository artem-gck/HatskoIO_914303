using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Reflection;
using TaskCrudService.Adapters.DataSource;
using TaskCrudService.Adapters.DataSource.Context;
using TaskCrudService.Adapters.Output;
using TaskCrudService.Domain.Entities;
using TaskCrudService.Middlewares;
using TaskCrudService.Ports.Output;
using TaskCrudService.Posts.DataSource;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TaskConnection");

// Add services to the container.

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddHealthChecksUI()
                .AddInMemoryStorage();
builder.Services.AddHealthChecks()
                .AddSqlServer(connectionString);

builder.Services.AddDbContext<TaskContext>(opt =>
    opt.UseSqlServer(connectionString, b => b.MigrationsAssembly("TaskCrudService.Adapters.DataSource")));

builder.Services.AddScoped<IRepository<TaskEntity>, TasksRepository>();
builder.Services.AddScoped<IService<TaskEntity>, TaskService>();

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
