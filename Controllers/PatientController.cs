using covid192020.Infrastructure;
using covid192020.Models.Entities;
using covid192020.Models.Repositories;
using covid192020.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public async Task<IActionResult> Index(int Page = 1)
        {
            string UserId = userManager.GetUserId(User);
            var result = await PatientRepository.GetPatients(UserId, Page, 20);
            return View(result);
        }
        // Добавление/изменение данных пациента GET
        [HttpGet]
        public async Task<IActionResult> AddEdit(int PatientId = 0)
        {
            TempData["ChangePatient"] = false;
            if (PatientId > 0)
            {
                var result = await PatientRepository.GetOnePatient(PatientId);
                if (result != null)
                {
                    TempData["ChangePatient"] = true;
                    result.SelectListState = await PatientRepository.GetSelectListItems("PatientState", PatientId, new List<int> { result.State ?? 0 });
                    return View(result);
                }
                else return NotFound();
            }
            return View();
        }
        // Добавление/изменение данных пациента POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(PatientViewModel model)
        {
            if (ModelState.IsValid)
            {

                string UserId = userManager.GetUserId(User);
                (int? Id, string Fio)? result; //кортеж
                if (model.Id > 0)
                {
                    TempData["ChangePatient"] = true;
                    result = await PatientRepository.UpdatePatient(model, UserId);
                    TempData["UpdatePatientSuccess_Fio"] = result.Value.Fio;
                }
                else
                {
                    TempData["ChangePatient"] = false;
                    result = await PatientRepository.SetPatient(model, UserId);
                    TempData["SetPatientSuccess_Fio"] = result.Value.Fio;
                }
                if (result != null)
                    return RedirectToAction("AddEdit", "Patient", new { area = "", PatientId = result.Value.Id });
                else
                    ModelState.AddModelError("SpecialError", "Не удалось сохранить данные!");
            }
            else
                ModelState.AddModelError("SpecialError", "Не керръектные данные");
            return View(model);
        }
        // Таблица лечения
        //public async Task<IActionResult> Summary(int PatientId, string Print)
        //{
        //    var patient = await PatientRepository.GetOnePatient(PatientId);

        //    if (patient != null)
        //    {
        //        //ViewBag.PatientInfo = patient;
        //        //ViewBag.Diagnosis = await PatientRepository.GetSelectListItems("Diagnosis", PatientId);
        //        var result = await PatientRepository.GetTreatment(PatientId);
        //        if(Print == null)
        //            return View(result);
        //        else
        //            return View("Partial/Print",result);

        //    }
        //    else return NotFound();
        //}
        /*public IActionResult EarlySummaries(int PatientId)
        {
            var result = PatientRepository.GetHistoriesSummaries(PatientId);
            return View(result);
        }*/
        // Лечение пациента GET
        [HttpGet]
        public async Task<IActionResult> Treatment(int PatientId, string Print = null)
        {
            var patient = await PatientRepository.GetOnePatient(PatientId);

            if (patient != null)
            {
                var result = await PatientRepository.GetTreatment(PatientId);
                if (result != null)
                {
                    if (Print == null)
                        return View(result);
                    else
                        return View("Partial/Print", result);
                }
            }
            return NotFound();
        }
        //Лечение пациента POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TreatmentChange]
        public async Task<IActionResult> Treatment(TreatmentViewModel model)
        {
            ModelState.Clear();
            TryValidateModel(model);

            if (ModelState.IsValid)
            {
               var result = await PatientRepository.SetTreatment(model);
                if (result == true)
                    TempData["SaveTreatment"] = true;
                
            }
            return RedirectToAction("Treatment", "Patient", new { model.PatientId });
        }
       

    }
}
