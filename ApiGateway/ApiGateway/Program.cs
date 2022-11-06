using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.RequestId;
using Ocelot.Values;
using System.Net;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);
var identityString = Environment.GetEnvironmentVariable("IdentityPath") ?? builder.Configuration["IdentityPath"];

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

var clientHandler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true 
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
    options.Audience = "gateway";
    options.BackchannelHttpHandler = clientHandler;
});

// adds an authorization policy to make sure the token is for scope 'api1'
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GatewayScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "gateway");
    });
});

builder.Services.AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

await app.UseOcelot();

ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

app.UseHttpsRedirection();

app.Run();