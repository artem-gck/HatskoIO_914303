using StructureService.Application.Realisation.MapperProfiles;
using StructureService.Application.Realisation;
using StructureService.Application.Services;
using StructureService.Domain.Services;
using StructureService.MapperProfiles;
using StructureService.Middlewares;
using StructureService.Dimain.Realisation.Context;
using Microsoft.EntityFrameworkCore;
using StructureService.Domain.Entities;
using StructureService.Dimain.Realisation;
using StructureService.Application.Services.Dto;
using Microsoft.OpenApi.Models;
using System.Reflection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("StructureConnection");

// Add services to the container.

builder.Services.AddHealthChecksUI()
                .AddInMemoryStorage();
builder.Services.AddHealthChecks()
                .AddSqlServer(connectionString);

builder.Services.AddAutoMapper(typeof(ApplicationProfile), typeof(ControllerProfile));

builder.Services.AddDbContext<StructureContext>(opt =>
    opt.UseSqlServer(connectionString, b => b.MigrationsAssembly("StructureService.Dimain.Realisation")));

builder.Services.AddScoped<IRepository<PositionEntity>, Repository<PositionEntity>>();
builder.Services.AddScoped<IRepository<DepartmentEntity>, Repository<DepartmentEntity>>();
builder.Services.AddScoped<IRepository<DepartmentUnitEntity>, Repository<DepartmentUnitEntity>>();

builder.Services.AddScoped<IService<PositionDto>, Service<PositionDto, PositionEntity>>();
builder.Services.AddScoped<IService<DepartmentDto>, Service<DepartmentDto, DepartmentEntity>>();
builder.Services.AddScoped<IService<DepartmentUnitDto>, Service<DepartmentUnitDto, DepartmentUnitEntity>>();

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

app.UseHttpsRedirection();

app.UseAuthorization();
app.ConfigureCustomExceptionMiddleware();
app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI();

app.Run();
