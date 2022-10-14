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
    public class DepartmentRepository : IDepartmentRepository
    {
        private const string ClientName = "departments";
        private const string ServiceName = "Department";

        private IHttpClientFactory _httpClientFactory;

        public DepartmentRepository(IHttpClientFactory httpClientFactory)
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

        public async Task<DepartmentResponce> Get(Guid id)
        {
            using var client = _httpClientFactory.CreateClient(ClientName);
            var answer = await client.GetAsync($"{id}");

            if (answer.IsSuccessStatusCode)
            {
                var departmentResponce = await answer.Content.ReadAsStringAsync();
                var department = JsonConvert.DeserializeObject<DepartmentResponce>(departmentResponce);

                return department;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound             => new NotFoundException(id),
                HttpStatusCode.InternalServerError  => new InternalServerException(ServiceName)
            };
        }

        public async Task<IEnumerable<DepartmentResponce>> Get()
        {
            using var client = _httpClientFactory.CreateClient(ClientName);
            var answer = await client.GetAsync(string.Empty);

            if (answer.IsSuccessStatusCode)
            {
                var departmentsResponce = await answer.Content.ReadAsStringAsync();
                var departments = JsonConvert.DeserializeObject<IEnumerable<DepartmentResponce>>(departmentsResponce);

                return departments;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.InternalServerError  => new InternalServerException(ServiceName)
            };
        }

        public async Task<Guid> Post(AddDepartmentRequest addDepartmentRequest)
        {
            using var client = _httpClientFactory.CreateClient(ClientName);
            var answer = await client.PostAsJsonAsync(string.Empty, addDepartmentRequest);

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
                HttpStatusCode.InternalServerError  => new InternalServerException(ServiceName)
            };
        }

        public async Task Put(Guid id, UpdateDepartmentRequest updateDepartmentRequest)
        {
            using var client = _httpClientFactory.CreateClient(ClientName);
            var answer = await client.PutAsJsonAsync($"{id}", updateDepartmentRequest);

            if (answer.IsSuccessStatusCode)
                return;

            throw answer.StatusCode switch
            {
                HttpStatusCode.BadRequest           => new InvalidModelStateException(nameof(updateDepartmentRequest)),
                HttpStatusCode.NotFound             => new NotFoundException(id),
                HttpStatusCode.Conflict             => new DbUpdateException(),
                HttpStatusCode.InternalServerError  => new InternalServerException(ServiceName)
            };
        }
    }
}
