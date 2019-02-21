using SS.Mvc.AxosoftApi.Controllers;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SS.Mvc.AxosoftApi
{
    public class ErrorConfig
    {
        public static void Handle(HttpContext context)
        {
            int statusCode = 500;
            var exception = context.Server.GetLastError();

            var httpException = exception as HttpException;

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");

            if (httpException == null)
            {
                routeData.Values.Add("action", "Index");
            }
            else //It's an HttpException, Let's handle it.
            {
                statusCode = httpException.GetHttpCode();
                switch (statusCode)
                {
                    case 401: // Unauthorized
                    case 405: // Method Not Allowed
                        break;

                    case 403: // Forbidden
                        routeData.Values.Add("action", "Error403");
                        break;

                    case 404: // Page not found.
                        routeData.Values.Add("action", "Error404");
                        break;

                    case 500: // Server error.
                        routeData.Values.Add("action", "Error500");
                        break;
                }
            }

            try
            {
                // Avoid IIS7 getting in the middle
                context.Response.TrySkipIisCustomErrors = true;
                context.Response.StatusCode = statusCode;
            }
            catch (HttpException)
            {
                // Catch exception that says:
                // Server cannot set status after HTTP headers have been sent.
                return;
            }

            context.Response.Clear();

            // Pass exception details to the target error View.
            routeData.Values.Add("exception", exception);

            // Clear the error on server.
            context.Server.ClearError();

            var contextWrapper = new HttpContextWrapper(context);
            var requestContext = new RequestContext(contextWrapper, routeData);

            var controller = DependencyResolver.Current.GetService<ErrorController>();

            // Call target Controller and pass the routeData.
            IController errorController = controller;
            errorController.Execute(requestContext);
        }
    }
}