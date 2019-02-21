using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SS.Mvc.AxosoftApi.Startup))]

namespace SS.Mvc.AxosoftApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ContainerConfig.ConfigureDependencyResolver(app);

            ConfigureAuth(app);
        }
    }
}
