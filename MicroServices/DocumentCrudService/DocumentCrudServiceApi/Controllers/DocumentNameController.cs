using DocumentCrudService.Cqrs.Dto;
using DocumentCrudService.Cqrs.Queries;
using DocumentCrudService.Cqrs.Realisation.Queries.GetAllNamesOfDocuments;
using DocumentCrudService.Cqrs.Realisation.Queries.GetAllNamesOfDocumentsByUserId;
using DocumentCrudService.Cqrs.Realisation.Queries.GetByDocumentId;
using DocumentCrudService.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IResult = DocumentCrudService.Cqrs.Results.IResult;

namespace DocumentCrudService.Controllers
{
    [Route("api/document-names")]
    [Produces("application/json")]
    [Authorize]
    public class DocumentNameController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public DocumentNameController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
        }

        /// <summary>
        /// Gets names of documents.
        /// </summary>
        /// <param name="page">The number of page.</param>
        /// <param name="count">The count of record at page.</param>
        /// <returns>Names of files</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/document-names?page=1 count=10
        ///
        /// </remarks>
        /// <response code="200">Send file</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int page = 1, int count = 10)
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

        [HttpGet("~/api/users/{userId}/document-names")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid userId, int? count)
        {
            var query = new GetAllNamesOfDocumentsByUserIdQuery()
            {
                CreatorId = userId,
                Count = count
            };

            var listOfFileName = await _queryDispatcher.Send(query);
            var listOfFileNameViewModel = MapToDocumentViewModel(listOfFileName);

            return Ok(listOfFileNameViewModel);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetByDocumentIdQuery()
            {
                Id = id
            };

            var listOfFileName = await _queryDispatcher.Send(query);
            var listOfFileNameViewModel = MapToDocumentViewModel(listOfFileName);

            return Ok(listOfFileNameViewModel.FirstOrDefault());
        }

        private List<DocumentNameResponce> MapToDocumentViewModel([FromBody] IList<IResult> input)
        {
            var listOfFileNameViewModel = new List<DocumentNameResponce>();

            foreach (var item in input)
            {
                var fileNameDto = (DocumentNameDto)item;

                var fileNameViewModel = new DocumentNameResponce()
                {
                    Name = fileNameDto.Name,
                    Version = fileNameDto.Version,
                    CreatorId = fileNameDto.CreatorId,
                    Id = fileNameDto.Id,
                    UploadDate = fileNameDto.UploadDate,
                };

                listOfFileNameViewModel.Add(fileNameViewModel);
            }

            return listOfFileNameViewModel;
        }
    }
}
