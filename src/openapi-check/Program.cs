using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace OpenApiCheck
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            try
            {
                if (args.Length != 2)
                {
                    await Console.Error.WriteLineAsync($"Invalid parameter count: {args.Length}");
                    await Console.Error.WriteLineAsync("Usage: openapi-check [old-swagger-uri] [new-swagger-uri]");
                    return -1;
                }

                return await CompareSwaggers(args);
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync($"Application failed with unexpected error:\n{ex}");
                return -1;
            }
        }

        private static async Task<int> CompareSwaggers(string[] args)
        {
            var swagger1 = await ReadSwagger(args[0]);
            var swagger2 = await ReadSwagger(args[1]);

            var result = OpenApiComparer.Compare(swagger1, swagger2);
            foreach (var operation in result.Operations)
            {
                Console.WriteLine($"{operation.FullName}: {operation.Status}");
                foreach (var message in operation.Messages)
                    Console.WriteLine($"  {message.Status}: {message.Message}");
            }

            return (int)result.Status;
        }

        private static async Task<OpenApiDocument> ReadSwagger(string path)
        {
            var uri = new Uri(path, UriKind.RelativeOrAbsolute);
            if (!uri.IsAbsoluteUri || uri.IsFile)
            {
                await using var stream = File.OpenRead(path);
                return new OpenApiStreamReader().Read(stream, out _);
            }
            else
            {
                using var client = new HttpClient();
                using var response = await client.GetAsync(path);
                await using var stream = await response.EnsureSuccessStatusCode().Content.ReadAsStreamAsync();
                return new OpenApiStreamReader().Read(stream, out _);
            }
        }
    }
}
