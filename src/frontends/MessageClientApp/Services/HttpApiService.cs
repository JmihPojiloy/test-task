using shared.Models;

namespace MessageClientApp.Services
{
    public class HttpApiService
    {
        private static int countId = 0;

        private readonly HttpClient _httpClient;

        public HttpApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Create(Message message)
        {
            message.Id = ++countId;
            var response = await _httpClient.PostAsJsonAsync("", message);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Message>> GetByTenMinutes()
        {
            var response = await _httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Message>>();
        }
    }
}