using System;
using Autofac;
using Owin;
using System.Web;
using SS.Web.Caching;
using SS.Caching;
using Newtonsoft.Json;

namespace SS.Mvc.AxosoftApi.Modules
{
    public sealed class WebServicesModule : Module
    {
        private readonly IAppBuilder _app;

        public WebServicesModule(IAppBuilder app)
        {
            _app = app;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(d => new HttpContextWrapper(HttpContext.Current)).As<HttpContextBase>().InstancePerRequest();

            builder.RegisterType<WebApplicationCache>().As<ICacheStore>();

            // For SignalR
            var serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });
            builder.RegisterInstance(serializer);
        }
    }
}
