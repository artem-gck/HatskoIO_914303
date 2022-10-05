using DocumentCrudService.Cqrs.Dto;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Realisation.Queries.GetAllNamesOfDocuments;
using DocumentCrudService.Cqrs.Realisation.Queries.GetDocumentByName;
using DocumentCrudService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using IResult = DocumentCrudService.Cqrs.Results.IResult;

namespace DocumentCrudService.Controllers
{
    [Route("document-names")]
    [Produces("application/json")]
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
            var documentDto = (DocumentDto)listOfFileName[0];

            return File(documentDto.DocumentBody, "application/octet-stream", documentDto.FileName);
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
