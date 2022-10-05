using DocumentCrudService.Cqrs.Commands;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Realisation.Commands;
using DocumentCrudService.Cqrs.Realisation.Commands.AddDocument;
using DocumentCrudService.Cqrs.Realisation.Commands.DeleteDocument;
using DocumentCrudService.Cqrs.Realisation.Commands.UpdateDocument;
using DocumentCrudService.Cqrs.Realisation.Queries;
using DocumentCrudService.Cqrs.Realisation.Queries.GetAllNamesOfDocuments;
using DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentById;
using DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentByName;
using DocumentCrudService.Repositories.DbServices;
using DocumentCrudService.Repositories.Realisation;
using DocumentCrudService.Repositories.Realisation.Context;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
builder.Services.AddScoped<IQueryHandler<GetDocumentByNameQuery>, GetDocumentByNameQueryHandler>();

builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();

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

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
