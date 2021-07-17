using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace TrueLayer.Service
{
    public class HttpService: IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> Get<T>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseAsString);
        }
    }

    public interface IHttpService
    {
        Task<T> Get<T>(string url);
    }
}
