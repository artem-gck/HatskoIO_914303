using StructureService.Application.Services;
using StructureService.Domain.Services;
using Microsoft.EntityFrameworkCore;
using StructureService.Domain.Entities;
using Microsoft.OpenApi.Models;
using System.Reflection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Events;
using StructureServiceApi.MapperProfiles;
using StructureService.Infrastructure.DataBase.Context;
using StructureService.Infrastructure.DataBase;
using StructureService.Infrastructure.Services;
using StructureServiceApi.Middlewares;
using MassTransit;
using StructureService.Infrastructure.Messages.Consumers;
using Messages;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("StructureConnection");

var connectionStringAzure = builder.Configuration.GetConnectionString("ServiceBus");
var newPurchaseTopic = builder.Configuration["Topics:NewUser"];
var subscriptionName = builder.Configuration["SubscriptionName"];

// Add services to the container.

var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("HealthChecks", LogEventLevel.Warning)
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddMassTransit(serviceCollectionConfigurator =>
{
    serviceCollectionConfigurator.AddConsumer<NewUserConsumer>();

    serviceCollectionConfigurator.AddBus(registrationContext => Bus.Factory.CreateUsingAzureServiceBus(configurator =>
    {
        configurator.Host(connectionStringAzure);

        configurator.Message<NewUserMessage>(m =>
        {
            m.SetEntityName(newPurchaseTopic);
        });

        configurator.SubscriptionEndpoint<NewUserMessage>(subscriptionName, endpointConfigurator =>
        {
            endpointConfigurator.ConfigureConsumer<NewUserConsumer>(registrationContext);
        });
    }));
});

builder.Services.AddHealthChecksUI()
                .AddInMemoryStorage();
builder.Services.AddHealthChecks()
                .AddSqlServer(connectionString);

builder.Services.AddAutoMapper(typeof(ControllerProfile));

builder.Services.AddDbContext<StructureContext>(opt =>
    opt.UseSqlServer(connectionString, b => b.MigrationsAssembly("StructureService.Infrastructure.DataBase")));

builder.Services.AddScoped<IRepository<PositionEntity>, Repository<PositionEntity>>();
builder.Services.AddScoped<IRepository<DepartmentEntity>, Repository<DepartmentEntity>>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IService<PositionEntity>, Service<PositionEntity>>();
builder.Services.AddScoped<IService<DepartmentEntity>, Service<DepartmentEntity>>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "StructureService API",
        Description = "An ASP.NET Core Web API for managing structure items"
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

app.UseHttpsRedirection();

app.UseAuthorization();
app.ConfigureCustomExceptionMiddleware();
app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI();

app.Run();
