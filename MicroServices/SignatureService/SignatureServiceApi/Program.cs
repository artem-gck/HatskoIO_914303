using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SignatureService.DataAccess.DataBase;
using SignatureService.DataAccess.DataBase.Interfaces;
using SignatureService.DataAccess.DataBase.Realisations;
using SignatureService.DataAccess.Http.Interfaces;
using SignatureService.DataAccess.Http.Realisation;
using SignatureService.Services.Interfaces;
using SignatureService.Services.Realisations;

var builder = WebApplication.CreateBuilder(args);

var dBConnectionString = builder.Configuration.GetConnectionString("SqlServer");
var documentsConnectionString = builder.Configuration.GetConnectionString("DocumentService");

// Add services to the container.

builder.Services.AddHealthChecks().AddSqlServer(dBConnectionString);

builder.Services.AddSingleton(new SqlServerConnectionProvider(dBConnectionString));

builder.Services.AddScoped<IDocumentAccess, DocumentAccess>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISignatureRepository, SignatureRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISignService, SignService>();

builder.Services.AddHttpClient<IDocumentAccess, DocumentAccess>(HttpClient => HttpClient.BaseAddress = new Uri(documentsConnectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapControllers();

app.Run();
