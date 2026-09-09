/* Commenting out this file for now as we are not using AWS SES for email sending in this project. We are using SendGrid instead. 
 To use AWS SES, uncomment the code below and install AWSSDK.SimpleEmail nuget package
And also do not forget to add this service in program.cs */


//using Amazon;
//using Amazon.SimpleEmail;
//using Amazon.SimpleEmail.Model;

//namespace TodoAppWithLogin.Services
//{

//    public class SesEmailSender : IEmailSender
//    {
//        private const string FromAddress = "noreply@todoapp.com.au";

//        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
//        {
//            using var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.APSoutheast2);

//            var sendRequest = new SendEmailRequest
//            {
//                Source = FromAddress,
//                Destination = new Destination { ToAddresses = new List<string> { toEmail } },
//                Message = new Message
//                {
//                    Subject = new Content(subject),
//                    Body = new Body { Html = new Content { Charset = "UTF-8", Data = htmlMessage } }
//                }
//            };

//            await client.SendEmailAsync(sendRequest);
//        }
//    }
//}