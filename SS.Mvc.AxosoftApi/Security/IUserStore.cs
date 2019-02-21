using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using SS.Mvc.AxosoftApi.Model;

namespace SS.Mvc.AxosoftApi.Security
{
    public interface IUserStore
    {
        //IQueryable<User> Users { get; }

        Task AddToRoleAsync(User user, string roleName);

        Task CreateAsync(User user);

        Task DeleteAsync(User user);

        Task<User> FindByIdAsync(int userId);

        Task<User> FindByNameAsync(string userName);

        Task<IList<string>> GetRolesAsync(User user);

        Task<bool> IsInRoleAsync(User user, string roleName);

        Task RemoveFromRoleAsync(User user, string roleName);

        Task UpdateAsync(User user);
    }
}