using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using SS.Mvc.AxosoftApi.Model;

namespace SS.Mvc.AxosoftApi.Security
{
    public class CustomClaimsIdentityFactory : ClaimsIdentityFactory<User, int>
    {
        public CustomClaimsIdentityFactory()
        {
        }

        public override async Task<ClaimsIdentity> CreateAsync(UserManager<User, int> manager, User user, string authenticationType)
        {
            var identity = await base.CreateAsync(manager, user, authenticationType);

            // TODO: Set custom identity values here, using extension methods from IdentityExtensions
            // identity.SetName("John Doe");

            return identity;
        }
    }
}