using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SS.Mvc.AxosoftApi.Model;

namespace SS.Mvc.AxosoftApi.Security
{
    public class UserManager : UserManager<User, int>, IUserManager
    {
        public UserManager(IUserStore<User, int> store, IClaimsIdentityFactory<User, int> claimsIdentityFactory)
            : base(store)
        {
            ClaimsIdentityFactory = claimsIdentityFactory;
        }

        public virtual async Task<IdentityResult> ActivateAccountAsync(int userId, string token, string password)
        {
            var user = await FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User Id not found.");
            }

            var success = await VerifyUserTokenAsync(userId, "Confirmation", token);
            if (!success)
            {
                return IdentityResult.Failed(new[] { "Invalid token." });
            }

            user.IsEmailConfirmed = true;

            var result = await PasswordValidator.ValidateAsync(password);

            if (!result.Succeeded)
            {
                return result;
            }

            user.PasswordHash = PasswordHasher.HashPassword(password);

            await ((IUserSecurityStampStore<User, int>)Store).SetSecurityStampAsync(user, Guid.NewGuid().ToString());

            return await UpdateAsync(user);
        }
    }
}