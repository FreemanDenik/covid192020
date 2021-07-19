using covid192020.Areas.Admin.Models.ViewModels;
using covid192020.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<CustomIdentityUser> userManager;
        private readonly SignInManager<CustomIdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountController(UserManager<CustomIdentityUser> _userManager, SignInManager<CustomIdentityUser> _signInManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
        }
        // Регестрация пользователя GET 
        [HttpGet]
        public async Task<IActionResult> Register()
        {

            ViewBag.RolesSelectList = await roleManager.Roles.Select(t => new SelectListItem { Text = t.Name }).ToListAsync();

            return View();
        }
        // Регестрация пользователя POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
   
            if (ModelState.IsValid)
            {
                CustomIdentityUser user = new CustomIdentityUser { UserFio = model.UserFio, UserName = model.UserName };
                // добавляем пользователя
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                   _= model.RolesName != null ? await userManager.AddToRolesAsync(user, model.RolesName) : null;
                    // установка куки
                    //await _signInManager.SignInAsync(user, false);
                    TempData["RegistStatusFio"] = model.UserFio;
                    return RedirectToAction("Index", "Home", new { area = "Admin" });

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            ViewBag.RolesSelectList = await roleManager.Roles.Select(t => new SelectListItem { Text = t.Name }).ToListAsync();

            return View(model);
        }
        [HttpGet]
        // Изменение данных пользователя (не трогая пароль) GET
        public async Task<IActionResult> Edit(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            var roles = await userManager.GetRolesAsync(user);
            ViewBag.RolesSelectList = await roleManager.Roles.Select(t => new SelectListItem { Text = t.Name, Selected = (roles.Any(i => i == t.Name)) }).ToListAsync();
            ViewBag.UserId = Id;
            return View(new EditViewModel()
            {
                UserFio = user.UserFio,
                UserName = user.UserName

            });
        }
        // Изменение данных пользователя (не трогая пароль) POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);
                var userFio = user.UserFio;
                user.UserFio = model.UserFio;
                user.UserName = model.UserName;
               
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    //var userFio = user.UserFio;

                    if (model.RolesName != null)
                    {
                        var userRoles = await userManager.GetRolesAsync(user);
                        var addedRoles = model.RolesName?.Except(userRoles);
                        var removedRoles = model.RolesName != null ? userRoles.Except(model.RolesName) : null;

                        _ = addedRoles != null ? await userManager.AddToRolesAsync(user, addedRoles) : null;

                        _ = removedRoles != null ? await userManager.RemoveFromRolesAsync(user, removedRoles) : null;
                    }
                    else
                        _ = model.RolesName == null ? await userManager.RemoveFromRolesAsync(user, await userManager.GetRolesAsync(user)) : null;
                    

                    TempData["EditStatusFio"] = userFio;
                    return RedirectToAction("Edit", "Account", new { area = "Admin", Id = model.Id });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        // Изменение пароля GET
        public async Task<IActionResult> ChangePassword(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            ViewBag.Id = user.Id;
            ViewBag.UserFio = user.UserFio;
            ViewBag.UserName = user.UserName;
            return View(new ChangePasswordViewModel()
            {
                Id = user.Id
            });
        }
        // Изменение пароля POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var result = await userManager.ResetPasswordAsync(user, token, model.Password);
                if (result.Succeeded)
                {
                    TempData["ResetPasswordStatusFio"] = user.UserFio;
                    return RedirectToAction("ChangePassword", "Account", new { area = "Admin", Id = model.Id });
                }else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            ViewBag.UserFio = model.UserFio;
            ViewBag.UserName = model.UserName;
            return View(new ChangePasswordViewModel()
            {
                Id = model.Id
            });
        }
        // Удаление пользователя
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            string userFio = user.UserFio;
            var result = await  userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["DeleteStatusFio"] = userFio;

                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            return RedirectToAction("Edit", "Account", new { area = "Admin" });
        }
    }
}
