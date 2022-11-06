using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using System.Reflection;
using TaskCrudService.Adapters.DataSource;
using TaskCrudService.Adapters.DataSource.Context;
using TaskCrudService.Adapters.Output;
using TaskCrudService.Domain.Entities;
using TaskCrudService.Middlewares;
using TaskCrudService.Ports.Output;
using TaskCrudService.Posts.DataSource;
using TaskCrudServiceApi.MapperProfiles;
using TaskCrudServiceApi.SwaggerConfiguration;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using FluentValidation;
using Microsoft.OpenApi.Models;
using TaskCrudServiceApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MassTransit;
using Messages;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("TasksConnection") ?? builder.Configuration.GetConnectionString("TasksConnection");
var identityString = Environment.GetEnvironmentVariable("IdentityPath") ?? builder.Configuration["IdentityPath"];
var connectionStringAzure = Environment.GetEnvironmentVariable("ServiceBus") ?? builder.Configuration.GetConnectionString("ServiceBus");
var newTaskQueue = builder.Configuration["Queues:NewTask"];
var updateTaskQueue = builder.Configuration["Queues:UpdateTask"];
var deleteTaskQueue = builder.Configuration["Queues:DeleteTask"];

// Add services to the container.

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddValidatorsFromAssembly(Assembly.Load("TaskCrudServiceApi"));
builder.Services.AddHealthChecks()
                .AddSqlServer(connectionString);

builder.Services.AddAutoMapper(typeof(ApiProfile));

builder.Services.AddDbContext<TaskContext>(opt =>
    opt.UseSqlServer(connectionString, b => b.MigrationsAssembly("TaskCrudService.Adapters.DataSource")));

builder.Services.AddScoped<IRepository<TaskEntity>, TasksRepository>();
builder.Services.AddScoped<IService<TaskEntity>, TaskService>();

builder.Services.AddMassTransit(serviceCollectionConfigurator =>
{
    serviceCollectionConfigurator.AddBus(registrationContext => Bus.Factory.CreateUsingAzureServiceBus(configurator =>
    {
        configurator.Host(connectionStringAzure);

        configurator.Message<NewTaskMessage>(m =>
        {
            m.SetEntityName(newTaskQueue);
        });

        configurator.Message<UpdateTaskMessage>(m =>
        {
            m.SetEntityName(updateTaskQueue);
        });

        configurator.Message<DeleteTaskMessage>(m =>
        {
            m.SetEntityName(deleteTaskQueue);
        });
    }));
});

builder.Services.AddControllers();
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
});
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

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
    options.Audience = "task_api";
    options.BackchannelHttpHandler = clientHandler;
});

// adds an authorization policy to make sure the token is for scope 'api1'
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TasksScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "task_api");
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{identityString}/connect/authorize"),
                TokenUrl = new Uri($"{identityString}/connect/token"),
                Scopes = new Dictionary<string, string> { { "task_api", "task api" } }
            }
        }
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId("task_api");
        options.OAuthAppName("Tasks api");
        //setup.OAuthScopeSeparator(" ");
        options.OAuthUsePkce();

        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.ConfigureCustomExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
