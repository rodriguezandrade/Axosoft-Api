using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using SS.Mvc.AxosoftApi.Model;

namespace SS.Mvc.AxosoftApi.Security
{
    public class SignInManager : SignInManager<User, int>, ISignInManager
    {
        public SignInManager(UserManager<User, int> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            var user = Util.IsEmail(userName) ? (await UserManager.FindByEmailAsync(userName)) : (await UserManager.FindByNameAsync(userName));
            if (user == null)
            {
                return SignInStatus.Failure;
            }

            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                return SignInStatus.LockedOut;
            }

            if (!(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                return SignInStatus.RequiresVerification;
            }

            if (!(await UserManager.CheckPasswordAsync(user, password)))
            {
                if (shouldLockout)
                {
                    await UserManager.AccessFailedAsync(user.Id);
                    if (await UserManager.IsLockedOutAsync(user.Id))
                    {
                        return SignInStatus.LockedOut;
                    }
                }

                return SignInStatus.Failure;
            }

            await SignInAsync(user, isPersistent, false);
            return SignInStatus.Success;
        }
    }

    internal static class Util
    {
        private static readonly EmailAddressAttribute EmailAddressAttribute = new EmailAddressAttribute();

        public static bool IsEmail(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return EmailAddressAttribute.IsValid(value);
            }

            return false;
        }
    }
}