using System.Linq;
using System;
using System.Web.Mvc;
using SS.Mvc.Angular;
using SS.Mvc.Angular.Validation;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;

namespace SS.Mvc.AxosoftApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static readonly Version AppVersion;

        //private BackgroundServerTimeTimer _backgroundServerTimeTimer;
        static MvcApplication()
        {
            AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        public static string AppName
        {
            get { return "SS.Mvc.AxosoftApi"; }
        }

        protected void Application_Start()
        {
            foreach (var viewEngine in ViewEngines.Engines.ToList())
            {
                var razorEngine = viewEngine as RazorViewEngine;
                if (razorEngine != null)
                {
                    razorEngine.PartialViewLocationFormats = razorEngine.PartialViewLocationFormats
                         .Concat(new[] { "~/App/components/{1}/{0}.cshtml" })
                         .ToArray();
                }
                else if (viewEngine is WebFormViewEngine)
                {
                    ViewEngines.Engines.Remove(viewEngine);
                }
            }
            MvcHandler.DisableMvcResponseHeader = true;

            //AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ConfigureClientSideValidations();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (Context.IsCustomErrorEnabled)
            {
                ErrorConfig.Handle(Context);
            }
        }

        private static void ConfigureClientSideValidations()
        {
            AngularConfiguration.ErrorCssClass = "has-error";
            AngularConfiguration.HelpCssClass = "help-block";
            // Angular validations
            AngularDataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            //AngularDataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(PasswordComplexityRegularExpressionAttribute), typeof(SS.Mvc.Angular.Validation.RegularExpressionAttributeAdapter));

            // jQuery validations
            //System.Web.Mvc.DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(PasswordComplexityRegularExpressionAttribute), typeof(System.Web.Mvc.RegularExpressionAttributeAdapter));
        }
    }
}