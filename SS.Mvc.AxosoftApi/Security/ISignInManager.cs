using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using SS.Mvc.AxosoftApi.Model;

namespace SS.Mvc.AxosoftApi.Security
{
    public interface ISignInManager
    {
        IAuthenticationManager AuthenticationManager { get; set; }

        string AuthenticationType { get; set; }

        UserManager<User, int> UserManager { get; set; }

        int ConvertIdFromString(string id);

        string ConvertIdToString(int id);

        Task<ClaimsIdentity> CreateUserIdentityAsync(User user);

        Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo loginInfo, bool isPersistent);

        Task<int> GetVerifiedUserIdAsync();

        Task<bool> HasBeenVerifiedAsync();

        Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout);

        Task<bool> SendTwoFactorCodeAsync(string provider);

        Task SignInAsync(User user, bool isPersistent, bool rememberBrowser);

        Task<SignInStatus> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberBrowser);
    }
}