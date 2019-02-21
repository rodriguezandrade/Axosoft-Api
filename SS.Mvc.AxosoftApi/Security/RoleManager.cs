using Microsoft.AspNet.Identity;
using SS.Mvc.AxosoftApi.Model;

namespace SS.Mvc.AxosoftApi.Security
{
    public class RoleManager : RoleManager<Role, int>, IRoleManager
    {
        public RoleManager(IRoleStore<Role, int> store)
            : base(store)
        {
        }
    }
}