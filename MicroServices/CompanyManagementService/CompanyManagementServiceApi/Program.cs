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

// Add services to the container.

builder.Services.AddHealthChecks();
builder.Services.AddHealthChecksUI().AddInMemoryStorage();

builder.Services.AddAutoMapper(typeof(ServicesProfile), typeof(ControllerProfile));

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IPositionsRepository, PositionsRepository>();
builder.Services.AddScoped<IUserInfoRepository, UserInfoRepository>();
builder.Services.AddScoped<IUserStructureRepository, UserStructureRepository>();

builder.Services.AddHttpClient<IDepartmentRepository, DepartmentRepository>(httpClient => { httpClient.BaseAddress = new Uri("https://localhost:7130/api/departments/"); });
builder.Services.AddHttpClient<IUserStructureRepository, UserStructureRepository>(httpClient => { httpClient.BaseAddress = new Uri("https://localhost:7130/api/departments/"); });
builder.Services.AddHttpClient<IPositionsRepository, PositionsRepository>(httpClient => { httpClient.BaseAddress = new Uri("https://localhost:7130/api/positions/"); });
builder.Services.AddHttpClient<IUserInfoRepository, UserInfoRepository>(httpClient => { httpClient.BaseAddress = new Uri("https://localhost:7221/api/"); });

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
app.MapHealthChecksUI();

app.UseHttpsRedirection();
app.ConfigureCustomExceptionMiddleware();
app.UseAuthorization();

app.MapControllers();

app.Run();
