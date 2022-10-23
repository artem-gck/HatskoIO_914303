using CompanyManagementService.DataAccess.Interfaces;
using CompanyManagementService.DataAccess.Realisation;
using CompanyManagementService.Services.Interfaces;
using CompanyManagementService.Services.MapperProfiles;
using CompanyManagementService.Services.Realisation;
using CompanyManagementServiceApi.MapperProfiles;
using CompanyManagementServiceApi.Middlewares;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
var departmentsConnectionString = builder.Configuration.GetConnectionString("Departments");
var usersStructureConnectionString = builder.Configuration.GetConnectionString("UsersStructure");
var positionsConnectionString = builder.Configuration.GetConnectionString("Positions");
var usersInfoConnectionString = builder.Configuration.GetConnectionString("UsersInfo");

var clientHandler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
};

// Add services to the container.

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddHealthChecks().AddRedis(redisConnectionString);

builder.Services.AddAutoMapper(typeof(ServicesProfile), typeof(ControllerProfile));

builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = redisConnectionString;
    options.InstanceName = "CompanyManagementService_";
});

builder.Services.AddScoped<IDepartmentAccess, DepartmentAccess>();
builder.Services.AddScoped<IPositionsAccess, PositionsAccess>();
builder.Services.AddScoped<IUserInfoAccess, UserInfoAccess>();
builder.Services.AddScoped<IUserStructureAccess, UserStructureAccess>();

builder.Services.AddHttpClient<IDepartmentAccess, DepartmentAccess>(httpClient => { httpClient.BaseAddress = new Uri(departmentsConnectionString); }).ConfigurePrimaryHttpMessageHandler(() => clientHandler);
builder.Services.AddHttpClient<IUserStructureAccess, UserStructureAccess>(httpClient => { httpClient.BaseAddress = new Uri(usersStructureConnectionString); }).ConfigurePrimaryHttpMessageHandler(() => clientHandler);
builder.Services.AddHttpClient<IPositionsAccess, PositionsAccess>(httpClient => { httpClient.BaseAddress = new Uri(positionsConnectionString); }).ConfigurePrimaryHttpMessageHandler(() => clientHandler);
builder.Services.AddHttpClient<IUserInfoAccess, UserInfoAccess>(httpClient => { httpClient.BaseAddress = new Uri(usersInfoConnectionString); }).ConfigurePrimaryHttpMessageHandler(() => clientHandler);

builder.Services.AddScoped<IStructureService, StructureService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Company management API",
        Description = "An ASP.NET Core Web API for company management"
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

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();
app.ConfigureCustomExceptionMiddleware();
app.UseAuthorization();

app.MapControllers();

app.Run();
