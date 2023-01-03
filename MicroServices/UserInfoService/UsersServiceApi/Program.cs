using HealthChecks.UI.Client;
using MassTransit;
using Messages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using UsersService.DataAccess;
using UsersService.DataAccess.Entities.Context;
using UsersService.Services;
using UsersService.Services.MapperProfiles;
using UsersServiceApi;
using UsersServiceApi.MapperProfiles;
using UsersServiceApi.Middlewares;
using UsersService.Services.Messages.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionStringUserInfo = Environment.GetEnvironmentVariable("UserInfoConnection") ?? builder.Configuration.GetConnectionString("UserInfoConnection");

var connectionStringServiceBus = Environment.GetEnvironmentVariable("ServiceBus") ?? builder.Configuration.GetConnectionString("ServiceBus");
var newUserTopic = builder.Configuration["Topics:NewUser"];
var updateEmailUserTopic = builder.Configuration["Topics:UpdateEmailUser"];
var updateUserQueue = builder.Configuration["Queues:UpdateUser"];
var subscriptionName = builder.Configuration["SubscriptionName"];
var identityString = Environment.GetEnvironmentVariable("IdentityPath") ?? builder.Configuration["IdentityPath"];


// Add services to the container.

builder.Services.AddMassTransit(serviceCollectionConfigurator =>
{
    serviceCollectionConfigurator.AddConsumer<NewUserConsumer>();
    serviceCollectionConfigurator.AddConsumer<UpdateUserConsumer>();
    serviceCollectionConfigurator.AddConsumer<UpdateEmailUserConsumer>();

    serviceCollectionConfigurator.AddBus(registrationContext => Bus.Factory.CreateUsingAzureServiceBus(configurator =>
    {
        configurator.Host(connectionStringServiceBus);

        configurator.Message<NewUserMessage>(m =>
        {
            m.SetEntityName(newUserTopic);
        });

        configurator.Message<UpdateEmailUserMessage>(m =>
        {
            m.SetEntityName(updateEmailUserTopic);
        });

        configurator.ReceiveEndpoint(updateUserQueue, endpointConfigurator =>
        {
            endpointConfigurator.ConfigureConsumer<UpdateUserConsumer>(registrationContext);
        });

        configurator.SubscriptionEndpoint<NewUserMessage>(subscriptionName, endpointConfigurator =>
        {
            endpointConfigurator.ConfigureConsumer<NewUserConsumer>(registrationContext);
        });

        configurator.SubscriptionEndpoint<UpdateEmailUserMessage>(subscriptionName, endpointConfigurator =>
        {
            endpointConfigurator.ConfigureConsumer<UpdateEmailUserConsumer>(registrationContext);
        });
    }));
});

builder.Services.AddDbContext<UsersContext>(opt =>
    opt.UseSqlServer(connectionStringUserInfo, b => b.MigrationsAssembly("UsersService.DataAccess")));

builder.Services.AddAutoMapper(typeof(ServiceProfile), typeof(ControllerProfile));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepositoty, UserRepository>();

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
    options.Audience = "userinfo_api";
    options.BackchannelHttpHandler = clientHandler;
});

// adds an authorization policy to make sure the token is for scope 'api1'
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserInfoScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "userinfo_api");
    });
});


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("UserInfoConnection"));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "UsersService API",
        Description = "An ASP.NET Core Web API for managing UsersService items"
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
                Scopes = new Dictionary<string, string> { { "userinfo_api", "userinfo api" } }
            }
        }
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddLogging(cfg => cfg.AddConsole());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setup =>
    {
        setup.SwaggerEndpoint($"/swagger/v1/swagger.json", "Version 1.0");
        setup.OAuthClientId("userinfo_api");
        setup.OAuthAppName("UserInfo api");
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

app.UseStatusCodePages();
app.ConfigureCustomExceptionMiddleware();
app.MapControllers();

app.Run();