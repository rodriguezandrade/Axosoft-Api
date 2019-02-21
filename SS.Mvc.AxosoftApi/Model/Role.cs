using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using SS.Model;
using System.ComponentModel.DataAnnotations;
using SS.Mvc.AxosoftApi.Properties;

namespace SS.Mvc.AxosoftApi.Model
{
    public class Role : Entity, IRole<int>
    {
        [Required]
        [StringLength(50)]
        [Display(Name = nameof(AppResources.Name), ResourceType = typeof(AppResources))]
        public virtual string Name { get; set; }

        public virtual ICollection<UserRole> Users { get; set; }
    }
}