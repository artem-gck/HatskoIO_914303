using CompanyManagementService.DataAccess.Exceptions;
using CompanyManagementService.DataAccess.Interfaces;
using CompanyManagementService.DataAccess.StructureEntities.AddRequest;
using CompanyManagementService.DataAccess.StructureEntities.Responce;
using CompanyManagementService.DataAccess.StructureEntities.UpdateRequest;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net;
using System.Net.Http.Headers;

namespace CompanyManagementService.DataAccess.Realisation
{
    public class PositionsAccess : IPositionsAccess
    {
        private HttpClient _httpClient;

        public PositionsAccess(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(PositionsAccess));
        }

        public async Task DeleteAsync(Guid id)
        {
            var answer = await _httpClient.DeleteAsync($"{id}");

            if (answer.IsSuccessStatusCode)
                return;

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound             => new NotFoundException(id),
                HttpStatusCode.InternalServerError  => new InternalServerException(nameof(PositionsAccess))
            };
        }

        public async Task<PositionResponce> GetAsync(Guid id)
            => await GetAsync(id, null);

        public async Task<PositionResponce> GetAsync(Guid id, string token)
        {
            if (token is not null && !_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            }

            var answer = await _httpClient.GetAsync($"{id}");

            if (answer.IsSuccessStatusCode)
            {
                var positionResponce = await answer.Content.ReadAsStringAsync();
                var position = JsonConvert.DeserializeObject<PositionResponce>(positionResponce);

                return position;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundException(id),
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(PositionsAccess))
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
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(PositionsAccess))
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
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(PositionsAccess))
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
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(PositionsAccess))
            };
        }
    }
}
