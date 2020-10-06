using System;
using System.IO;
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
            var swagger1 = await ReadSwagger(args[0]);
            var swagger2 = await ReadSwagger(args[1]);

            return 0;
        }

        private static async Task<OpenApiDocument> ReadSwagger(string path)
        {
            if (new Uri(path).IsFile)
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
