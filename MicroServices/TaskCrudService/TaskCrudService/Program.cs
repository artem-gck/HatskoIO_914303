using Microsoft.EntityFrameworkCore;
using TaskCrudService.Application.Realisation;
using TaskCrudService.Application.Realisation.MapperProfiles;
using TaskCrudService.Application.Services;
using TaskCrudService.Application.Services.Dto;
using TaskCrudService.Domain.Entities;
using TaskCrudService.Domain.Realisation;
using TaskCrudService.Domain.Realisation.Context;
using TaskCrudService.Domain.Services;
using TaskCrudService.MapperProfiles;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TaskConnection");

// Add services to the container.
builder.Services.AddAutoMapper(typeof(ApplicationProfile), typeof(ControllerProfile));

builder.Services.AddDbContext<TaskContext>(opt =>
    opt.UseSqlServer(connectionString, b => b.MigrationsAssembly("TaskCrudService.Domain.Realisation")));

builder.Services.AddScoped<IRepository<TaskEntity>, TasksRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IService<TaskDto>, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();

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
