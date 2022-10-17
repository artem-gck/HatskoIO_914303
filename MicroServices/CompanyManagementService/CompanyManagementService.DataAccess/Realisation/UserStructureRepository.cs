using CompanyManagementService.DataAccess.Exceptions;
using CompanyManagementService.DataAccess.Interfaces;
using CompanyManagementService.DataAccess.StructureEntities.AddRequest;
using CompanyManagementService.DataAccess.StructureEntities.Responce;
using CompanyManagementService.DataAccess.StructureEntities.UpdateRequest;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace CompanyManagementService.DataAccess.Realisation
{
    public class UserStructureRepository : IUserStructureRepository
    {
        private const string ServiceName = "UserStructure";

        private readonly HttpClient _httpClient;

        public UserStructureRepository(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task Delete(Guid departmentId, Guid userId)
        {
            var answer = await _httpClient.DeleteAsync($"{departmentId}/users/{userId}");

            if (answer.IsSuccessStatusCode)
                return;

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundException(userId),
                HttpStatusCode.InternalServerError => new InternalServerException(ServiceName)
            };
        }

        public async Task<UserResponce> Get(Guid departmentId, Guid userId)
        {
            var answer = await _httpClient.GetAsync($"{departmentId}/users/{userId}");

            if (answer.IsSuccessStatusCode)
            {
                var userResponce = await answer.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserResponce>(userResponce);

                return user;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundException(userId),
                HttpStatusCode.InternalServerError => new InternalServerException(ServiceName)
            };
        }

        public async Task<IEnumerable<UserResponce>> GetByDepartmentId(Guid id)
        {
            var answer = await _httpClient.GetAsync($"{id}/users");

            if (answer.IsSuccessStatusCode)
            {
                var userResponce = await answer.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<IEnumerable<UserResponce>>(userResponce);

                return users;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundException(id),
                HttpStatusCode.InternalServerError => new InternalServerException(ServiceName)
            };
        }

        public async Task<Guid> Post(Guid departmentId, AddUserRequest addUserRequest)
        {
            var answer = await _httpClient.PostAsJsonAsync($"{departmentId}/users", addUserRequest);

            if (answer.IsSuccessStatusCode)
            {
                var idResponce = await answer.Content.ReadAsStringAsync();
                var id = Guid.Parse(idResponce);

                return id;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.BadRequest => new InvalidModelStateException(nameof(addUserRequest)),
                HttpStatusCode.Conflict => new DbUpdateException(),
                HttpStatusCode.InternalServerError => new InternalServerException(ServiceName)
            };
        }

        public async Task Put(Guid departmentId, Guid userId, UpdateUserRequest updateUserRequest)
        {
            var answer = await _httpClient.PutAsJsonAsync($"{departmentId}/users/{userId}", updateUserRequest);

            if (answer.IsSuccessStatusCode)
                return;

            throw answer.StatusCode switch
            {
                HttpStatusCode.BadRequest => new InvalidModelStateException(nameof(updateUserRequest)),
                HttpStatusCode.NotFound => new NotFoundException(userId),
                HttpStatusCode.Conflict => new DbUpdateException(),
                HttpStatusCode.InternalServerError => new InternalServerException(ServiceName)
            };
        }
    }
}
