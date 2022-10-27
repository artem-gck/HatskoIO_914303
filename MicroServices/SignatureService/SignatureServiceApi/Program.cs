using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using SignatureService.DataAccess.DataBase;
using SignatureService.DataAccess.DataBase.Helpers;
using SignatureService.DataAccess.DataBase.Interfaces;
using SignatureService.DataAccess.DataBase.Realisations;
using SignatureService.DataAccess.Http.Interfaces;
using SignatureService.DataAccess.Http.Realisation;
using SignatureService.Services.Interfaces;
using SignatureService.Services.Realisations;
using SignatureServiceApi.Middlewares;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var dBConnectionString = Environment.GetEnvironmentVariable("SqlServer") ?? builder.Configuration.GetConnectionString("SqlServer");
var documentsConnectionString = builder.Configuration.GetConnectionString("DocumentService");

// Add services to the container.

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

builder.Services.AddSingleton(new SqlServerConnectionProvider(dBConnectionString));
builder.Services.AddHttpClient<IDocumentAccess, DocumentAccess>(HttpClient => HttpClient.BaseAddress = new Uri(documentsConnectionString)).ConfigurePrimaryHttpMessageHandler(() => clientHandler);

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

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var serviceProvider = builder.Services.BuildServiceProvider();

await CreateDbHelper.CreateDb(serviceProvider);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();

app.UseAuthorization();
app.ConfigureCustomExceptionMiddleware();
app.MapControllers();

app.Run();
