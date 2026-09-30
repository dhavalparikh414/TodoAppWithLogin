using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TodoAppReminderTrigger;

public class Function
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<string> FunctionHandler(object input, ILambdaContext context)
    {
        var secretKey = Environment.GetEnvironmentVariable("REMINDER_SECRET_KEY");
        var targetUrl = "https://todoapp.com.au/api/send-reminders";

        var request = new HttpRequestMessage(HttpMethod.Post, targetUrl);
        request.Headers.Add("X-Reminder-Key", secretKey);
        request.Headers.Add("User-Agent", "TodoAppReminderLambda/1.0");

        var response = await client.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        context.Logger.LogInformation($"Reminder job responded: {response.StatusCode} - {body}");

        return body;
    }
}