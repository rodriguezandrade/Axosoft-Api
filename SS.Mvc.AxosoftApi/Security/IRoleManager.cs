using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SS.Mvc.AxosoftApi.Model;

namespace SS.Mvc.AxosoftApi.Security
{
    public interface IRoleManager
    {
        IQueryable<Role> Roles { get; }

        Task<IdentityResult> CreateAsync(Role role);

        Task<IdentityResult> DeleteAsync(Role role);

        Task<Role> FindByIdAsync(int roleId);

        Task<Role> FindByNameAsync(string roleName);

        Task<bool> RoleExistsAsync(string roleName);

        Task<IdentityResult> UpdateAsync(Role role);
    }
}