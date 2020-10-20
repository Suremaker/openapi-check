using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using NUnit.Framework;
using OpenApiCheck;

namespace OpenApi.Check.Tests
{
    [TestFixture]
    public class ComparisonChecksTests
    {
        private OpenApiDocument _doc1;
        private OpenApiDocument _doc2;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            _doc1 = await GetSwagger<WebApiV1.Startup>("v1.json");
            _doc2 = await GetSwagger<WebApiV2.Startup>("v2.json");
        }

        private static async Task<OpenApiDocument> GetSwagger<T>(string path) where T : class
        {
            using var host = new WebApplicationFactory<T>();
            using var response = await host.CreateDefaultClient().GetAsync("swagger/v1/swagger.json");
            var swagger = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            await File.WriteAllTextAsync(path, swagger);
            return new OpenApiStreamReader().Read(new MemoryStream(Encoding.UTF8.GetBytes(swagger)), out _);
        }

        [Test]
        public void Operation_compatibility_tests()
        {
            OpenApiComparer.Compare(_doc1, _doc2)
                .FindOperations("/api/Operations")
                .ShouldAllBeOk();

            OpenApiComparer.Compare(_doc2, _doc1)
                .FindOperations("/api/Operations")
                .ShouldHaveErrors(
                    "Put /api/Operations: [Error] Operation no longer exists",
                    "Get /api/Operations/{0}/summary: [Error] Operation no longer exists",
                    "[deprecated] Get /api/Operations/some: [Warning] Operation no longer exists"
                );
        }

        [Test]
        public void Responses_compatibility_tests()
        {
            OpenApiComparer.Compare(_doc1, _doc2)
                .FindOperations("/api/Responses")
                .ShouldAllBeOk();

            OpenApiComparer.Compare(_doc2, _doc1)
                .FindOperations("/api/Responses")
                .ShouldHaveErrors(
                    "Get /api/Responses: [Warning] Operation no longer returns HTTP 400 code"
                );
        }

        [Test]
        public void ResponseContent_compatibility_tests()
        {
            OpenApiComparer.Compare(_doc1, _doc2)
                .FindOperations("/api/ResponseContent")
                .ShouldAllBeOk();

            OpenApiComparer.Compare(_doc2, _doc1)
                .FindOperations("/api/ResponseContent")
                .ShouldHaveErrors(
                    "Get /api/ResponseContent: [Error] Operation no longer returns application/xml for 200 code"
                );
        }

        [Test]
        public void ResponseBreakingModel_compatibility_tests()
        {
            OpenApiComparer.Compare(_doc1, _doc2)
                .FindOperations("/api/ResponseBreakingModel")
                .ShouldHaveErrors(
                    "Get /api/ResponseBreakingModel: [Error] response(HTTP 200|application/json).body type does not match (before: string, after: array)",
                    "Get /api/ResponseBreakingModel/number: [Error] response(HTTP 200|application/json).body type does not match (before: integer, after: number)",
                    "Get /api/ResponseBreakingModel/details: [Warning] response(HTTP 200|application/json).body.obsolete no longer exists (deprecated)",
                    "Get /api/ResponseBreakingModel/array: [Error] response(HTTP 200|application/json).body[] type does not match (before: integer, after: string)");
        }

        [Test]
        public void ResponseModel_compatibility_tests()
        {
            OpenApiComparer.Compare(_doc1, _doc2)
                .FindOperations("/api/ResponseModel")
                .ShouldAllBeOk();

            OpenApiComparer.Compare(_doc2, _doc1)
                .FindOperations("/api/ResponseModel")
                .ShouldHaveErrors(
                    "Get /api/ResponseModel: [Error] response(HTTP 200|application/json).body[].id is now nullable",
                    "Get /api/ResponseModel: [Error] response(HTTP 200|application/json).body[].text is now nullable",
                    "Get /api/ResponseModel: [Error] response(HTTP 200|application/json).body[].newField no longer exists"
                );
        }

        [Test]
        public void RequestModel_compatibility_tests()
        {
            OpenApiComparer.Compare(_doc1, _doc2)
                .FindOperations("/api/RequestModel")
                .ShouldAllBeOk();

            OpenApiComparer.Compare(_doc2, _doc1)
                .FindOperations("/api/RequestModel")
                .ShouldHaveErrors(
                    "Post /api/RequestModel: [Error] Operation request body is now required",
                    "Post /api/RequestModel: [Error] request(application/json).body.requestId is no longer nullable",
                    "Post /api/RequestModel: [Error] request(application/json).body.text is now required",
                    "Post /api/RequestModel: [Error] request(application/json).body.number is no longer nullable",
                    "Post /api/RequestModel: [Error] request(application/json).body.additionalParameter no longer exists",
                    "Post /api/RequestModel: [Error] Operation no longer accepts request for application/xml"
                );
        }

        [Test]
        public void RequestBreakingModel_compatibility_tests()
        {
            OpenApiComparer.Compare(_doc1, _doc2)
                .FindOperations("/api/RequestBreakingModel")
                .ShouldHaveErrors(
                    "Post /api/RequestBreakingModel: [Error] request(application/json).body.someField type does not match (before: string, after: integer)",
                    "Post /api/RequestBreakingModel: [Error] request(application/json).body.someArray[] type does not match (before: , after: integer)",
                    "Post /api/RequestBreakingModel: [Warning] request(application/json).body.obsoleteField no longer exists (deprecated)"
                );
        }

        [Test]
        public void RecursiveModels_compatibility_tests()
        {
            OpenApiComparer.Compare(_doc1, _doc2)
                .FindOperations("/api/RecursiveModel")
                .ShouldAllBeOk();

            OpenApiComparer.Compare(_doc2, _doc1)
                .FindOperations("/api/RecursiveModel")
                .ShouldHaveErrors(
                    "Post /api/RecursiveModel: [Error] response(HTTP 200|application/json).body.responseId no longer exists",
                    "Post /api/RecursiveModel: [Error] response(HTTP 200|application/json).body.otherResponses{}.key no longer exists",
                    "Post /api/RecursiveModel: [Error] response(HTTP 200|application/json).body.otherItems[].key no longer exists",
                    "Post /api/RecursiveModel: [Error] request(application/json).body.requestId no longer exists",
                    "Post /api/RecursiveModel: [Error] request(application/json).body.otherRequests{}.key no longer exists",
                    "Post /api/RecursiveModel: [Error] request(application/json).body.otherItems[].key no longer exists"
                );
        }
    }
}
