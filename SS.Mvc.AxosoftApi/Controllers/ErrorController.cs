using SS.Logging;
using SS.Web;
using System;
using System.Web.Mvc;

namespace SS.Mvc.AxosoftApi.Controllers
{
    [AllowAnonymous]
    internal class ErrorController : Controller
    {
        private readonly ILogFacade _log;
        private Exception _exception;

        public ErrorController(ILogFacade log)
        {
            _log = log;
        }

        public ActionResult Index()
        {
            LogException(Properties.Resources.UnknownError);
            Response.StatusCode = 500;
            var message = _exception != null ? string.Format("{0}: {1}", _exception.GetType(), _exception.Message)
                : Properties.Resources.UnknownError;

            return Request.IsAjaxRequest() ? Json(new { Message = message }, JsonRequestBehavior.AllowGet) : (ActionResult)View(_exception);
        }

        public ActionResult Error403()
        {
            Response.StatusCode = 403;

            return Request.IsAjaxRequest() ? Json(new { Message = Properties.Resources.Forbidden }, JsonRequestBehavior.AllowGet) : (ActionResult)View();
        }

        public ActionResult Error404()
        {
            Response.StatusCode = 404;

            return Request.IsAjaxRequest() ? Json(new { Message = Properties.Resources.NotFound }, JsonRequestBehavior.AllowGet) : (ActionResult)View("NotFound");
        }

        public ActionResult Error500()
        {
            LogException(Properties.Resources.InternalServerError);
            Response.StatusCode = 500;
            var message = _exception != null ? string.Format("{0}: {1}", _exception.GetType(), _exception.Message)
                : Properties.Resources.InternalServerError;

            return Request.IsAjaxRequest() ? Json(new { Message = message }, JsonRequestBehavior.AllowGet) : (ActionResult)View(_exception);
        }

        private void LogException(string message)
        {
            var exception = RouteData.Values["exception"] as Exception;
            if (exception != null)
            {
                _log.Error(new HttpExceptionWrapper(HttpContext, exception), message);
            }
            if (HttpContext.IsDebuggingEnabled)
            {
                _exception = exception;
            }
        }
    }
}