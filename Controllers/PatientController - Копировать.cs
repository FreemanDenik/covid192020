using covid192020.Models.Entities;
using covid192020.Models.Repositories;
using covid192020.Models.ViewModels;
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
    public class PatientController : Controller
    {
        private readonly UserManager<CustomIdentityUser> userManager;
        public IPatientRepository PatientRepository { get; set; }
        public PatientController(UserManager<CustomIdentityUser> _userManager, IPatientRepository _PatientRepository)
        {
            userManager = _userManager;
            PatientRepository = _PatientRepository;

        }
        public async Task<IActionResult> Index()
        {
            string UserId = userManager.GetUserId(User);
            var result = await PatientRepository.GetPatients(UserId);
            return View(result);
        }
        // Добавление/изменение данных GET
        [HttpGet]
        public async Task<IActionResult> AddEditPatient(int Id = 0)
        {
            TempData["ChangePatient"] = false;
            if (Id > 0)
            {
                var result = await PatientRepository.GetOnePatient(Id);
                if (result != null)
                {
                    TempData["ChangePatient"] = true;
                    return View(result);
                }
                else return NotFound();
            }
            return View();
        }
        // Добавление/изменение данных POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditPatient(PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                string UserId = userManager.GetUserId(User);
                string result;
                if (model.Id > 0)
                {
                    TempData["ChangePatient"] = true;
                    TempData["UpdatePatientSuccess_Fio"] = result = await PatientRepository.UpdatePatient(model, UserId);
                }
                else
                {
                    TempData["ChangePatient"] = false;
                    TempData["SetPatientSuccess_Fio"] = result = await PatientRepository.SetPatient(model, UserId);
                }
                if (result != null)
                    return RedirectToAction("Index", "Patient", new { area = "" });
                else
                    ModelState.AddModelError("SpecialError", "Не удалось сохранить данные!");
            }
            else
                ModelState.AddModelError("SpecialError", "Не керръектные данные");
            return View(model);
        }
        // Динамика данных GET
        [HttpGet]
        public async Task<IActionResult> DinamicPatient(int Id)
        {
            var result = await PatientRepository.GetOnePatient(Id);
            if (result != null)
            {
                TempData["PatiebtInfo"] = result;
                TempData["Diagnosis"] = await PatientRepository.GetSelectListItems("Diagnosis");

                return View();
            }
            else return NotFound();
        }
        // Динамика данных POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DinamicPatient(DinamicViewModel model)
        {
     
            if (ModelState.IsValid)
            {
               
           
            }
            return View();
        }
        // Препараты GET
        [HttpGet]
        public IActionResult PreparatPatient()
        {
            return View();
        }

    }

}
