using DocumentCrudService.Cqrs.Commands;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Realisation.Commands;
using DocumentCrudService.Cqrs.Realisation.Commands.AddDocument;
using DocumentCrudService.Cqrs.Realisation.Commands.DeleteDocument;
using DocumentCrudService.Cqrs.Realisation.Commands.UpdateDocument;
using DocumentCrudService.Cqrs.Realisation.Queries;
using DocumentCrudService.Cqrs.Realisation.Queries.GetAllNamesOfDocuments;
using DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentById;
using DocumentCrudService.Repositories.DbServices;
using DocumentCrudService.Repositories.Realisation;
using DocumentCrudService.Repositories.Realisation.Context;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog.Events;
using Serilog;
using System.Reflection;
using DocumentCrudServiceApi.Middlewares;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var identityString = builder.Configuration.GetValue<string>("IdentityPath");

// Add services to the container.

var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("HealthChecks", LogEventLevel.Warning)
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddHealthChecks().AddMongoDb(builder.Configuration.GetConnectionString("MongoDb"));
builder.Services.AddHealthChecksUI().AddInMemoryStorage();

builder.Services.AddScoped<DocumentContext>();

builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IDocumentNameRepository, DocumentNameRepository>();

builder.Services.AddScoped<ICommandHandler<AddDocumentCommand>, AddDocumentCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteDocumentCommand>, DeleteDocumentCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateDocumentCommand>, UpdateDocumentCommandHandler>();

builder.Services.AddScoped<IQueryHandler<GetAllNamesOfDocumentsQuery>, GetAllNamesOfDocumentsQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetDocumentByIdQuery>, GetDocumentByIdQueryHandler>();

builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();

builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = identityString;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

// adds an authorization policy to make sure the token is for scope 'api1'
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DocumentsScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "admin_api");
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Document CRUD service API",
        Description = "An ASP.NET Core Web API for managing documents items",
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.ConfigureCustomExceptionMiddleware();

app.Run();
