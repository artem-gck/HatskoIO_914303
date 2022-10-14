using CompanyManagementService.DataAccess.Interfaces;
using CompanyManagementService.DataAccess.Realisation;
using CompanyManagementService.Services.Interfaces;
using CompanyManagementService.Services.MapperProfiles;
using CompanyManagementService.Services.Realisation;
using CompanyManagementServiceApi.MapperProfiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient("departments", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7130/api/departments/");
});
builder.Services.AddHttpClient("positions", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7130/api/positions/");
});
builder.Services.AddHttpClient("userInfo", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7221/api/");
});

builder.Services.AddAutoMapper(typeof(ServicesProfile), typeof(ControllerProfile));

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IPositionsRepository, PositionsRepository>();
builder.Services.AddScoped<IUserInfoRepository, UserInfoRepository>();
builder.Services.AddScoped<IUserStructureRepository, UserStructureRepository>();

builder.Services.AddScoped<IStructureService, StructureService>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
