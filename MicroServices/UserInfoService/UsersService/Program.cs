using Microsoft.EntityFrameworkCore;
using UsersService.DataAccess;
using UsersService.DataAccess.Entities.Context;
using UsersService.MapperProfiles;
using UsersService.Middlewares;
using UsersService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("UserInfoConnection");

builder.Services.AddDbContext<UsersInfoContext>(opt =>
    opt.UseSqlServer(connectionString, b => b.MigrationsAssembly("UsersService")));

builder.Services.AddAutoMapper(typeof(ServiceProfile), typeof(ControllerProfile));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserAccess, UserAccess>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStatusCodePages();
app.ConfigureCustomExceptionMiddleware();
app.MapControllers();

app.Run();