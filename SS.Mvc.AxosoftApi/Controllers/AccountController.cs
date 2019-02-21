using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SS.Mvc.AxosoftApi.ViewModels;
using SS.Mvc.AxosoftApi.Security;
using SS.Mvc.AxosoftApi.Properties;

namespace SS.Mvc.AxosoftApi.Controllers
{
    [Authorize]
    [InitializeIdentity]
    public class AccountController : Controller
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ISignInManager _signInManager;
        private readonly IUserManager _userManager;

        public AccountController(IUserManager userManager, ISignInManager signInManager, IAuthenticationManager authenticationManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ConfirmEmail(int userId, string code)
        {
            if (userId > 0 && !string.IsNullOrEmpty(code))
            {
                return View("ConfirmEmail", new ActivateAccountModel { Id = userId, Token = code });
            }

            return View("ConfirmEmailError");
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmEmail(ActivateAccountModel model)
        {
            if (ModelState.IsValid)
            {
                //var isValidToken = await _userManager.VerifyUserTokenAsync(model.Id, "Confirmation", model.Token);
                //if (!isValidToken)
                //{
                //	return View("ConfirmEmailError");
                //}

                var result = await _userManager.ActivateAccountAsync(model.Id, model.Token, model.Password);

                if (result.Succeeded)
                {
                    return View("ConfirmEmailSuccess");
                }

                return View("ConfirmEmailError");
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Util.IsEmail(model.Email) ? (await _userManager.FindByEmailAsync(model.Email)) : (await _userManager.FindByNameAsync(model.Email));
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                /*
				string code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);

				var emailSender = _templateEmailSenderFactory();
				var variables = new
				{
					ResetPasswordLink = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: Request.Url.Scheme),
					Logo = Url.Absolute(Url.Content("~/Content/images/sonora-logo-nav.jpg"))
				};
				await emailSender.SendAsync("ResetPassword", variables, user.Email, Resources.ResetPasswrod);
				*/
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return View("AccountNotConfirmed", model);
                case SignInStatus.Failure:
                    ModelState.AddModelError("", Resources.InvalidCredentials);
                    return View(model);
                default:
                    throw new InvalidEnumArgumentException("result", (int)result, typeof(SignInStatus));
            }
        }

        public ActionResult LogOff()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return string.IsNullOrEmpty(code) ? View("Error") : View(new ResetPasswordViewModel { Code = code });
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = Util.IsEmail(model.Email) ? (await _userManager.FindByEmailAsync(model.Email)) : (await _userManager.FindByNameAsync(model.Email));
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            AddErrors(result);

            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }

    internal static class UrlHelperExtensions
    {
        public static string Absolute(this UrlHelper url, string relativeUrl)
        {
            return new Uri(url.RequestContext.HttpContext.Request.Url, relativeUrl).AbsoluteUri;
        }
    }
}