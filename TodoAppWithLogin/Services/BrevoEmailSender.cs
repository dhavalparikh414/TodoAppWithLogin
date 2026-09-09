using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace TodoAppWithLogin.Services
{
    public class BrevoEmailSender : IEmailSender
    {
        private readonly string _apiKey;
        private readonly IHttpClientFactory _httpClientFactory;

        public BrevoEmailSender(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _apiKey = config["Brevo:ApiKey"]!;
            _httpClientFactory = httpClientFactory;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://api.brevo.com/");
            client.DefaultRequestHeaders.Add("api-key", _apiKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var payload = new
            {
                sender = new { name = "TodoApp", email = "noreply@todoapp.com.au" },
                to = new[] { new { email = toEmail } },
                subject = subject,
                htmlContent = htmlMessage
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("v3/smtp/email", content);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new Exception($"Brevo email failed: {response.StatusCode} - {body}");
            }
        }
    }
}