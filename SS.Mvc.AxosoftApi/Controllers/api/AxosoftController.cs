using AxosoftAPI.NET;
using AxosoftAPI.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Web.Http.Routing;
using Newtonsoft.Json;
using System.IO;
using SS.Mvc.AxosoftApi.Model.Mailer.Interface;
using System.Net.Mail;
using System.Net.Mime;
using SS.Mvc.AxosoftApi.Services.Interface;

namespace SS.Mvc.AxosoftApi.Controllers.api
{
    [RoutePrefix("api/Axosoft")]
    public class AxosoftController : ApiController
    {
        public readonly IMailerInterface _mailer;
        public AxosoftController(IMailerInterface mailer)
        {
            _mailer = mailer;
        }

        [HttpGet]
        [Route("redirect")]
        public IHttpActionResult redirect()
        {
            return Redirect("https://jonathanandrade.axosoft.com/auth?response_type=code&client_id=xxxxxxxxxxxxxxxxxxxxxxxxxxxxx&redirect_uri=http://localhost:49307/api/axosoft/data&scope=read%20write&expiring=false");
        }

        [HttpGet]
        [Route("data")]
        public IHttpActionResult RedirectResult(string code)
        {
            var url = "https://jonathanandrade.axosoft.com/api/oauth2/token?grant_type=authorization_code&code=" + code + "&redirect_uri=http://localhost:49307/api/axosoft/data&client_id=262a938b-4e5f-47eb-baf4-b0e8d133259b&client_secret=XmfjDGRS_6qW489j4N8pgy76MPVaHZMcXyOy1akjTo9rBULl7Aj_mSlTRO149YmNvVjvXL4VRNkSM5e03zJx0ehm-HpllC5drus7";
            var data = formater(url);

            return Ok(data);
        }

        [HttpGet]
        [Route("listActivities")]
        public IHttpActionResult ListActivities(string code)
        {
            var url = "https://jonathanandrade.axosoft.com/api/v5/users?access_token=" + code;
            var data = formater(url);

            return Ok(data);
        }

        private static object formater(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject(json);

                return data;
            }
        }

        [HttpGet]
        [Route("activities")]
        public void Activities()
        {
            var axosoftClient = new Proxy
            {
                Url = "https://jonathanandrade.axosoft.com/",
                ClientId = "xxxxxxxxxxxxx",
                ClientSecret = "xxxxxxxxxxxxx"
            };

            axosoftClient.ObtainAccessTokenFromAuthorizationCode("xxxxxxxxxxxxxx", "xxxxxxxxxxxxx", ScopeEnum.ReadWrite);

            var proyectResult = axosoftClient.Projects.Get();
        }

        [HttpGet]
        [Route("sendMail")]
        public IHttpActionResult sendMail()
        {
            var Template = DinamicImages();

            _mailer.SendMail("ra@unav.edu.mx", AppResources.TittleMail, Template);

            return Ok();
        }

        private string DinamicImages()
        {
            var Url = "https://jonathanandrade.axosoft.com/";

            var urlPhotho = AppDomain.CurrentDomain.BaseDirectory + AppResources.ImageUrl;
            var wc = new WebClient();
            byte[] bytes = wc.DownloadData(urlPhotho);

            var linkedResource = new LinkedResource(new MemoryStream(bytes), "image/jpg");
            linkedResource.ContentId = AppResources.NameImage;
            linkedResource.TransferEncoding = TransferEncoding.Base64;

            var buildBody = "<img src='cid:{0}'><p>Hi,</p><p>Please <b><a style='cursor:pointer;'  href='{1}'>CLICK HERE</a></b> to go to the Dinner app and fill the Survey to confirm your meals.</p><p>Its very important to confirm your meals <b> into 30 hours</b> otherwise we should not be able to serve your lunch.</p><p>If you have any questions or concern, please let the <b> Administrator </b> know.</p><p>Thank you for helping us improve our business and provide a better service!.</p><p>Regards,</p><p><b>Dinner Team </b></p>";
            return string.Format(buildBody, urlPhotho,  Url);
        }
    }
}
