using CompanyManagementService.DataAccess.Exceptions;
using CompanyManagementService.DataAccess.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net;
using CompanyManagementService.DataAccess.UserEntity;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace CompanyManagementService.DataAccess.Realisation
{
    public class UserInfoAccess : IUserInfoAccess
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserInfoAccess> _logger;

        public UserInfoAccess(HttpClient httpClient, ILogger<UserInfoAccess> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task DeleteAsync(Guid id)
        {
            var answer = await _httpClient.DeleteAsync($"users/{id}");

            if (answer.IsSuccessStatusCode)
                return;

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundException(id),
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(UserInfoAccess))
            };
        }

        public async Task<UserResponce> GetAsync(Guid id)
            => await GetAsync(id, null);

        public async Task<UserResponce> GetAsync(Guid id, string token)
        {
            if (!string.IsNullOrWhiteSpace(token) && !_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            }

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
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(UserInfoAccess))
            };
        }

        public async Task<IEnumerable<UserResponce>> GetByDepartmentIdAsync(Guid id)
            => await GetByDepartmentIdAsync(id, null);

        public async Task<IEnumerable<UserResponce>> GetByDepartmentIdAsync(Guid id, string token)
        {
            if (!string.IsNullOrWhiteSpace(token) && !_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            }

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
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(UserInfoAccess))
            };
        }

        public async Task<IEnumerable<UserResponce>> GetAsync()
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
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(UserInfoAccess))
            };
        }

        public async Task<Guid> PostAsync(AddUserRequest addUserRequest)
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
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(UserInfoAccess))
            };
        }

        public async Task PutAsync(Guid id, UpdateUserRequest updateUserRequest)
        {
            var answer = await _httpClient.PutAsJsonAsync($"{id}", updateUserRequest);

            if (answer.IsSuccessStatusCode)
                return;

            throw answer.StatusCode switch
            {
                HttpStatusCode.BadRequest => new InvalidModelStateException(nameof(updateUserRequest)),
                HttpStatusCode.NotFound => new NotFoundException(id),
                HttpStatusCode.Conflict => new DbUpdateException(),
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(UserInfoAccess))
            };
        }
    }
}
