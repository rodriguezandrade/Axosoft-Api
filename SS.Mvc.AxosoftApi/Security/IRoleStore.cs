using System.Linq;
using System.Threading.Tasks;
using SS.Mvc.AxosoftApi.Model;

namespace SS.Mvc.AxosoftApi.Security
{
    public interface IRoleStore
    {
        Task CreateAsync(Role role);
        Task DeleteAsync(Role role);
        Task<Role> FindByIdAsync(int roleId);
        Task<Role> FindByNameAsync(string roleName);
        Task UpdateAsync(Role role);
    }
}