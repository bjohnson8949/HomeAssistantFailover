using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace HomeAssistantFailover
{
    public class http_helpers
    {
        public static async Task Put(string urlPath, string payload)
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Put, urlPath);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(payload, Encoding.UTF8);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
        }

        public static async Task<string> Post(string urlPath, string? payload = null)
        {
            string? responseBody = null;

            using (HttpClient _httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, urlPath);

                if (payload != null)
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Content = new StringContent(payload, Encoding.UTF8);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                responseBody = await response.Content.ReadAsStringAsync();
            }

            return responseBody;
        }

        public static async Task<string> Get(string urlPath, string? payload = null)
        {
            string? responseBody = null;

            using (HttpClient _httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, urlPath);

                if (payload != null)
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Content = new StringContent(payload, Encoding.UTF8);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                responseBody = await response.Content.ReadAsStringAsync();
            }

            return responseBody;
        }

    }
}