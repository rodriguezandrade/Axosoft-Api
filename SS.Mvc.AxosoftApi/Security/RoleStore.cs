using System.Linq;
using Microsoft.AspNet.Identity;
using SS.Data.EntityFramework;
using System;
using System.Threading.Tasks;
using SS.Mvc.AxosoftApi.Model;

namespace SS.Mvc.AxosoftApi.Security
{
    public class RoleStore : IRoleStore, IRoleStore<Role, int>
    {
        private readonly IWorkspace _workspace;

        public RoleStore(IWorkspace workspace)
        {
            _workspace = workspace;
        }

        public Task CreateAsync(Role role)
        {
            if (role == null) throw new ArgumentNullException("role");

            _workspace.Add(role);
            _workspace.SaveChanges();
            return Task.FromResult(0);
        }

        public Task DeleteAsync(Role role)
        {
            if (role == null) throw new ArgumentNullException("role");

            _workspace.Delete(role);
            _workspace.SaveChanges();
            return Task.FromResult(0);
        }

        public void Dispose()
        {
        }

        public Task<Role> FindByIdAsync(int roleId)
        {
            var role = _workspace.Single<Role>(x => x.Id == roleId);
            return Task.FromResult(role);
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            if (roleName == null) throw new ArgumentNullException("roleName");

            var role = _workspace.Single<Role>(x => x.Name.ToUpper() == roleName.ToUpper());
            return Task.FromResult(role);
        }

        public Task UpdateAsync(Role role)
        {
            if (role == null) throw new ArgumentNullException("role");

            _workspace.Update(role);
            _workspace.SaveChanges();
            return Task.FromResult(0);
        }
    }
}