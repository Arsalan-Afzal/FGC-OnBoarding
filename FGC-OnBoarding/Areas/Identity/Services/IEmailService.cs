using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Areas.Identity.Services
{
    public class IEmailService : IEmailSender
    {

        public IEmailService()
        {

        }
      public async  Task  SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //You Email Sending Program here
            string fromMail = "info@fgconboarding.com";
            string fromPassword = "2Ze7pk2&";
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(email));
            message.Body = "<html><body> " + htmlMessage + " </body></html>";
            message.IsBodyHtml = true;
            var smtpClient = new SmtpClient("webmail.fgconboarding.com")
            {
                Port = 25,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };
            smtpClient.Send(message);
        }

    }
}
