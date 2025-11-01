using Serilog.Sinks.Grafana.Loki;
using System.Net;

namespace ServiceA
{
    // Custom HTTP client for Loki
    public class LokiHttpClient : Serilog.Sinks.Grafana.Loki.ILokiHttpClient
    {
        private readonly HttpClient _httpClient;
        private LokiCredentials? _credentials;
        private string? _tenant;

        public LokiHttpClient()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, Stream contentStream)
        {
            Console.WriteLine($"[DEBUG] Posting to Loki: {requestUri}");

            var content = new StreamContent(contentStream);
            content.Headers.Add("Content-Type", "application/json");

            // Add tenant header if set
            if (!string.IsNullOrEmpty(_tenant))
            {
                content.Headers.Add("X-Scope-OrgID", _tenant);
            }

            var response = await _httpClient.PostAsync(requestUri, content);
            Console.WriteLine($"[DEBUG] Loki response: {response.StatusCode}");

            return response;
        }

        public void SetCredentials(LokiCredentials? credentials)
        {
            _credentials = credentials;
            Console.WriteLine($"[DEBUG] SetCredentials called");
        }

        public void SetTenant(string? tenant)
        {
            _tenant = tenant;
            Console.WriteLine($"[DEBUG] SetTenant called: {tenant}");
        }

        public void Dispose() => _httpClient?.Dispose();
    }
}
