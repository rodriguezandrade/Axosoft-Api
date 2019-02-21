using System;
using System.Web;
using Autofac;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using SS.Mvc.AxosoftApi.Model;
using SS.Mvc.AxosoftApi.Security;

namespace SS.Mvc.AxosoftApi.Modules
{
    public sealed class WebAuthModule : Module
    {
        private readonly IAppBuilder _app;

        public WebAuthModule(IAppBuilder app)
        {
            _app = app;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => HttpContext.Current.GetOwinContext()).ExternallyOwned();

            var provider = _app.GetDataProtectionProvider();

            var passwordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            var userTokenProvider = new DataProtectorTokenProvider<User, int>(provider.Create("Confirmation"));

            builder.Register(x => HttpContext.Current.GetOwinContext().Authentication).ExternallyOwned();
            builder.RegisterType<CustomClaimsIdentityFactory>().As<IClaimsIdentityFactory<User, int>>().InstancePerRequest();
            builder.RegisterType<UserStore>().AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterType<RoleStore>().AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterType<SignInManager>().As<ISignInManager>().InstancePerRequest();
            builder.RegisterType<RoleManager>().As<IRoleManager>().As<RoleManager<Role, int>>();
            builder.RegisterType<UserManager>().As<IUserManager>().As<UserManager<User, int>>()
                .OnActivated(x =>
                {
                    var manager = x.Instance;
                    manager.UserValidator = new UserValidator<User, int>(manager)
                    {
                        AllowOnlyAlphanumericUserNames = false,
                        RequireUniqueEmail = true
                    };

                    manager.PasswordValidator = passwordValidator;

                    manager.UserLockoutEnabledByDefault = true;
                    manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    manager.MaxFailedAccessAttemptsBeforeLockout = 5;

                    manager.UserTokenProvider = userTokenProvider;
                });
        }
    }
}
