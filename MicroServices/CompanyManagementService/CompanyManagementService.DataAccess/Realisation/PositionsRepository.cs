using CompanyManagementService.DataAccess.Exceptions;
using CompanyManagementService.DataAccess.Interfaces;
using CompanyManagementService.DataAccess.StructureEntities.AddRequest;
using CompanyManagementService.DataAccess.StructureEntities.Responce;
using CompanyManagementService.DataAccess.StructureEntities.UpdateRequest;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net;

namespace CompanyManagementService.DataAccess.Realisation
{
    public class PositionsRepository : IPositionsRepository
    {
        private const string ClientName = "positions";
        private const string ServiceName = "Position";

        private IHttpClientFactory _httpClientFactory;

        public PositionsRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task Delete(Guid id)
        {
            using var client = _httpClientFactory.CreateClient(ClientName);
            var answer = await client.DeleteAsync($"{id}");

            if (answer.IsSuccessStatusCode)
                return;

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound             => new NotFoundException(id),
                HttpStatusCode.InternalServerError  => new InternalServerException(ServiceName)
            };
        }

        public async Task<PositionResponce> Get(Guid id)
        {
            using var client = _httpClientFactory.CreateClient(ClientName);
            var answer = await client.GetAsync($"{id}");

            if (answer.IsSuccessStatusCode)
            {
                var positionResponce = await answer.Content.ReadAsStringAsync();
                var position = JsonConvert.DeserializeObject<PositionResponce>(positionResponce);

                return position;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound             => new NotFoundException(id),
                HttpStatusCode.InternalServerError  => new InternalServerException(ServiceName)
            };
        }

        public async Task<IEnumerable<PositionResponce>> Get()
        {
            using var client = _httpClientFactory.CreateClient(ClientName);
            var answer = await client.GetAsync(string.Empty);

            if (answer.IsSuccessStatusCode)
            {
                var positionsResponce = await answer.Content.ReadAsStringAsync();
                var positions = JsonConvert.DeserializeObject<IEnumerable<PositionResponce>>(positionsResponce);

                return positions;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.InternalServerError => new InternalServerException(ServiceName)
            };
        }

        public async Task<Guid> Post(AddPositionRequest addPositionRequest)
        {
            using var client = _httpClientFactory.CreateClient(ClientName);
            var answer = await client.PostAsJsonAsync(string.Empty, addPositionRequest);

            if (answer.IsSuccessStatusCode)
            {
                var idResponce = await answer.Content.ReadAsStringAsync();
                var id = Guid.Parse(idResponce);

                return id;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.BadRequest => new InvalidModelStateException(nameof(addPositionRequest)),
                HttpStatusCode.Conflict => new DbUpdateException(),
                HttpStatusCode.InternalServerError => new InternalServerException(ServiceName)
            };
        }

        public async Task Put(Guid id, UpdatePositionRequest updatePositionRequest)
        {
            using var client = _httpClientFactory.CreateClient(ClientName);
            var answer = await client.PutAsJsonAsync($"{id}", updatePositionRequest);

            if (answer.IsSuccessStatusCode)
                return;

            throw answer.StatusCode switch
            {
                HttpStatusCode.BadRequest => new InvalidModelStateException(nameof(updatePositionRequest)),
                HttpStatusCode.NotFound => new NotFoundException(id),
                HttpStatusCode.Conflict => new DbUpdateException(),
                HttpStatusCode.InternalServerError => new InternalServerException(ServiceName)
            };
        }
    }
}
