using SS.Mvc.AxosoftApi.Model;
using System.Data.Entity.ModelConfiguration;

namespace SS.Mvc.AxosoftApi.Data.Mappings
{
    internal sealed class UserRoleMapping : EntityTypeConfiguration<UserRole>
    {
        public UserRoleMapping()
        {
            HasKey(x => new { x.UserId, x.RoleId });
        }
    }
}