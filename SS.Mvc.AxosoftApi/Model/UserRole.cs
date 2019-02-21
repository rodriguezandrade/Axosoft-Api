using System;

namespace SS.Mvc.AxosoftApi.Model
{
    public class UserRole : IEquatable<UserRole>
    {
        public Role Role { get; set; }

        public int RoleId { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        public bool Equals(UserRole other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return RoleId == other.RoleId && UserId == other.UserId;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserRole);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (RoleId * 397) ^ UserId;
            }
        }
    }
}