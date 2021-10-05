using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Movies.Client.Services
{
    public class CRUDService : IIntegrationService
    {
        private static HttpClient httpClient = new HttpClient();

        public CRUDService()
        {
            // set up HttpClient instance
            httpClient.BaseAddress = new Uri("http://localhost:57863");
            httpClient.Timeout = new TimeSpan(0, 0, 30);
        }
        
        public async Task Run()
        {
            await GetResource();
        }

        public async Task GetResource()
        {
            var response = await httpClient.GetAsync("api/movies");
            response.EnsureSuccessStatusCode();
        }
    }
}