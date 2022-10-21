using Newtonsoft.Json;
using SignatureService.DataAccess.Http.Exceptions;
using SignatureService.DataAccess.Http.Interfaces;
using SignatureService.DataAccess.Http.Responce;
using System.Net;

namespace SignatureService.DataAccess.Http.Realisation
{
    public class DocumentAccess : IDocumentAccess
    {
        private readonly HttpClient _httpClient;

        public DocumentAccess(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<HashResponce> GetHashAsync(Guid documentId, int version)
        {
            var answer = await _httpClient.GetAsync($"{documentId}/{version}/hash");

            if (answer.IsSuccessStatusCode)
            {
                var hashString = await answer.Content.ReadAsStringAsync();
                var hash = JsonConvert.DeserializeObject<HashResponce>(hashString);

                return hash;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound => new DocumentNotFoundException(documentId),
                _                       => new Exception()
            };
        }
    }
}
