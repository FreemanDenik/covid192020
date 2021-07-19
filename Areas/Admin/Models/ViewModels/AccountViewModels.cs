using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020.Areas.Admin.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserFio { get; set; }
        public List<string> RolesName { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
    public class EditViewModel
    {
        public string Id { get; set; }
        [Required]
        public string UserFio { get; set; }
        public List<string> RolesName { get; set; }
        [Required]
        public string UserName { get; set; }

        
    }
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string UserFio { get; set; }
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
