using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020.Models.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public string Fio { get; set; }
        //Дата заболевания
        public DateTime DateInfected { get; set; }
        //Дата поступления
        public DateTime DateAdmission { get; set; }
        //Дата поступления в ОРИТ
        public DateTime DateOrit { get; set; }
        public byte Age { get; set; }
        public short Weight { get; set; }
        public DateTime CreateDateTime { get; set; }
        //public DateTime? LastTreatmentDate { get; set; }
        //public string Diagnosis { get; set; }
     //   public DateTime? DateVirusIdentification { get; set; }
        public int State { get; set; }
        public string UserId { get; set; }
    }
}
