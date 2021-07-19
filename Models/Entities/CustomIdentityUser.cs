using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020.Models.Entities
{
    public class CustomIdentityUser : IdentityUser
    {
        [Required]
        [MaxLength(256)]
        public string UserFio { get; set; }
    }
}
