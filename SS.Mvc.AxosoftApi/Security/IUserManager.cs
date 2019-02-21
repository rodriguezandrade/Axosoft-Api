using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SS.Mvc.AxosoftApi.Model;

namespace SS.Mvc.AxosoftApi.Security
{
    public interface IUserManager
    {
        //IQueryable<User> Users { get; }

        IUserTokenProvider<User, int> UserTokenProvider { get; set; }

        IIdentityValidator<User> UserValidator { get; set; }

        Task<IdentityResult> AccessFailedAsync(int userId);

        Task<IdentityResult> AddClaimAsync(int userId, Claim claim);

        Task<IdentityResult> AddLoginAsync(int userId, UserLoginInfo login);

        Task<IdentityResult> AddPasswordAsync(int userId, string password);

        Task<IdentityResult> AddToRoleAsync(int userId, string role);

        Task<IdentityResult> AddToRolesAsync(int userId, params string[] roles);

        Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword);

        Task<IdentityResult> ChangePhoneNumberAsync(int userId, string phoneNumber, string token);

        Task<bool> CheckPasswordAsync(User user, string password);

        Task<IdentityResult> ConfirmEmailAsync(int userId, string token);

        Task<IdentityResult> CreateAsync(User user);

        Task<IdentityResult> CreateAsync(User user, string password);

        Task<ClaimsIdentity> CreateIdentityAsync(User user, string authenticationType);

        Task<IdentityResult> DeleteAsync(User user);

        Task<User> FindAsync(string userName, string password);

        Task<User> FindAsync(UserLoginInfo login);

        Task<User> FindByEmailAsync(string email);

        Task<User> FindByIdAsync(int userId);

        Task<User> FindByNameAsync(string userName);

        Task<string> GenerateChangePhoneNumberTokenAsync(int userId, string phoneNumber);

        Task<string> GenerateEmailConfirmationTokenAsync(int userId);

        Task<string> GeneratePasswordResetTokenAsync(int userId);

        Task<string> GenerateTwoFactorTokenAsync(int userId, string twoFactorProvider);

        Task<string> GenerateUserTokenAsync(string purpose, int userId);

        Task<int> GetAccessFailedCountAsync(int userId);

        Task<IList<Claim>> GetClaimsAsync(int userId);

        Task<string> GetEmailAsync(int userId);

        Task<bool> GetLockoutEnabledAsync(int userId);

        Task<DateTimeOffset> GetLockoutEndDateAsync(int userId);

        Task<IList<UserLoginInfo>> GetLoginsAsync(int userId);

        Task<string> GetPhoneNumberAsync(int userId);

        Task<IList<string>> GetRolesAsync(int userId);

        Task<string> GetSecurityStampAsync(int userId);

        Task<bool> GetTwoFactorEnabledAsync(int userId);

        Task<IList<string>> GetValidTwoFactorProvidersAsync(int userId);

        Task<bool> HasPasswordAsync(int userId);

        Task<bool> IsEmailConfirmedAsync(int userId);

        Task<bool> IsInRoleAsync(int userId, string role);

        Task<bool> IsLockedOutAsync(int userId);

        Task<bool> IsPhoneNumberConfirmedAsync(int userId);

        Task<IdentityResult> NotifyTwoFactorTokenAsync(int userId, string twoFactorProvider, string token);

        void RegisterTwoFactorProvider(string twoFactorProvider, IUserTokenProvider<User, int> provider);

        Task<IdentityResult> RemoveClaimAsync(int userId, Claim claim);

        Task<IdentityResult> RemoveFromRoleAsync(int userId, string role);

        Task<IdentityResult> RemoveFromRolesAsync(int userId, params string[] roles);

        Task<IdentityResult> RemoveLoginAsync(int userId, UserLoginInfo login);

        Task<IdentityResult> RemovePasswordAsync(int userId);

        Task<IdentityResult> ResetAccessFailedCountAsync(int userId);

        Task<IdentityResult> ResetPasswordAsync(int userId, string token, string newPassword);

        Task<IdentityResult> UpdateAsync(User user);

        Task<IdentityResult> UpdateSecurityStampAsync(int userId);

        Task<bool> VerifyChangePhoneNumberTokenAsync(int userId, string token, string phoneNumber);

        Task<bool> VerifyTwoFactorTokenAsync(int userId, string twoFactorProvider, string token);

        Task<bool> VerifyUserTokenAsync(int userId, string purpose, string token);

        Task<IdentityResult> ActivateAccountAsync(int userId, string token, string password);
    }
}