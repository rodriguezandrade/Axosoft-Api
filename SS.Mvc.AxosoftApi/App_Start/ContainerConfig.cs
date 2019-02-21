using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Owin;
using SS.Logging;
using System;
using System.Configuration;
using System.IO;
using System.Net.Configuration;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using SS.Mvc.AxosoftApi.Modules;

namespace SS.Mvc.AxosoftApi
{
    internal static class ContainerConfig
    {
        public static void ConfigureDependencyResolver(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterControllers(assembly);
            // Register API controllers using assembly scanning.
            builder.RegisterApiControllers(assembly);
            builder.RegisterFilterProvider();

            RegisterModules(builder, app);

            var container = builder.Build();
            app.UseAutofacMiddleware(container);

            app.UseAutofacMvc();
            var resolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);

            // This override is needed because Web API is not using DependencyResolver to build controllers
            var config = GlobalConfiguration.Configuration;
            app.UseAutofacWebApi(config);
            var apiResolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = apiResolver;
        }

        private static void RegisterModules(ContainerBuilder builder, IAppBuilder app)
        {
            var httpContext = HttpContext.Current;
            var lifetimeScopeTags = new[] { MatchingScopeLifetimeTags.RequestLifetimeScopeTag };

            var thisAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterModule(new MappingsModule(thisAssembly, typeof(ContainerConfig).Namespace));

            builder.RegisterModule(new DataModule("AppConnection", lifetimeScopeTags));
            builder.RegisterModule(new WebAuthModule(app));
            builder.RegisterModule(new WebServicesModule(app));

            string dumpDir = Path.Combine(HttpRuntime.AppDomainAppPath, "Logs");
            var templateDirectory = httpContext.Server.MapPath(@"~/Content/EmailTemplates");
            var smtpSection = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            if (smtpSection != null && smtpSection.DeliveryMethod == SmtpDeliveryMethod.Network
                && smtpSection.Network.Host != null)
            {
                dumpDir = null;
            }

            builder.RegisterModule(new ServicesModule(templateDirectory, dumpDir, lifetimeScopeTags));
            builder.RegisterModule(new LogModule());
        }
    }
}