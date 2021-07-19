using covid192020.Models.Entities;
using covid192020.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020.Controllers
{
    [Authorize(Roles = "Admin, Assistant, Medic")]
    public class HomeController : Controller
    {
        private readonly UserManager<CustomIdentityUser> userManager;
        public IPatientRepository PatientRepository { get; set; }
        public HomeController(UserManager<CustomIdentityUser> _userManager, IPatientRepository _PatientRepository)
        {
            userManager = _userManager;
            PatientRepository = _PatientRepository;

        }
        public async Task<IActionResult> Index(int Page = 1)
        {
            string UserId = userManager.GetUserId(User);
            var result = await PatientRepository.GetPatients(UserId, Page, 5);
            return View(result);
        }
    }
}
