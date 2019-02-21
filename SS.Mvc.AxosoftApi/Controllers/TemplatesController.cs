using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Mvc.AxosoftApi.Controllers
{
    public class TemplatesController : Controller
    {
        // This will handle templates
        protected override void HandleUnknownAction(string actionName)
        {
            var feature = RouteData.GetRequiredString("feature");
            var view = RouteData.GetRequiredString("view");

            if (!string.IsNullOrEmpty(feature) && !string.IsNullOrEmpty(view))
            {
                RouteData.Values["controller"] = feature;
                RouteData.Values["action"] = view;

                var result = ViewEngines.Engines.FindPartialView(ControllerContext, view);
                if (result != null && result.View != null && result.ViewEngine != null)
                {
                    var viewContext = new ViewContext(ControllerContext, result.View, ViewData, TempData, Response.Output);
                    result.View.Render(viewContext, Response.Output);
                    result.ViewEngine.ReleaseView(ControllerContext, result.View);
                    return;
                }
            }
            base.HandleUnknownAction(actionName);
        }
    }
}