using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Movies.API.Models;
using Newtonsoft.Json;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;
using Movie = Movies.API.Entities.Movie;

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
            httpClient.DefaultRequestHeaders.Clear();
            // httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
        }

        public async Task Run()
        {
            // await GetResource();
            // await GetResourceThroughHttpRequestMessage();
            // await CreateResource();
            // await UpdateResource();
            // await DeleteResource();
            // await PostResourceShortcut();
            // await PutResourceShortcut();
            // await DeleteResourceShortcut();
        }

        private async Task GetResource()
        {
            var response = await httpClient.GetAsync("api/movies");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var movies = new List<Movie>();
            if (response.Content.Headers.ContentType.MediaType == "application/json")
            {
                movies = JsonConvert.DeserializeObject<List<Movie>>(content);
            }
            else if (response.Content.Headers.ContentType.MediaType == "application/xml")
            {
                var serializer = new XmlSerializer(typeof(List<Movie>));
                movies = (List<Movie>) serializer.Deserialize(new StringReader(content));
            }
        }

        private async Task GetResourceThroughHttpRequestMessage()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/movies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<List<Movie>>(content);
        }

        private async Task CreateResource()
        {
            var movieToCreate = new MovieForCreation()
            {
                Title = "title",
                Description = "description",
                DirectorId = Guid.Parse(""),
                ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
                Genre = "Crime, Drama"
            };

            var serializedToCreate = JsonConvert.SerializeObject(movieToCreate);

            var request = new HttpRequestMessage(HttpMethod.Post, "api/movies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(serializedToCreate);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createdMovie = JsonConvert.DeserializeObject<Movie>(content);
        }

        private async Task UpdateResource()
        {
            var movieToUpdate = new MovieForUpdate()
            {
                Title = "title 2",
                Description = "description 2",
                DirectorId = Guid.Parse(""),
                ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 3)),
                Genre = "Crime2, Drama2"
            };

            var serializedMovieToUpdate = JsonConvert.SerializeObject(movieToUpdate);

            var request = new HttpRequestMessage(HttpMethod.Put, $"api/movies/guid");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(serializedMovieToUpdate);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var updateMovie = JsonConvert.DeserializeObject<Movie>(content);
        }

        private async Task DeleteResource()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/movies/guid");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
        }
        
        private async Task PostResourceShortcut()
        {
            var movieToCreate = new MovieForCreation()
            {
                Title = "Reservoir Dogs",
                Description = "After a simple jewelry heist goes terribly wrong, the " +
                "surviving criminals begin to suspect that one of them is a police informant.",
                DirectorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
                Genre = "Crime, Drama"
            };

            var response = await httpClient.PostAsync(
                "api/movies",
                new StringContent(
                    JsonConvert.SerializeObject(movieToCreate),
                    Encoding.UTF8,
                    "application/json"));

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createdMovie = JsonConvert.DeserializeObject<Movie>(content);
        }

        private async Task PutResourceShortcut()
        {
            var movieToUpdate = new MovieForUpdate()
            {
                Title = "Pulp Fiction",
                Description = "The movie with Zed.",
                DirectorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
                Genre = "Crime, Drama"
            };

            var response = await httpClient.PutAsync(
               "api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b",
               new StringContent(
                   JsonConvert.SerializeObject(movieToUpdate),
                   System.Text.Encoding.UTF8,
                   "application/json"));

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var updatedMovie = JsonConvert.DeserializeObject<Movie>(content);
        }

        private async Task DeleteResourceShortcut()
        {
            var response = await httpClient.DeleteAsync(
                "api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
        }
    }
}