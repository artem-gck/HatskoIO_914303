using SignatureService.DataAccess.DataBase;
using SignatureService.DataAccess.DataBase.Interfaces;
using SignatureService.DataAccess.DataBase.Realisations;
using SignatureService.Services.Interfaces;
using SignatureService.Services.Realisations;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

var dBConnectionString = builder.Configuration.GetConnectionString("SqlServer");

// Add services to the container.

builder.Services.AddSingleton(new SqlServerConnectionProvider(dBConnectionString));

builder.Services.AddScoped<IUserRepository, UserRepository>();
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
