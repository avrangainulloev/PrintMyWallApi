using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;

namespace PrintMyWallApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : Controller
    {
        [HttpPost("send")]
        public IActionResult SendEmail([FromBody] EmailRequest request)
        {
            // Отправляем письмо вам (администратору)
            SendHtmlEmail(
                toEmail: "avrang.ainulloev@gmail.com",
                subject: "New Quote Request",
                htmlContent: GenerateHtmlForAdmin(request.Name, request.Email, request.Phone, request.Details)
            );

            SendHtmlEmail(
               toEmail: "info@printmywall.ca",
               subject: "New Quote Request",
               htmlContent: GenerateHtmlForAdmin(request.Name, request.Email, request.Phone, request.Details)
           );

            // Отправляем письмо отправителю
            SendHtmlEmail(
                toEmail: request.Email,
                subject: "Thank You for Your Request",
                htmlContent: GenerateHtmlForUser(request.Name)
            );

            return Ok(new { status = "Emails sent successfully" });
        }

        [HttpGet(Name = "TestApi")]
        public IActionResult TestApi()
        {
            return Ok("API is working");
        }

        private void SendHtmlEmail(string toEmail, string subject, string htmlContent)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("PrintMyWall", "printmywallca@gmail.com"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlContent
            };

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                //client.Authenticate("avrang.ainulloev@gmail.com", "iqeb fxzv bzoc icmb");
                client.Authenticate("printmywallca@gmail.com", "lpja kdpk jyyf dolb");
                client.Send(message);
                client.Disconnect(true);
            }
        }

        private string GenerateHtmlForAdmin(string name, string email, string phone, string details)
        {
            return $@"
        <html>
        <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; text-align: center;'>
            <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
                <h2 style='color: #333333;'>New Quote Request from {name}</h2>
                <p style='color: #555555;'>Here is the information the client provided:</p>
                <div style='text-align: left; margin: 20px 0;'>
                    <p><strong>Name:</strong> {name}</p>
                    <p><strong>Email:</strong> {email}</p>
                    <p><strong>Phone:</strong> {phone}</p>
                    <p><strong>Project Details:</strong></p>
                    <p style='background-color: #f0f0f0; padding: 10px; border-radius: 5px;'>{details}</p>
                </div>
            </div>
        </body>
        </html>";
        }

        private string GenerateHtmlForUser(string name)
        {
            return $@"
        <html>
        <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; text-align: center;'>
            <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
                <h2 style='color: #333333;'>Thank You, {name}!</h2>
                <p style='color: #555555;'>Thank you for reaching out to us. We will get back to you shortly.</p>
                <p style='color: #777777;'>Best regards,<br>Your Team</p>
            </div>
        </body>
        </html>";
        }

        public class EmailRequest
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; } // Поле для номера телефона
            public string Details { get; set; }
        }
    }
}
