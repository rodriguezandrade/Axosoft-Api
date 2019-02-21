using System;
using System.Web.Mvc;

namespace SS.Mvc.AxosoftApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

#if DEBUG
	
		[System.Diagnostics.DebuggerStepThrough]
		public ActionResult ErrorTest()
		{
			throw new ApplicationException("Error test.");
		}

#endif
    }
}