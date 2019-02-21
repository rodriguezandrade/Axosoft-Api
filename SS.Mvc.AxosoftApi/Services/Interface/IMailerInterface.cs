using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SS.Mvc.AxosoftApi.Services.Interface
{
    public interface IMailerInterface
    {
      void SendMail(string to, string subject, string body);
    }
}