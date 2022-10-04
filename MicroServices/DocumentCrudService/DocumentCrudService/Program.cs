using DocumentCrudService.Application.DbServices;
using DocumentCrudService.Application.Services.Commands;
using DocumentCrudService.Application.Services.Queries;
using DocumentCrudService.Infrastructure.DbRrealisation;
using DocumentCrudService.Infrastructure.DbRrealisation.Context;
using DocumentCrudService.Infrastructure.Realisation.Commands;
using DocumentCrudService.Infrastructure.Realisation.Commands.AddDocument;
using DocumentCrudService.Infrastructure.Realisation.Commands.DeleteDocument;
using DocumentCrudService.Infrastructure.Realisation.Commands.UpdateDocument;
using DocumentCrudService.Infrastructure.Realisation.Queries;
using DocumentCrudService.Infrastructure.Realisation.Queries.GetAllNamesOfDocuments;
using DocumentCrudService.Infrastructure.Realisation.Queries.GetDocumentById;
using DocumentCrudService.Infrastructure.Realisation.Queries.GetDocumentByName;
using DocumentCrudService.Infrastructure.Realisation.Queries.GetNameOfDocumentById;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<DocumentContext>();

builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IDocumentNameRepository, DocumentNameRepository>();

builder.Services.AddScoped<ICommandHandler<AddDocumentCommand>, AddDocumentCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteDocumentCommand>, DeleteDocumentCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateDocumentCommand>, UpdateDocumentCommandHandler>();

builder.Services.AddScoped<IQueryHandler<GetAllNamesOfDocumentsQuery>, GetAllNamesOfDocumentsQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetDocumentByIdQuery>, GetDocumentByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetDocumentByNameQuery>, GetDocumentByNameQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetNameOfDocumentByIdQuery>, GetNameOfDocumentByIdQueryHandler>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
