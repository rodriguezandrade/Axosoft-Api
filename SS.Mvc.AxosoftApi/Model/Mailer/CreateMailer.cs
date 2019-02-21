using SS.Mvc.AxosoftApi.Model.Mailer.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace SS.Mvc.AxosoftApi.Model.Mailer
{
    public class CreateMailer : ICreateMailer
    {
        public CreateMailer() { }

        private LinkedResource GetResource()
        {
            var urlPhotho = AppDomain.CurrentDomain.BaseDirectory + AppResources.ImageUrl;
            var wc = new WebClient();
            byte[] bytes = wc.DownloadData(urlPhotho);

            var linkedResource = new LinkedResource(new MemoryStream(bytes), "image/jpg");
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

        private MailMessage BuildMail()
        {
            LinkedResource Resource = GetResource();

            var htmlView = AlternateView.CreateAlternateViewFromString("Prueba Mail", null, "text/html");
            htmlView.LinkedResources.Add(Resource);

            var mail = new MailMessage();
            mail.AlternateViews.Add(htmlView);
            mail.To.Add("ra@unav.edu.mx");
            mail.Subject = "Subject";
            mail.Body = "Body Bienvenido";
            mail.IsBodyHtml = true;

            return mail;
        }

        public void SendMail()
        {
            var smtpclient = CreateSmtpClient();
            try
            {
                var mailData = BuildMail();

                smtpclient.Send(mailData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}