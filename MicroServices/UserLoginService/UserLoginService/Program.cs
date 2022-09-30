<<<<<<< Updated upstream
=======
using UserLoginService.Application.Services;
using UserLoginService.Application.Realisation;
using UserLoginService.Application.Realisation.MapperProfiles;
using UserLoginService.Domain.Entities;
using UserLoginService.Domain.Realisation;
using UserLoginService.Domain.Services;
using UserLoginService.MapperProfiles;
using Microsoft.EntityFrameworkCore;
using UserLoginService.Domain.Realisation.Context;

>>>>>>> Stashed changes
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UserLoginConnection");

// Add services to the container.

<<<<<<< Updated upstream
=======
builder.Services.AddAutoMapper(typeof(ApplicationProfile), typeof(ControllerProfile));

builder.Services.AddDbContext<UserLoginContext>(opt =>
    opt.UseSqlServer(connectionString, b => b.MigrationsAssembly("UserLoginService.Domain.Realisation")));

builder.Services.AddScoped<IRepository<UserLoginEntity>, UserLoginRepository>();
builder.Services.AddScoped<IRepository<RoleEntity>, RoleRopository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();

>>>>>>> Stashed changes
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
