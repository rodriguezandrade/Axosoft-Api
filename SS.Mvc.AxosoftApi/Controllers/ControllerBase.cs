using System.Web.Mvc;

namespace SS.Mvc.AxosoftApi.Controllers
{
    public abstract class ControllerBase : SS.Mvc.ControllerBase
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            // Output a nice error page
            if (filterContext.HttpContext.IsCustomErrorEnabled)
            {
                filterContext.ExceptionHandled = true;
                Response.StatusCode = 500;
                Response.TrySkipIisCustomErrors = true;
                RedirectToAction("Index", "Error").ExecuteResult(ControllerContext);
            }
        }
    }
}