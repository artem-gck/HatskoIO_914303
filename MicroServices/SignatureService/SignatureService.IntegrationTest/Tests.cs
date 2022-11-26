using Dapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SignatureService.DataAccess.DataBase;
using SignatureService.DataAccess.Http.Interfaces;
using SignatureService.DataAccess.Http.Realisation;
using SignatureService.IntegrationTest.Helpers;
using SignatureServiceApi.ViewModels;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace SignatureService.IntegrationTest
{
    public class Tests
    {
        private const string Token = "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IkJEQTFFRURDNjNDQkVDNDY4N0Q5MzdDNThCM0ZBQjYxIiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2Njc5ODEzNzcsImV4cCI6MTY2Nzk4NDk3NywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzMTAiLCJhdWQiOlsiZG9jdW1lbnRfYXBpIiwic2lnbmF0dXJlX2FwaSJdLCJjbGllbnRfaWQiOiJzaWduYXR1cmVfYXBpIiwic3ViIjoiMmE0YTUwYzktYzQ1Mi00ZmZiLWI1MjItOGZiNmQ4MjdlZjUxIiwiYXV0aF90aW1lIjoxNjY3NzM3NTIwLCJpZHAiOiJsb2NhbCIsInJvbGUiOlsiYWRtaW4iLCJTa29ydWJhSWRlbnRpdHlBZG1pbkFkbWluaXN0cmF0b3IiXSwibmFtZSI6ImFydGVtX2djayIsImp0aSI6IkI1OTdBNkM2MkZENzE0MDU3RjQzQTcwRDkzMjcwMEE3Iiwic2lkIjoiMDMzRDYyODk4QjJGRkQzMENEQURBMzcyOTg3RDAwQUUiLCJpYXQiOjE2Njc5ODEzNzcsInNjb3BlIjpbImRvY3VtZW50X2FwaSIsInNpZ25hdHVyZV9hcGkiXSwiYW1yIjpbInB3ZCJdfQ.AqjK3AWhTKoOCoKeakTQpzFlSm4JZAuBnWKIAwbx43UaZRvYxpF-vjMaHkWN86FYk2LuTz4zo1dFdn6n_1e4UZe0vQFBQBGC67QDX6cfzCe2AzooaVYNPk6UIUSXeU6PteCXqpu51qVKf3t5u2yP2JDUAIzOTKts5ce0Y5adqke_8XTabVDeSaKPXME3b7mN6ix_VXent6VtOBiPoVZLvSSn-nc_BACJQngLp3lsKAHO9nhW1x5OYeWjfw3bwUxfgngQil5WhwvXkGjqYmGlmUSJdDDCfdqPQDyz4PibtBDCvuXXl0KmcrnndSnIRQu8NW6KT6IQQ2hgxgmVlVitYw";
        private const string SqlConnectionString = "Filename=Signatures.db";
        private const string DocumentAcceessConnectionString = "https://localhost:7129/api/documents/";
        private HttpClient _httpClient;

        public Tests()
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            var factory = new WebApplicationFactory<Program>();

            _httpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var sqlServerDescriptor = services.SingleOrDefault(d => d.ServiceType.Name == typeof(IConnectionProvider).Name);
                    var httpClientDiscriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IHttpClientFactory));
                    var connectionStringDiscriptor = services.SingleOrDefault(d => d.ServiceType == typeof(string));

                    if (sqlServerDescriptor != null)
                        services.Remove(sqlServerDescriptor);

                    if (httpClientDiscriptor != null)
                        services.Remove(httpClientDiscriptor);

                    if (connectionStringDiscriptor != null)
                        services.Remove(connectionStringDiscriptor);

                    services.AddSingleton(SqlConnectionString);
                    services.AddSingleton<IConnectionProvider, SqliteConnectionProvider>();
                    services.AddHttpClient<IDocumentAccess, DocumentAccess>(HttpClient => HttpClient.BaseAddress = new Uri(DocumentAcceessConnectionString)).ConfigurePrimaryHttpMessageHandler(() => clientHandler);

                    SqlMapper.AddTypeHandler(new GuidHandler());
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"{Token}");
            _httpClient.BaseAddress = new Uri("https://localhost/api/");
        }

        [TestCase(-2)]
        [TestCase(-50)]
        [TestCase(-100)]
        [TestCase(int.MinValue)]
        public async Task PostAddSignature_VersionLessThenMinusOne_EndpointsReturnBadRequest(int version)
        {
            // Arrange
            var url = $"signatures/{Guid.NewGuid()}/{Guid.NewGuid()}/{version}";

            // Act
            var response = await _httpClient.PostAsync(url, null);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test, Order(1)]
        public async Task PostAddSignature_UserIdAndDocument_EndpointsReturnCreated()
        {
            var url = $"signatures/0be409a4-d101-4242-84bc-1a6695d37e89/7c8f4490-25b6-43b8-8de2-db1b3c01ebbe/2";

            var response = await _httpClient.PostAsync(url, null);
            
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
        }

        [Test]
        public async Task PostAddSignature_UserIdAndRandomDocument_EndpointsReturnNotFoundDocument()
        {
            var url = $"signatures/0be409a4-d101-4242-84bc-1a6695d37e89/{Guid.NewGuid()}/2";

            var response = await _httpClient.PostAsync(url, null);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task PostAddSignature_UserIdAndRandomDocument_EndpointsReturnNotFoundUser()
        {
            var url = $"signatures/{Guid.NewGuid()}/7c8f4490-25b6-43b8-8de2-db1b3c01ebbe/2";

            var response = await _httpClient.PostAsync(url, null);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [TestCase(-2)]
        [TestCase(-50)]
        [TestCase(-100)]
        [TestCase(int.MinValue)]
        public async Task GetSignatures_VersionLessMinusOne_EndpointsReturnBadRequest(int version)
        {
            var url = $"documents/{Guid.NewGuid()}/{version}";

            var response = await _httpClient.GetAsync(url);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test, Order(5)]
        public async Task GetSignatures_Document_EndpointsReturnOk()
        {
            var url = $"documents/7c8f4490-25b6-43b8-8de2-db1b3c01ebbe/2";

            var response = await _httpClient.GetAsync(url);

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test, Order(6)]
        public async Task GetSignatures_RandomDocument_EndpointsReturnNotFound()
        {
            var url = $"documents/{Guid.NewGuid()}/0";

            var response = await _httpClient.GetAsync(url);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [TestCase(-2)]
        [TestCase(-50)]
        [TestCase(-100)]
        [TestCase(int.MinValue)]
        public async Task PostCheckSignatures_VersionLessMinusOne_EndpointsReturnBadRequest(int version)
        {
            var url = $"signatures/7c8f4490-25b6-43b8-8de2-db1b3c01ebbe/{version}";

            var checkPublicKeyRequest = new CheckPublicKeyRequest();
            checkPublicKeyRequest.Key = JsonSerializer.Deserialize<byte[]>("\"MIIBCgKCAQEAqhRT+l8zaqH2Jw5M/mhUt007XvAeAILCE9fx9xOvZxTAuHoWrepqdF6bNNJUWKcAya4f672TKaonxzusF19VLpWZgz2ELC074bMXb9FaIANz3TQ/gqoXJ246Zdbcz/TRhcrEvgjl+Lrx65rBRwZGGT6SslPpr8jDRjKkRIhs0OkDbmSALo1K9iZhJTn0kn8SkIk6tKTval917zTRAZyi98+mp02/lV57d0P7AIraFSOcbHZwLe0TQjMYRrojQx3LI9p6RUOxWkN35u+Zc9graA9iLinISFZgWgHfc6OZfXa6mxcbcU+IMsdIB9VhZ6zu+wiy6H2aIRg0gne+zOhJ1QIDAQAB\"");

            var response = await _httpClient.PostAsJsonAsync(url, checkPublicKeyRequest);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task PostCheckSignatures_InvalidModelState_EndpointsReturnBadRequest()
        {
            var url = $"signatures/7c8f4490-25b6-43b8-8de2-db1b3c01ebbe/2";

            var checkPublicKeyRequest = new CheckPublicKeyRequest();

            var response = await _httpClient.PostAsJsonAsync(url, checkPublicKeyRequest);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test, Order(2)]
        public async Task PostCheckSignatures_IncorrectKey_EndpointsReturnNotFound()
        {
            var url = $"signatures/7c8f4490-25b6-43b8-8de2-db1b3c01ebbe/2";

            var checkPublicKeyRequest = new CheckPublicKeyRequest();
            checkPublicKeyRequest.Key = JsonSerializer.Deserialize<byte[]>("\"MIIBCgKCAQEAqhRT+l8zaqH2Jw5M/mhUt007XvAeAILCE9fx9xOvZxTAuHoWrepqdF6bNNJUWKcAya4f672TKaonxzusF19VLpWZgz2ELC074bMXb9FaIANz3TQ/gqoXJ246Zdbcz/TRhcrEvgjl+Lrx65rBRwZGGT6SslPpr8jDRjKkRIhs0OkDbmSALo1K9iZhJTn0kn8SkIk6tKTval917zTRAZyi98+mp02/lV57d0P7AIraFSOcbHZwLe0TQjMYRrojQx3LI9p6RUOxWkN35u+Zc9graA9iLinISFZgWgHfc6OZfXa6mxcbcU+IMsdIB9VhZ6zu+wiy6H2aIRg0gne+zOhJ1QIDAQAA\"");

            var response = await _httpClient.PostAsJsonAsync(url, checkPublicKeyRequest);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [Test, Order(3)]
        public async Task PostCheckSignatures_CorrectKey_EndpointsReturnOk()
        {
            var url = $"signatures/7c8f4490-25b6-43b8-8de2-db1b3c01ebbe/2";

            var checkPublicKeyRequest = new CheckPublicKeyRequest();
            checkPublicKeyRequest.Key = JsonSerializer.Deserialize<byte[]>("\"MIIBCgKCAQEAqhRT+l8zaqH2Jw5M/mhUt007XvAeAILCE9fx9xOvZxTAuHoWrepqdF6bNNJUWKcAya4f672TKaonxzusF19VLpWZgz2ELC074bMXb9FaIANz3TQ/gqoXJ246Zdbcz/TRhcrEvgjl+Lrx65rBRwZGGT6SslPpr8jDRjKkRIhs0OkDbmSALo1K9iZhJTn0kn8SkIk6tKTval917zTRAZyi98+mp02/lV57d0P7AIraFSOcbHZwLe0TQjMYRrojQx3LI9p6RUOxWkN35u+Zc9graA9iLinISFZgWgHfc6OZfXa6mxcbcU+IMsdIB9VhZ6zu+wiy6H2aIRg0gne+zOhJ1QIDAQAB\"");

            var response = await _httpClient.PostAsJsonAsync(url, checkPublicKeyRequest);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test, Order(4)]
        public async Task PostCheckSignatures_CorrectKey_EndpointsReturnNotFound()
        {
            var url = $"signatures/{Guid.NewGuid()}/2";

            var checkPublicKeyRequest = new CheckPublicKeyRequest();
            checkPublicKeyRequest.Key = JsonSerializer.Deserialize<byte[]>("\"MIIBCgKCAQEAqhRT+l8zaqH2Jw5M/mhUt007XvAeAILCE9fx9xOvZxTAuHoWrepqdF6bNNJUWKcAya4f672TKaonxzusF19VLpWZgz2ELC074bMXb9FaIANz3TQ/gqoXJ246Zdbcz/TRhcrEvgjl+Lrx65rBRwZGGT6SslPpr8jDRjKkRIhs0OkDbmSALo1K9iZhJTn0kn8SkIk6tKTval917zTRAZyi98+mp02/lV57d0P7AIraFSOcbHZwLe0TQjMYRrojQx3LI9p6RUOxWkN35u+Zc9graA9iLinISFZgWgHfc6OZfXa6mxcbcU+IMsdIB9VhZ6zu+wiy6H2aIRg0gne+zOhJ1QIDAQAB\"");

            var response = await _httpClient.PostAsJsonAsync(url, checkPublicKeyRequest);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }
    }
}
