using Microsoft.EntityFrameworkCore;
using TaskCrudService.Domain.Realisation.Context;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TaskConnection");

// Add services to the container.

builder.Services.AddDbContext<TaskContext>(opt =>
    opt.UseSqlServer(connectionString, b => b.MigrationsAssembly("TaskCrudService.Domain.Realisation")));

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
