using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using SS.Model;
using System.ComponentModel.DataAnnotations;

namespace SS.Mvc.AxosoftApi.Model
{
    public class User : Entity, IUser<int>
    {
        public virtual int AccessFailedCount { get; set; }

        public DateTime? LockoutEndDate { get; set; }

        [Required]
        [StringLength(150)]
        [EmailAddress]
        [Display(Name = nameof(AppResources.Email), ResourceType = typeof(AppResources))]
        public virtual string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        public virtual string PasswordHash { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = nameof(AppResources.UserName), ResourceType = typeof(AppResources))]
        public virtual string UserName { get; set; }

        /// <summary>
        /// A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        public virtual string SecurityStamp { get; set; }
    }
}
