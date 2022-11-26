using HealthChecks.UI.Client;
using MassTransit;
using Messages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using SignatureService.DataAccess.DataBase;
using SignatureService.DataAccess.DataBase.Helpers;
using SignatureService.DataAccess.DataBase.Interfaces;
using SignatureService.DataAccess.DataBase.Realisations;
using SignatureService.DataAccess.Http.Interfaces;
using SignatureService.DataAccess.Http.Realisation;
using SignatureService.Services.Interfaces;
using SignatureService.Services.Messages.Consumers;
using SignatureService.Services.Realisations;
using SignatureServiceApi;
using SignatureServiceApi.Middlewares;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var dBConnectionString = Environment.GetEnvironmentVariable("SignaturesConnection") ?? builder.Configuration.GetConnectionString("SignaturesConnection");
var documentsConnectionString = Environment.GetEnvironmentVariable("DocumentServiceConnection") ?? builder.Configuration.GetConnectionString("DocumentServiceConnection");
var identityString = Environment.GetEnvironmentVariable("IdentityPath") ?? builder.Configuration["IdentityPath"];

var connectionString = Environment.GetEnvironmentVariable("ServiceBus") ?? builder.Configuration.GetConnectionString("ServiceBus");
var newUserTopic = builder.Configuration["Topics:NewUser"];
var subscriptionName = builder.Configuration["SubscriptionName"];

// Add services to the container. 

builder.Services.AddMassTransit(serviceCollectionConfigurator =>
{
    serviceCollectionConfigurator.AddConsumer<NewUserConsumer>();
    
    serviceCollectionConfigurator.AddBus(registrationContext => Bus.Factory.CreateUsingAzureServiceBus(configurator => 
        {
            configurator.Host(connectionString);

            configurator.Message<NewUserMessage>(m => { m.SetEntityName(newUserTopic); });

            configurator.SubscriptionEndpoint<NewUserMessage>(subscriptionName, endpointConfigurator =>
            {
                endpointConfigurator.ConfigureConsumer<NewUserConsumer>(registrationContext);
            });
        }));
});

builder.Services.AddHealthChecks().AddSqlServer(dBConnectionString);

builder.Services.AddScoped<IDocumentAccess, DocumentAccess>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISignatureRepository, SignatureRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISignService, SignService>();

var clientHandler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
};

builder.Services.AddSingleton(dBConnectionString);
builder.Services.AddSingleton<IConnectionProvider, SqlServerConnectionProvider>();
builder.Services.AddHttpClient<IDocumentAccess, DocumentAccess>(HttpClient => HttpClient.BaseAddress = new Uri(documentsConnectionString)).ConfigurePrimaryHttpMessageHandler(() => clientHandler);

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
    options.Audience = "signature_api";
    options.BackchannelHttpHandler = clientHandler;
});

// adds an authorization policy to make sure the token is for scope 'api1'
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SignatureScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "signature_api");
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
        Title = "Signature API",
        Description = "An ASP.NET Core Web API for managing signature items",
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
                Scopes = new Dictionary<string, string> { { "signature_api", "signature api" } }
            }
        }
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

var serviceProvider = builder.Services.BuildServiceProvider();
await CreateDbHelper.CreateDb(serviceProvider);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setup =>
    {
        setup.SwaggerEndpoint($"/swagger/v1/swagger.json", "Version 1.0");
        setup.OAuthClientId("signature_api");
        setup.OAuthAppName("Sihnature api");
        //setup.OAuthScopeSeparator(" ");
        setup.OAuthUsePkce();
    });
}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureCustomExceptionMiddleware();
app.MapControllers();

app.Run();

public partial class Program { }