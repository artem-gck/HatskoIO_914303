using DocumentCrudService.Application.Services.Dto;
using DocumentCrudService.Application.Services.Queries;
using DocumentCrudService.Infrastructure.Realisation.Queries.GetAllNamesOfDocuments;
using DocumentCrudService.Infrastructure.Realisation.Queries.GetDocumentByName;
using DocumentCrudService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using IResult = DocumentCrudService.Application.Services.Results.IResult;

namespace DocumentCrudService.Controllers
{
    [Route("document-names")]
    public class DocumentNameController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public DocumentNameController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllNamesOfDocumentsQuery();

            var listOfFileName = await _queryDispatcher.Send(query);
            var listOfFileNameViewModel = MapToDocumentViewModel(listOfFileName);

            return Ok(listOfFileNameViewModel);
        }

        [HttpGet("{name}/{version}")]
        public async Task<IActionResult> Get(string name, int version = -1)
        {
            var query = new GetDocumentByNameQuery()
            {
                FileName = name,
                Version = version
            };

            var listOfFileName = await _queryDispatcher.Send(query);
            var listOfFileNameViewModel = MapToDocumentViewModel(listOfFileName);

            return Ok(listOfFileNameViewModel);
        }

        private List<DocumentNameViewModel> MapToDocumentViewModel(IList<IResult> input)
        {
            var listOfFileNameViewModel = new List<DocumentNameViewModel>();

            foreach (var item in input)
            {
                var fileNameDto = (DocumentNameDto)item;

                var fileNameViewModel = new DocumentNameViewModel()
                {
                    DocumentName = fileNameDto.DocumentName,
                    Id = fileNameDto.Id,
                    UploadDate = fileNameDto.UploadDate,
                };

                listOfFileNameViewModel.Add(fileNameViewModel);
            }

            return listOfFileNameViewModel;
        }
    }
}
