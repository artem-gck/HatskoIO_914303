using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using UsersService.DataAccess;
using UsersService.DataAccess.Entities.Context;
using UsersService.MapperProfiles;
using UsersService.Middlewares;
using UsersService.Services;
using UsersService.Services.MapperProfiles;

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
builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("UserInfoConnection"));
builder.Services.AddHealthChecksUI().AddInMemoryStorage();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(cfg => cfg.AddConsole());

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI();

app.UseHttpsRedirection();

app.UseStatusCodePages();
app.ConfigureCustomExceptionMiddleware();
app.MapControllers();

app.Run();