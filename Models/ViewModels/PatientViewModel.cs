using covid192020.Infrastructure;
using covid192020.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020.Models.ViewModels
{

    // Модель списка пациентов
    public class ListPatientsViewModel
    {
        public int Id { get; set; }
        public string UserFio { get; set; }
        public string Fio { get; set; }
        //Дата заболевания
        public DateTime DateInfected { get; set; }
        //Дата поступления
        public DateTime DateAdmission { get; set; }
        //Дата поступления в ОРИТ
        public DateTime DateOrit { get; set; }

        public byte Age { get; set; }
        public DateTime CreateDateTime { get; set; }

        public short Weight { get; set; }
        public string State { get; set; }
        public string UserId { get; set; }
    }
    // ВАЛИДАЦИЯ
    // Модель одного пацианета с валидацией
    public class PatientViewModel
    {
        public int? Id { get; set; }
        // ФИО
        [Required]
        public string Fio { get; set; }
        //Дата заболевания
        [MyRangeDays(DaysBack = 100, DaysForword = 30)]
        [DataType(DataType.Date)]
        public DateTime DateInfected { get; set; }
        /*{
            get { return this._DateInfected; }
            set { this._DateInfected = value.Date;}
        }*/
        //private DateTime _DateInfected { get; set; }
        //Дата поступления
        [MyRangeDays(DaysBack = 100, DaysForword = 30)]
        [DataType(DataType.Date)]
        public DateTime DateAdmission { get; set; }
        /*{
            get{ return this._DateAdmission; }
            set{ this._DateAdmission = value.Date; }
        }*/
        //private DateTime _DateAdmission { get; set; }
        //Дата поступления в ОРИТ
        [MyRangeDays(DaysBack = 100, DaysForword = 30)]
        [DataType(DataType.Date)]
        public DateTime DateOrit { get; set; }

        [Range(1, 170)]
        public byte Age { get; set; }
        [Range(1, 300)]
        public short Weight { get; set; }
        public int? State { get; set; }
        public List<SelectListItem> SelectListState { get; set; }
        /* public string Diagnosis { get; set; }
         public DateTime? DateVirusIdentification { get; set; }*/
    }
    // ВАЛИДАЦИЯ
    // Модель динамики с валидацией

    public class TreatmentViewModel
    {
        public int PatientId { get; set; }
        public DateTime? LastChangeDate { get; set; }
        public PatientViewModel PatientInfo { get; set; }
        [Required]
        public List<int> Diagnosis { get; set; }
        public List<SelectListItem> SelectListDiagnosis { get; set; }
        // Идентификация вируса
        //[FullDateValid]
        //[MyRangeDays(DaysBack = 100, DaysForword = 30)]
        //public DateTime? DateVirusIdentification { get; set; }
        public List<DinamicViewModel> Dinamics { get; set; }
        public List<PreparatViewModel> Preparats { get; set; }

        public List<KtViewModel> Kts { get; set; }
        public List<IdentVirusViewModel> IdentViruses { get; set; }
        //[MaxLength(512)]
        //public string Note { get; set; }
    }
    public class IdentVirusViewModel
    {
        public long Id { get; set; }
        [Required]
        [MyRangeDays(DaysBack = 100, DaysForword = 30)]
        public DateTime CreateDate { get; set; }

        public bool IsIdent { get; set; }
    }
    public class KtViewModel
    {
        public long Id { get; set; }
        [Required]
        [MyRangeDays(DaysBack = 100, DaysForword = 30)]
        public DateTime CreateDate { get; set; }
        [Required]
        public string Title { get; set; }
    }
    public class PreparatViewModel
    {
        public long Id { get; set; }
        [Required]
        [MyRangeDays(DaysBack = 100, DaysForword = 30)]
        public DateTime CreateDate { get; set; }
        [Required]
        public string Title { get; set; }
        //[FullDateValid]
        [MyRangeDays(DaysBack = 100, DaysForword = 30)]
        public DateTime? CancelDate { get; set; }
        //public bool Cancel { get; set; }

    }
    public class DinamicViewModel
    {
        public long Id { get; set; } = 0;
        [Required]
        [MyRangeDays(DaysBack = 100, DaysForword = 10)]
        public DateTime CreateDate { get; set; }
        public string PaO2FiO2 { get; set; }
        public string SpO2 { get; set; }
        public string CHDD { get; set; }
        public string PKT { get; set; }
        public string CRB { get; set; }
        public string WBC { get; set; }
        public string PLT { get; set; }
        public string D_Dim { get; set; }
        public string Ob_bel { get; set; }
        public string Ob_bil { get; set; }
        public string Moch { get; set; }
        public string Kreat { get; set; }
        public string F_gen { get; set; }
        public string ALT { get; set; }
        public string ACT { get; set; }
        public string LDG { get; set; }
        public string Lymphocytes { get; set; }
        public string Ferritin { get; set; }
    }
    //public class CustomRangeDateAttribute : RangeAttribute
    //{
    //    public CustomRangeDateAttribute()
    //      : base(typeof(DateTime),
    //              DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd"),
    //              DateTime.Now.ToString("yyyy-MM-dd"))
    //    { }
    //}
}
