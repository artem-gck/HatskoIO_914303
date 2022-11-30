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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StructureServiceApi;

var builder = WebApplication.CreateBuilder(args);
var connectionString = Environment.GetEnvironmentVariable("StructureConnection") ?? builder.Configuration.GetConnectionString("StructureConnection");

var connectionStringAzure = Environment.GetEnvironmentVariable("ServiceBus") ?? builder.Configuration.GetConnectionString("ServiceBus");
var newUserTopic = builder.Configuration["Topics:NewUser"];
var updateUserQueue = builder.Configuration["Queues:UpdateUser"];
var subscriptionName = builder.Configuration["SubscriptionName"];
var identityString = Environment.GetEnvironmentVariable("IdentityPath") ?? builder.Configuration["IdentityPath"];

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
            m.SetEntityName(newUserTopic);
        });

        configurator.Message<UpdateUserMessage>(m =>
        {
            m.SetEntityName(updateUserQueue);
        });

        configurator.SubscriptionEndpoint<NewUserMessage>(subscriptionName, endpointConfigurator =>
        {
            endpointConfigurator.ConfigureConsumer<NewUserConsumer>(registrationContext);
        });
    }));
});

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

var clientHandler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
};

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
    options.Audience = "structure_api";
    options.BackchannelHttpHandler = clientHandler;
});

// adds an authorization policy to make sure the token is for scope 'api1'
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StructuresScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "structure_api");
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
        Title = "StructureService API",
        Description = "An ASP.NET Core Web API for managing structure items"
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
                Scopes = new Dictionary<string, string> { { "structure_api", "structure api" } }
            }
        }
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setup =>
    {
        setup.SwaggerEndpoint($"/swagger/v1/swagger.json", "Version 1.0");
        setup.OAuthClientId("structure_api");
        setup.OAuthAppName("Structure api");
        //setup.OAuthScopeSeparator(" ");
        setup.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

app.ConfigureCustomExceptionMiddleware();
app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
