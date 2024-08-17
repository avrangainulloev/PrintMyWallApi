using Microsoft.AspNetCore.Mvc;
using MimeKit;
//using System.Net.Mail;
using MailKit.Net.Smtp;
namespace PrintMyWallApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   
    public class EmailController : Controller
    {
         

        //[HttpPost("send")]
        //public IActionResult SendEmail([FromBody] EmailRequest request)
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("Avrang", "avrang.ainulloev@gmail.com"));
        //    message.To.Add(new MailboxAddress("", "avrang.ainulloev@gmail.com"));
        //    message.Subject = "Quote Request";

        //    message.Body = new TextPart("plain")
        //    {
        //        Text = $"Name: {request.Name}\nEmail: {request.Email}\nDetails: {request.Details}"
        //    };

        //    using (var client = new SmtpClient())
        //    {
        //        client.Connect("smtp.gmail.com", 587, false);
        //        client.Authenticate("avrang.ainulloev@gmail.com", "iqeb fxzv bzoc icmb");
        //        client.Send(message);
        //        client.Disconnect(true);
        //    }

        //    return Ok(new { status = "Email sent successfully" });
        //}

        [HttpPost("send")]
        public IActionResult SendEmail([FromBody] EmailRequest request)
        {
            // Отправляем письмо вам
            SendHtmlEmail(
                toEmail: "avrang.ainulloev@gmail.com",
                subject: "Новый запрос на предложение",
                htmlContent: GenerateHtmlForAdmin(request.Name, request.Email, request.Details)
            );

            // Отправляем письмо отправителю
            SendHtmlEmail(
                toEmail: request.Email,
                subject: "Спасибо за ваш запрос",
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
            message.From.Add(new MailboxAddress("PrintMyWall", "avrang.ainulloev@gmail.com"));
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
                client.Authenticate("avrang.ainulloev@gmail.com", "iqeb fxzv bzoc icmb");
                client.Send(message);
                client.Disconnect(true);
            }
        }

        private string GenerateHtmlForAdmin(string name, string email, string details)
        {
            return $@"
        <html>
        <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; text-align: center;'>
            <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
                <h2 style='color: #333333;'>Новый запрос от {name}</h2>
                <p style='color: #555555;'>Вот информация, которую отправил клиент:</p>
                <div style='text-align: left; margin: 20px 0;'>
                    <p><strong>Имя:</strong> {name}</p>
                    <p><strong>Email:</strong> {email}</p>
                    <p><strong>Детали проекта:</strong></p>
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
                <h2 style='color: #333333;'>Спасибо, {name}!</h2>
                <p style='color: #555555;'>Спасибо, что связались с нами. Мы свяжемся с вами в ближайшее время.</p>
                <p style='color: #777777;'>С уважением,<br>Ваша команда</p>
            </div>
        </body>
        </html>";
        }
        public class EmailRequest
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Details { get; set; }
            public string ToEmail { get; set; }
        }
    }
}
