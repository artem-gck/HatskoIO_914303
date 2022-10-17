using CompanyManagementService.DataAccess.Exceptions;
using CompanyManagementService.DataAccess.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net;
using CompanyManagementService.DataAccess.UserEntity;

namespace CompanyManagementService.DataAccess.Realisation
{
    public class UserInfoRepository : IUserInfoRepository
    {
        private const string ServiceName = "UserInfo";

        private readonly HttpClient _httpClient;

        public UserInfoRepository(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task Delete(Guid id)
        {
            var answer = await _httpClient.DeleteAsync($"users/{id}");

            if (answer.IsSuccessStatusCode)
                return;

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundException(id),
                HttpStatusCode.InternalServerError => new InternalServerException(ServiceName)
            };
        }

        public async Task<UserResponce> Get(Guid id)
        {
            var answer = await _httpClient.GetAsync($"users/{id}");

            if (answer.IsSuccessStatusCode)
            {
                var userResponce = await answer.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserResponce>(userResponce);

                return user;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundException(id),
                HttpStatusCode.InternalServerError => new InternalServerException(ServiceName)
            };
        }

        public async Task<IEnumerable<UserResponce>> GetByDepartmentId(Guid id)
        {
            var answer = await _httpClient.GetAsync($"departments/{id}/users");

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

        public async Task<IEnumerable<UserResponce>> Get()
        {
            var answer = await _httpClient.GetAsync(string.Empty);

            if (answer.IsSuccessStatusCode)
            {
                var usersResponce = await answer.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<IEnumerable<UserResponce>>(usersResponce);

                return users;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.InternalServerError => new InternalServerException(ServiceName)
            };
        }

        public async Task<Guid> Post(AddUserRequest addUserRequest)
        {
            var answer = await _httpClient.PostAsJsonAsync(string.Empty, addUserRequest);

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

        public async Task Put(Guid id, UpdateUserRequest updateUserRequest)
        {
            var answer = await _httpClient.PutAsJsonAsync($"{id}", updateUserRequest);

            if (answer.IsSuccessStatusCode)
                return;

            throw answer.StatusCode switch
            {
                HttpStatusCode.BadRequest => new InvalidModelStateException(nameof(updateUserRequest)),
                HttpStatusCode.NotFound => new NotFoundException(id),
                HttpStatusCode.Conflict => new DbUpdateException(),
                HttpStatusCode.InternalServerError => new InternalServerException(ServiceName)
            };
        }
    }
}
