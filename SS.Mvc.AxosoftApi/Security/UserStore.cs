using Microsoft.AspNet.Identity;
using SS.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SS.Mvc.AxosoftApi.Properties;
using SS.Mvc.AxosoftApi.Model;

namespace SS.Mvc.AxosoftApi.Security
{
    public class UserStore : IUserRoleStore<User, int>, IUserPasswordStore<User, int>, IUserEmailStore<User, int>, IUserLockoutStore<User, int>, IUserSecurityStampStore<User, int>, IUserStore
    {
        private readonly IWorkspace _workspace;

        public UserStore(IWorkspace workspace)
        {
            _workspace = workspace;
        }

        public Task AddToRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Value of cannot be null or empty.", "roleName");
            }

            var role = _workspace.Single<Role>(x => x.Name.ToUpper() == roleName.ToUpper());
            if (role == null)
            {
                throw new InvalidOperationException(string.Format("Role {0} not found.", roleName));
            }

            var userRole = new UserRole { UserId = user.Id, RoleId = role.Id };
            _workspace.Add(userRole);
            _workspace.SaveChanges();

            return Task.FromResult(0);
        }

        public Task CreateAsync(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            _workspace.Add(user);
            _workspace.SaveChanges();
            return Task.FromResult(0);
        }

        public Task DeleteAsync(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            foreach (var userRole in user.Roles.ToArray())
            {
                _workspace.Delete(userRole);
            }

            _workspace.Delete(user);
            _workspace.SaveChanges();
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            if (email == null) throw new ArgumentNullException("email");
            var user = _workspace.Query<User>(x => x.Email == email).FirstOrDefault();
            return Task.FromResult(user);
        }

        public Task<User> FindByIdAsync(int userId)
        {
            var user = _workspace.Single<User>(x => x.Id == userId, x => x.Roles);
            return Task.FromResult(user);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            if (userName == null) throw new ArgumentNullException("userName");
            var user = _workspace.Single<User>(x => x.UserName.ToUpper() == userName.ToUpper());
            return Task.FromResult(user);
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<string> GetEmailAsync(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            return Task.FromResult(user.IsEmailConfirmed);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.FromResult(true);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            DateTimeOffset dateTimeOffset = user.LockoutEndDate.HasValue ?
                new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDate.Value, DateTimeKind.Utc))
                : new DateTimeOffset();

            return Task.FromResult(dateTimeOffset);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.FromResult(user.PasswordHash);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var roles = _workspace.Query<UserRole>(x => x.UserId == user.Id)
                .Select(x => x.Role.Name)
                .ToList();

            return Task.FromResult<IList<string>>(roles);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            user.AccessFailedCount = user.AccessFailedCount + 1;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Value of cannot be null or empty.", "roleName");
            }

            bool isInRole = false;

            var role = _workspace.Single<Role>(x => x.Name.ToUpper() == roleName.ToUpper());
            if (role != null)
            {
                isInRole = _workspace.Query<UserRole>(x => x.UserId == user.Id && x.RoleId == role.Id).Any();
            }

            return Task.FromResult(isInRole);
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Value of cannot be null or empty.", "roleName");
            }

            var role = _workspace.Single<Role>(x => x.Name.ToUpper() == roleName.ToUpper());
            if (role != null)
            {
                _workspace.Delete<UserRole>(x => x.UserId == user.Id && x.RoleId == role.Id);
                _workspace.SaveChanges();
                //var userRole = _workspace.Single<UserRole>(x => x.UserId == user.Id && x.RoleId == role.Id);
                //if (userRole != null)
                //{
                //	_workspace.Delete(userRole);
                //	_workspace.SaveChanges();
                //}
            }

            return Task.FromResult(0);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task SetEmailAsync(User user, string email)
        {
            if (user == null) throw new ArgumentNullException("user");
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            if (user == null) throw new ArgumentNullException("user");
            user.IsEmailConfirmed = true;
            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.FromResult(0);
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            if (user == null) throw new ArgumentNullException("user");
            DateTime? nullable;
            if (lockoutEnd == DateTimeOffset.MinValue)
            {
                nullable = null;
            }
            else
            {
                nullable = lockoutEnd.UtcDateTime;
            }
            user.LockoutEndDate = nullable;
            return Task.FromResult(0);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            if (user == null) throw new ArgumentNullException("user");
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task UpdateAsync(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            _workspace.Update(user);
            _workspace.SaveChanges();
            return Task.FromResult(0);
        }

        protected void Dispose(bool disposing)
        {
        }

        public Task SetSecurityStampAsync(User user, string stamp)
        {
            if (user == null) throw new ArgumentNullException("user");

            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            return Task.FromResult(user.SecurityStamp);
        }
    }
}