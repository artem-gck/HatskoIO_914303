using Newtonsoft.Json;
using NotificationService.DataAccess.Http.Entity;
using NotificationService.DataAccess.Http.Exceptions;
using NotificationService.DataAccess.Http.Interfaces;
using System.Net;

namespace NotificationService.DataAccess.Http.Realisations
{
    public class TaskAccess : ITaskAccess
    {
        private readonly HttpClient _httpClient;

        public TaskAccess(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<TaskResponce>> GetTasksAsync()
        {
            var answer = await _httpClient.GetAsync("{id}");

            if (answer.IsSuccessStatusCode)
            {
                var tasksString = await answer.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<IEnumerable<TaskResponce>>(tasksString);

                return tasks;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.InternalServerError => new InternalServerException()
            };
        }
    }
}
