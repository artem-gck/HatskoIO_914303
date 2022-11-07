using Microsoft.AspNetCore.Http;
using Moq;
using RichardSzalay.MockHttp;
using SignatureService.DataAccess.Http.Exceptions;
using SignatureService.DataAccess.Http.Realisation;
using SignatureService.DataAccess.Http.Responce;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;

namespace SignatureService.Test.DataAccess.Document
{
    public class GetHashAsync
    {
        private Guid _documentId;
        private int _version;
        private byte[] _hash;
        private string _hashString;

        [SetUp]
        public void Setup()
        {
            _documentId = Guid.NewGuid();
            _version = 0;

            using var rsa = RSA.Create();

            var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            rsaFormatter.SetHashAlgorithm(nameof(SHA256));
            _hash = rsaFormatter.CreateSignature(SHA256.HashData(new byte[] { 1, 1, 1 }));

            _hashString = JsonSerializer.Serialize(new HashResponce() { Hash = _hash });
        }

        [Test]
        public async Task GetHashAsync_DocumentIdAndVersion_200StatusCode()
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When($"http://localost/api/documents/{_documentId}/{_version}/hash")
                    .Respond("application/json", _hashString);

            var client = new HttpClient(mockHttp)
            {
                BaseAddress = new Uri("http://localost/api/documents/")
            };

            var access = new DocumentAccess(client);

            var result = await access.GetHashAsync(_documentId, _version);

            Assert.IsNotNull(result);
            Assert.AreEqual(_hash, result.Hash);
        }

        [Test]
        public async Task GetHashAsync_DocumentIdAndVersion_404StatusCode()
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When($"http://localost/api/documents/{_documentId}/{_version}/hash")
                    .Respond(HttpStatusCode.NotFound);

            var client = new HttpClient(mockHttp)
            {
                BaseAddress = new Uri("http://localost/api/documents/")
            };

            var access = new DocumentAccess(client);

            Assert.ThrowsAsync<DocumentNotFoundException>(() => access.GetHashAsync(_documentId, _version));
        }

        [TestCase(HttpStatusCode.Forbidden)]
        [TestCase(HttpStatusCode.BadGateway)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.PermanentRedirect)]
        [TestCase(HttpStatusCode.Conflict)]
        public async Task GetHashAsync_DocumentIdAndVersion_OtherStatusCode(HttpStatusCode statusCode)
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When($"http://localost/api/documents/{_documentId}/{_version}/hash")
                    .Respond(statusCode);

            var client = new HttpClient(mockHttp)
            {
                BaseAddress = new Uri("http://localost/api/documents/")
            };

            var access = new DocumentAccess(client);

            Assert.ThrowsAsync<Exception>(() => access.GetHashAsync(_documentId, _version));
        }
    }
}
