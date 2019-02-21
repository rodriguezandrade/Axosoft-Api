using SS.Mvc.Filters;
using System.Web.Mvc;
using SS.Logging.NLog;

namespace SS.Mvc.AxosoftApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new SsHandleErrorAttribute { Logger = new NLogLogFacade(typeof(SsHandleErrorAttribute)) });
            // Uncomment this line to protect the whole application
            // filters.Add(new AuthorizeAttribute());
        }
    }
}