using System.Net.Mail;
using System.Net;
namespace ProjectPRN221.Helpers
{
    public class SendPasswordMail
    {
        public async static void SendMail(string email, string body)
        {

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "",
                    Password = ""
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                var message = new MailMessage();
                message.To.Add(email);
                message.Subject = "New password";
                message.Body = body;
                message.IsBodyHtml = true;
                message.From = new MailAddress("");
                await smtp.SendMailAsync(message);
            }
        }
    }
}
