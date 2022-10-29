using HealthChecks.UI.Client;
using MassTransit;
using Messages;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using UsersService.DataAccess;
using UsersService.DataAccess.Entities.Context;
using UsersService.Services;
using UsersService.Services.MapperProfiles;
using UsersService.Services.Messages.Consumers;
using UsersServiceApi.MapperProfiles;
using UsersServiceApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("UserInfoConnection");

var connectionStringAzure = "Endpoint=sb://document-flow.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=+xItHOUEoMT6c9//vrNA93XfJQmOAZykczU7zkfeXKI=";
var newPurchaseTopic = "new-user-topic";
var subscriptionName = "userinfo-service";

// Add services to the container.

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

builder.Services.AddDbContext<UsersContext>(opt =>
    opt.UseSqlServer(connectionString, b => b.MigrationsAssembly("UsersService.DataAccess")));

builder.Services.AddAutoMapper(typeof(ServiceProfile), typeof(ControllerProfile));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepositoty, UserRepository>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("UserInfoConnection"));
builder.Services.AddHealthChecksUI().AddInMemoryStorage();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "UsersService API",
        Description = "An ASP.NET Core Web API for managing UsersService items"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddLogging(cfg => cfg.AddConsole());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI();

app.UseHttpsRedirection();

app.UseStatusCodePages();
app.ConfigureCustomExceptionMiddleware();
app.MapControllers();

app.Run();