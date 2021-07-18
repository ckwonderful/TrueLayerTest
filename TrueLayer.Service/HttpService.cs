using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TrueLayer.Service
{
    public class HttpService<TResponse> : IHttpService<TResponse>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TResponse> Get(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await DeserializeObject(response);
        }

        private async Task<TResponse> DeserializeObject(HttpResponseMessage response)
        {
            var responseAsString = await response.Content.ReadAsStringAsync();

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return JsonConvert.DeserializeObject<TResponse>(responseAsString, jsonSerializerSettings);
        }

        public async Task<TResponse> Post<TInput>(string url, TInput parameters)
        {
            var client = _httpClientFactory.CreateClient();

            var objAsJson = JsonConvert.SerializeObject(parameters);
            var content = new StringContent(objAsJson, Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            return await DeserializeObject(response);
        }
    }

    public interface IHttpService<TResponse>
    {
        Task<TResponse> Get(string url);
        Task<TResponse> Post<TInput>(string url, TInput translateRequest);
    }
}
