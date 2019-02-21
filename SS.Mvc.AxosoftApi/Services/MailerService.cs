using SS.Mvc.AxosoftApi.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace SS.Mvc.AxosoftApi.Services
{
    public class MailerService : IMailerInterface
    {
        private LinkedResource GetResource()
        {
            var urlPhotho = AppDomain.CurrentDomain.BaseDirectory + AppResources.ImageUrl;
            var wc = new WebClient();
            byte[] bytes = wc.DownloadData(urlPhotho);

            var linkedResource = new LinkedResource(new MemoryStream(bytes), "image/jpeg");
            linkedResource.ContentId = AppResources.NameImage;
            linkedResource.TransferEncoding = TransferEncoding.Base64;

            return linkedResource;
        }

        private SmtpClient CreateSmtpClient()
        {
            var smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            return smtpClient;
        }

        private MailMessage BuildMail(string to, string subject, string body)
        {
            LinkedResource Resource = GetResource(); 
            var htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
            htmlView.LinkedResources.Add(Resource);
     
            var mail = new MailMessage();
            mail.AlternateViews.Add(htmlView);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            return mail;
        }

        public void SendMail(string to, string subject, string body)
        {
            var smtpclient = CreateSmtpClient();

            try
            {
                var mailData = BuildMail(to, subject, body);

                smtpclient.Send(mailData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}