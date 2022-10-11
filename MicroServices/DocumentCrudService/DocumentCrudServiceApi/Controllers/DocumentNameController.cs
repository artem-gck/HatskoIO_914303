using DocumentCrudService.Cqrs.Dto;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Realisation.Queries.GetAllNamesOfDocuments;
using DocumentCrudService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using IResult = DocumentCrudService.Cqrs.Results.IResult;

namespace DocumentCrudService.Controllers
{
    [Route("api/document-names")]
    [Produces("application/json")]
    public class DocumentNameController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public DocumentNameController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
        }

        /// <summary>
        /// Gets all names of documents.
        /// </summary>
        /// <returns>Names of files</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/document-names
        ///
        /// </remarks>
        /// <response code="200">Send file</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int page, int count)
        {
            var query = new GetAllNamesOfDocumentsQuery()
            {
                NumberOfPage = page,
                CountOnPage = count
            };

            var listOfFileName = await _queryDispatcher.Send(query);
            var listOfFileNameViewModel = MapToDocumentViewModel(listOfFileName);

            return Ok(listOfFileNameViewModel);
        }

        private List<DocumentNameResponce> MapToDocumentViewModel(IList<IResult> input)
        {
            var listOfFileNameViewModel = new List<DocumentNameResponce>();

            foreach (var item in input)
            {
                var fileNameDto = (DocumentNameDto)item;

                var fileNameViewModel = new DocumentNameResponce()
                {
                    Name = fileNameDto.Name,
                    Version = fileNameDto.Version,
                    Id = fileNameDto.Id,
                    UploadDate = fileNameDto.UploadDate,
                };

                listOfFileNameViewModel.Add(fileNameViewModel);
            }

            return listOfFileNameViewModel;
        }
    }
}
