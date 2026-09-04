using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace TodoAppWithLogin.Services
{

    public class SesEmailSender : IEmailSender
    {
        private const string FromAddress = "noreply@todoapp.com.au";

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            using var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.APSoutheast2);

            var sendRequest = new SendEmailRequest
            {
                Source = FromAddress,
                Destination = new Destination { ToAddresses = new List<string> { toEmail } },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body { Html = new Content { Charset = "UTF-8", Data = htmlMessage } }
                }
            };

            await client.SendEmailAsync(sendRequest);
        }
    }
}