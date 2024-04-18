using System.IO.Compression;
using System.Text;

namespace StackTag
{
    public interface IStackOverflowAPI
    {
        Task<string> GetTagsAsync(string queryString = "");
    }

    public class StackOverflowAPI : IStackOverflowAPI
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseURL = "https://api.stackexchange.com/2.3/";
        private readonly IConfiguration _config;

        public StackOverflowAPI(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _config = config;   
        }

        public async Task<string> GetTagsAsync(string queryString = "")
        {
            var endpoint = "tags";
            var key = _config.GetValue<string>("StackOverflowAPI:Key") ?? throw new Exception();
            var url = $"{_baseURL}{endpoint}?{queryString}&key={key}";

            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync(url);
            ThrottlingDetection(response);
            response.EnsureSuccessStatusCode();
            response.Content.ReadAsStringAsync().Wait();
            var responded = response.Content.ReadAsStreamAsync().Result;
            Stream decompressed = new GZipStream(responded, CompressionMode.Decompress);
            StreamReader objReader = new StreamReader(decompressed, Encoding.UTF8);
            return objReader.ReadToEnd();
        }

        private static void ThrottlingDetection(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                IEnumerable<string> name;
                IEnumerable<string> message;
                if (response.Headers.TryGetValues("x-error-name", out name) && response.Headers.TryGetValues("x-error-message", out message))
                {
                    throw new Exception(name.FirstOrDefault() + ": " + message.FirstOrDefault());
                }
            }
        }
    }
}