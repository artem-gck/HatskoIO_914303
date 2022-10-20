using Newtonsoft.Json;
using NotificationService.DataAccess.Http.Entity;
using NotificationService.DataAccess.Http.Exceptions;
using NotificationService.DataAccess.Http.Interfaces;
using System.Net;

namespace NotificationService.DataAccess.Http.Realisations
{
    public class ManagementAccess : IManagementAccess
    {
        private readonly HttpClient _httpClient;

        public ManagementAccess(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));   
        }

        public async Task<UserResponce> GetUserInfoAsync(Guid id)
        {
            var answer = await _httpClient.GetAsync($"{id}");

            if (answer.IsSuccessStatusCode)
            {
                var userString = await answer.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserResponce>(userString);

                return user;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundException(id),
                HttpStatusCode.InternalServerError => new InternalServerException()
            };
        }
    }
}
