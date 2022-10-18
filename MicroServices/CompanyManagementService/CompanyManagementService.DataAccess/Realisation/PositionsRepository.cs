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
        private HttpClient _httpClient;

        public PositionsRepository(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(PositionsRepository));
        }

        public async Task DeleteAsync(Guid id)
        {
            var answer = await _httpClient.DeleteAsync($"{id}");

            if (answer.IsSuccessStatusCode)
                return;

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound             => new NotFoundException(id),
                HttpStatusCode.InternalServerError  => new InternalServerException(nameof(PositionsRepository))
            };
        }

        public async Task<PositionResponce> GetAsync(Guid id)
        {
            var answer = await _httpClient.GetAsync($"{id}");

            if (answer.IsSuccessStatusCode)
            {
                var positionResponce = await answer.Content.ReadAsStringAsync();
                var position = JsonConvert.DeserializeObject<PositionResponce>(positionResponce);

                return position;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound             => new NotFoundException(id),
                HttpStatusCode.InternalServerError  => new InternalServerException(nameof(PositionsRepository))
            };
        }

        public async Task<IEnumerable<PositionResponce>> GetAsync()
        {
            var answer = await _httpClient.GetAsync(string.Empty);

            if (answer.IsSuccessStatusCode)
            {
                var positionsResponce = await answer.Content.ReadAsStringAsync();
                var positions = JsonConvert.DeserializeObject<IEnumerable<PositionResponce>>(positionsResponce);

                return positions;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(PositionsRepository))
            };
        }

        public async Task<Guid> PostAsync(AddPositionRequest addPositionRequest)
        {
            var answer = await _httpClient.PostAsJsonAsync(string.Empty, addPositionRequest);

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
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(PositionsRepository))
            };
        }

        public async Task PutAsync(Guid id, UpdatePositionRequest updatePositionRequest)
        {
            var answer = await _httpClient.PutAsJsonAsync($"{id}", updatePositionRequest);

            if (answer.IsSuccessStatusCode)
                return;

            throw answer.StatusCode switch
            {
                HttpStatusCode.BadRequest => new InvalidModelStateException(nameof(updatePositionRequest)),
                HttpStatusCode.NotFound => new NotFoundException(id),
                HttpStatusCode.Conflict => new DbUpdateException(),
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(PositionsRepository))
            };
        }
    }
}
