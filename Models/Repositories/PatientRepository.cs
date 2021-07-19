using covid192020.Areas.Admin.Models;
using covid192020.Infrastructure;
using covid192020.Models.Entities;
using covid192020.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020.Models.Repositories
{
    public interface IPatientRepository
    {
        public Task<(int? Id, string Fio)?> SetPatient(PatientViewModel model, string UserId);
        public Task<(int? Id, string Fio)?> UpdatePatient(PatientViewModel model, string UserId);
        public Task<PaginationViewModel> GetPatients(string UserId, int page, int pageSize);
        public Task<PatientViewModel> GetOnePatient(int PatientId);
        public Task<List<SelectListItem>> GetSelectListItems(string Name, int PatientId, List<int> selected);

        public List<DinamicViewModel> GetDinamics(int PatientId);
        public List<PreparatViewModel> GetPreparats(int PatientId);
        public List<KtViewModel> GetKts(int PatientId);
        public List<IdentVirusViewModel> GetIdentViruses(int PatientId);
        public Task<TreatmentViewModel> GetTreatment(int PatientId);
        public Task<bool> SetTreatment(TreatmentViewModel model);
    }
    public class PatientRepository : IPatientRepository
    {
        private ApplicationContext Context { get; set; } 
        public PatientRepository(ApplicationContext _Context )
        {
            Context = _Context; 
        }
        // Сохранить одного пациента в базе
        public async Task<(int? Id, string Fio)?> SetPatient(PatientViewModel model, string UserId)
        {
            (int? Id, string Fio)? result = null;
            try
            {
                Patient pt = new Patient()
                {
                    Fio = model.Fio,
                    DateInfected = model.DateInfected,
                    DateAdmission = model.DateAdmission,
                    DateOrit = model.DateOrit,
                    Age = model.Age,
                    Weight = model.Weight,
                    CreateDateTime = DateTime.Now,
                    State = 4, // 4 - пункт 'болеет' таблица SelectItems
                    UserId = UserId
                };
                Context.Patients.Add(pt);
          await Context.SaveChangesAsync();
                result = (pt.Id, model.Fio);
                return result;
            }
            catch(Exception e)
            {
                return result;
            }
        }
        // Изменить одного пациента в базе
        public async Task<(int? Id, string Fio)?> UpdatePatient(PatientViewModel model, string UserId)
        {
            (int? Id, string Fio)? result = null;
            try
            {
                var patient = await Context.Patients.FirstOrDefaultAsync(t => t.Id == model.Id);
                string OldFio = patient.Fio;
                patient.Fio = model.Fio;
                patient.DateInfected = model.DateInfected;
                patient.DateAdmission = model.DateAdmission;
                patient.DateOrit = model.DateOrit;
                patient.Age = model.Age;
                patient.Weight = model.Weight;
                patient.State = (int)model.State;
                await Context.SaveChangesAsync();
                result = (patient.Id, OldFio);

                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }
        // Получить список пациентов с приоритетом сортировки текущего пользователя
        public async Task<PaginationViewModel> GetPatients(string UserId, int page, int pageSize)
        {
            var selectState = Context.SelectItems.Where(t => t.Name == "PatientState");
            var count = (from cp in Context.Patients
                         join cc in Context.CustomIdentityUsers on cp.UserId equals cc.Id into ccp
                         from cc in ccp.DefaultIfEmpty() select new { }).Count();
            return new PaginationViewModel
            {
                Page = new PaginationPage(count, page, pageSize),
                Elements = await
                    (from cp in Context.Patients
                     join cc in Context.CustomIdentityUsers on cp.UserId equals cc.Id into ccp
                     from cc in ccp.DefaultIfEmpty()
                     select new ListPatientsViewModel
                     {
                         Id = cp.Id,
                         UserFio = cc.UserFio,
                         Fio = cp.Fio,
                         DateInfected = cp.DateInfected,
                         DateAdmission = cp.DateAdmission,
                         DateOrit = cp.DateOrit,
                         Age = cp.Age,
                         CreateDateTime = cp.CreateDateTime,
                         Weight = cp.Weight,
                         State = selectState.FirstOrDefault(i => i.Id == cp.State).Text,
                         UserId = cc.Id
                     }).OrderByDescending(t => t.UserId == UserId)
                      //.ThenByDescending(t => t.UserFio)
                      .ThenByDescending(t => t.CreateDateTime).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync()
            };
            //return await 
            //        (from cp in Context.Patients 
            //        join cc in Context.CustomIdentityUsers on cp.UserId equals cc.Id into ccp
            //        from cc in ccp.DefaultIfEmpty()
            //         select new ListPatientsViewModel
            //        {
            //            Id = cp.Id,
            //            UserFio = cc.UserFio,
            //            Fio = cp.Fio,
            //            DateInfected = cp.DateInfected,
            //            DateAdmission = cp.DateAdmission,
            //            DateOrit = cp.DateOrit,
            //            Age = cp.Age,
            //            CreateDateTime = cp.CreateDateTime,
            //            Weight = cp.Weight,
            //            State = selectState.FirstOrDefault(i=>i.Id == cp.State).Text,
            //            UserId = cc.Id
            //        }).OrderByDescending(t=>t.UserId == UserId)
            //          .ThenByDescending(t=>t.UserFio)
            //          .ThenByDescending(t=>t.CreateDateTime).ToListAsync();
        }
        // Получить одного пациента
        public async Task<PatientViewModel> GetOnePatient(int PatientId)
        {
            var result = await Context.Patients.FirstOrDefaultAsync(t => t.Id == PatientId);
            return result != null ? new PatientViewModel
            {
                Id = result.Id,
                Fio = result.Fio,
                DateInfected = result.DateInfected,
                DateAdmission = result.DateAdmission,
                DateOrit = result.DateOrit,
                Age = result.Age,
                Weight = result.Weight,
                State = result.State
                
            } : null;
        }
        // Получить значение для Select
        public async Task<List<SelectListItem>> GetSelectListItems(string Name, int PatientId, List<int> selected)
        {

            return await Context.SelectItems
                .Where(t => t.Name == Name)
                .OrderBy(t => t.Number)
                .Select(t => new SelectListItem {
                    Value = t.Id.ToString(),
                    Text = t.Text,
                    Selected = selected != null && selected.Any(i=>i == t.Id)
            }).ToListAsync(); 
        }
        //Получить список динамик пациента
        public List<DinamicViewModel> GetDinamics(int PatientId)
        { 
            var result =  Context.Dinamics.Where(t => t.PatientId == PatientId).Select(t => new DinamicViewModel()
            {
                Id = t.Id,
                CreateDate = t.CreateDate,
                PaO2FiO2 = t.PaO2FiO2,
                SpO2 = t.SpO2,
                CHDD = t.CHDD,
                PKT = t.PKT,
                CRB = t.CRB,
                WBC = t.WBC,
                PLT = t.PLT,
                D_Dim = t.D_Dim,
                Ob_bel = t.Ob_bel,
                Ob_bil = t.Ob_bil,
                Moch = t.Moch,
                Kreat = t.Kreat,
                F_gen = t.F_gen,
                ALT = t.ALT,
                ACT = t.ACT,
                LDG = t.LDG,
                Lymphocytes = t.Lymphocytes,
                Ferritin = t.Ferritin
            }).ToList();
            return result.Count > 0 ? result : null;
        }
        // Получить список препаратов пациента
        public List<PreparatViewModel> GetPreparats(int PatientId)
        {
            var result = Context.Preparats.Where(t => t.PatientId == PatientId).Select(t => new PreparatViewModel()
            {
                Id = t.Id,
                CreateDate = t.CreateDate,
                Title = t.Title,
                CancelDate = t.CancelDate
            }).ToList();
            return result.Count > 0 ? result : null;
        }
        // Получить список КТ пациента
        public List<KtViewModel> GetKts(int PatientId)
        {
            var result = Context.Kts.Where(t => t.PatientId == PatientId).Select(t => new KtViewModel()
            {
                Id = t.Id,
                CreateDate = t.CreateDate,
                Title = t.Title
            }).ToList();
            return result.Count > 0 ? result : null;
        }
        // Получить список Исследования Идентификации Вируса
        public List<IdentVirusViewModel> GetIdentViruses(int PatientId)
        {
            var result = Context.IdentViruses.Where(t => t.PatientId == PatientId).Select(t => new IdentVirusViewModel()
            {
                Id = t.Id,
                CreateDate = t.CreateDate,
                IsIdent = t.IsIdent
            }).ToList();
            return result.Count > 0 ? result : null;
        }
        // Получить последние данные по лечению
        public async Task<TreatmentViewModel> GetTreatment(int PatientId)
        {
            var treatment = Context.Treatments.FirstOrDefault(t => t.PatientId == PatientId);
            var diagnosis = treatment?.Diagnosis != null ? JsonConvert.DeserializeObject<List<int>>(treatment.Diagnosis) : null;
            return new TreatmentViewModel
            {
                PatientId = PatientId,
                LastChangeDate = treatment?.LastChangeDate,
                PatientInfo = await GetOnePatient(PatientId),
                SelectListDiagnosis = await GetSelectListItems("Diagnosis", PatientId, diagnosis),
                Dinamics = GetDinamics(PatientId) ?? new List<DinamicViewModel>(),
                Preparats = GetPreparats(PatientId) ?? new List<PreparatViewModel>(),
                Kts = GetKts(PatientId) ?? new List<KtViewModel>(),
                IdentViruses = GetIdentViruses(PatientId) ?? new List<IdentVirusViewModel>()
            };
        }
        // Сохранить(ввод данных) форму лечение куда входят и динамика и препараты и кт и данные общего значеня 
        public async Task<bool> SetTreatment(TreatmentViewModel model)
        {

            using var trns = await Context.Database.BeginTransactionAsync();
            try
            {
                var treatment = Context.Treatments.FirstOrDefault(t => t.PatientId == model.PatientId);
                long TreatmentId = 0;
                Treatment trtmnt = new Treatment
                {
                    CreateDate = DateTime.Now,
                    Diagnosis = model.Diagnosis != null ? JsonConvert.SerializeObject(model.Diagnosis) : null,
                    PatientId = model.PatientId,
                };

                if (treatment == null)
                {
                    Context.Treatments.Add(trtmnt);
                    await Context.SaveChangesAsync();
                }
                else
                {
                    treatment.LastChangeDate = DateTime.Now;
                    treatment.Diagnosis = model.Diagnosis != null ? JsonConvert.SerializeObject(model.Diagnosis) : null;
                }
                TreatmentId = treatment != null ? treatment.Id : trtmnt.Id;

                List<Dinamic> dinamics = model.Dinamics?.Select(t => new Dinamic()
                {
                    Id = t.Id,
                    CreateDate = t.CreateDate,
                    PaO2FiO2 = t.PaO2FiO2,
                    SpO2 = t.SpO2,
                    CHDD = t.CHDD,
                    PKT = t.PKT,
                    CRB = t.CRB,
                    WBC = t.WBC,
                    PLT = t.PLT,
                    D_Dim = t.D_Dim,
                    Ob_bel = t.Ob_bel,
                    Ob_bil = t.Ob_bil,
                    Moch = t.Moch,
                    Kreat = t.Kreat,
                    F_gen = t.F_gen,
                    ALT = t.ALT,
                    ACT = t.ACT,
                    LDG = t.LDG,
                    Lymphocytes = t.Lymphocytes,
                    Ferritin = t.Ferritin,
                    PatientId = model.PatientId,
                    TreatmentId = TreatmentId
                }).ToList();
                List<Preparat> preparats = model.Preparats?.Select(t => new Preparat()
                {
                    Id = t.Id,
                    CreateDate = t.CreateDate,
                    Title = t.Title,
                    CancelDate = t.CancelDate,
                    PatientId = model.PatientId,
                    TreatmentId = TreatmentId
                }).ToList();
                List<Kt> kts = model.Kts?.Select(t=> new Kt()
                {
                    Id = t.Id,
                    CreateDate = t.CreateDate,
                    Title = t.Title,
                    PatientId = model.PatientId,
                    TreatmentId = TreatmentId
                }).ToList();
                List<IdentVirus> identViruses = model.IdentViruses?.Select(t => new IdentVirus()
                {
                    Id = t.Id,
                    CreateDate = t.CreateDate,
                    IsIdent = t.IsIdent,
                    PatientId = model.PatientId,
                    TreatmentId = TreatmentId
                }).ToList();

                if (dinamics != null && dinamics.Any(t => t.Id == 0))
                    Context.Dinamics.AddRange(dinamics.Where(t => t.Id == 0));  
                if (dinamics != null && dinamics.Any(t => t.Id > 0))
                    Context.Dinamics.UpdateRange(dinamics.Where(t => t.Id > 0));
                if (dinamics != null && dinamics.Any(t => t.Id < 0))
                {
                    List<Dinamic> removeDimanics = new List<Dinamic>();
                    foreach (var i in dinamics.Where(t => t.Id < 0))
                    {
                        //данные на удаление приходят с отрицательным числом id. Формула *=-1 переводит их в положительное число
                        i.Id *= -1;
                        removeDimanics.Add(i);
                    }
                    Context.Dinamics.RemoveRange(removeDimanics);
                }

                if (preparats != null && preparats.Any(t => t.Id == 0))
                    Context.Preparats.AddRange(preparats.Where(t => t.Id == 0));
                if (preparats != null && preparats.Any(t => t.Id > 0))
                    Context.Preparats.UpdateRange(preparats.Where(t => t.Id > 0));
                if (preparats != null && preparats.Any(t => t.Id < 0))
                {
                    List<Preparat> removePreparats = new List<Preparat>();
                    foreach (var i in preparats.Where(t => t.Id < 0))
                    {
                        i.Id *= -1;
                        removePreparats.Add(i);
                    }
                    Context.Preparats.RemoveRange(removePreparats);
                }

                if (kts != null && kts.Any(t => t.Id == 0))
                    Context.Kts.AddRange(kts.Where(t => t.Id == 0));
                if (kts != null && kts.Any(t => t.Id > 0))
                    Context.Kts.UpdateRange(kts.Where(t => t.Id > 0));
                if (kts != null && kts.Any(t => t.Id < 0))
                {
                    List<Kt> removeKts = new List<Kt>();
                    foreach (var i in kts.Where(t => t.Id < 0))
                    {
                        i.Id *= -1;
                        removeKts.Add(i);
                    }
                    Context.Kts.RemoveRange(removeKts);
                }

                if (identViruses != null && identViruses.Any(t => t.Id == 0))
                    Context.IdentViruses.AddRange(identViruses.Where(t => t.Id == 0));
                if (identViruses != null && identViruses.Any(t => t.Id > 0))
                    Context.IdentViruses.UpdateRange(identViruses.Where(t => t.Id > 0));
                if (identViruses != null && identViruses.Any(t => t.Id < 0))
                {
                    List<IdentVirus> removeIdentViruses = new List<IdentVirus>();
                    foreach (var i in identViruses.Where(t => t.Id < 0))
                    {
                        i.Id *= -1;
                        removeIdentViruses.Add(i);
                    }
                    Context.IdentViruses.RemoveRange(removeIdentViruses);
                }

                await Context.SaveChangesAsync();
                await trns.CommitAsync();
                return true;
            }
            catch(Exception e)
            {
                await trns.RollbackAsync();
                return false;
            }
        }
    }
}
