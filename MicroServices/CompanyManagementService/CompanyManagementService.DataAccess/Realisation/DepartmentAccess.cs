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
    public class DepartmentAccess : IDepartmentAccess
    {
        private HttpClient _httpClient;

        public DepartmentAccess(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task DeleteAsync(Guid id)
        {
            var answer = await _httpClient.DeleteAsync($"{id}");

            if (answer.IsSuccessStatusCode)
                return;

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound             => new NotFoundException(id),
                HttpStatusCode.InternalServerError  => new InternalServerException(nameof(DepartmentAccess))
            };
        }

        public async Task<DepartmentResponce> GetAsync(Guid id)
        {
            var answer = await _httpClient.GetAsync($"{id}");

            if (answer.IsSuccessStatusCode)
            {
                var departmentResponce = await answer.Content.ReadAsStringAsync();
                var department = JsonConvert.DeserializeObject<DepartmentResponce>(departmentResponce);

                return department;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound             => new NotFoundException(id),
                HttpStatusCode.InternalServerError  => new InternalServerException(nameof(DepartmentAccess))
            };
        }

        public async Task<IEnumerable<DepartmentResponce>> GetAsync()
        {
            var answer = await _httpClient.GetAsync(string.Empty);

            if (answer.IsSuccessStatusCode)
            {
                var departmentsResponce = await answer.Content.ReadAsStringAsync();
                var departments = JsonConvert.DeserializeObject<IEnumerable<DepartmentResponce>>(departmentsResponce);

                return departments;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.InternalServerError  => new InternalServerException(nameof(DepartmentAccess))
            };
        }

        public async Task<Guid> PostAsync(AddDepartmentRequest addDepartmentRequest)
        {
            var answer = await _httpClient.PostAsJsonAsync(string.Empty, addDepartmentRequest);

            if (answer.IsSuccessStatusCode)
            {
                var idResponce = await answer.Content.ReadAsStringAsync();
                var id = Guid.Parse(idResponce);

                return id;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.BadRequest           => new InvalidModelStateException(nameof(addDepartmentRequest)),
                HttpStatusCode.Conflict             => new DbUpdateException(),
                HttpStatusCode.InternalServerError  => new InternalServerException(nameof(DepartmentAccess))
            };
        }

        public async Task PutAsync(Guid id, UpdateDepartmentRequest updateDepartmentRequest)
        {
            var answer = await _httpClient.PutAsJsonAsync($"{id}", updateDepartmentRequest);

            if (answer.IsSuccessStatusCode)
                return;

            throw answer.StatusCode switch
            {
                HttpStatusCode.BadRequest           => new InvalidModelStateException(nameof(updateDepartmentRequest)),
                HttpStatusCode.NotFound             => new NotFoundException(id),
                HttpStatusCode.Conflict             => new DbUpdateException(),
                HttpStatusCode.InternalServerError  => new InternalServerException(nameof(DepartmentAccess))
            };
        }
    }
}
