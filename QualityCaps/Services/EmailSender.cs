using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityCaps.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mes = new MimeMessage();
            mes.From.Add(new MailboxAddress("chenl85", "chenl85@myunitec.ac.nz"));
            mes.To.Add(new MailboxAddress("User", email));
            mes.Subject = subject;

            mes.Body = new TextPart("html") {
                Text = message
            };

            using (var client = new SmtpClient()) {

                //for demo-purposes, accept all SSL certificates(in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.office365.com", 587, false);

                //no OAuth2 token, disable the XOAUTH2 authentication mechanism
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                //only need if the SMTP server requires authentication
                client.Authenticate("chenl85@myunitec.ac.nz", "a0vachan");

                client.Send(mes);
                client.Disconnect(true);
            }

            return Task.CompletedTask;
        }
    }
}
