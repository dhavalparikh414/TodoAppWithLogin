/* Commenting out this file for now as we are not using SendGrid for email sending in this project. We are using Brevo instead. 
 To use SendGrid, uncomment the code below and install SendGrid nuget package
And also do not forget to add this service in program.cs */


//using SendGrid;
//using SendGrid.Helpers.Mail;

//namespace TodoAppWithLogin.Services
//{
//    public class SendGridEmailSender : IEmailSender
//    {
//        private readonly string _apiKey;

//        public SendGridEmailSender(IConfiguration config)
//        {
//            _apiKey = config["SendGrid:ApiKey"]!;
//        }

//        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
//        {
//            var client = new SendGridClient(_apiKey);
//            var msg = MailHelper.CreateSingleEmail(
//                new EmailAddress("noreply@todoapp.com.au", "TodoApp"),
//                new EmailAddress(toEmail),
//                subject,
//                plainTextContent: "",
//                htmlContent: htmlMessage);

//            var response = await client.SendEmailAsync(msg);

//            if ((int)response.StatusCode >= 400)
//            {
//                var body = await response.Body.ReadAsStringAsync();
//                throw new Exception($"SendGrid email failed: {response.StatusCode} - {body}");
//            }
//        }
//    }
//}