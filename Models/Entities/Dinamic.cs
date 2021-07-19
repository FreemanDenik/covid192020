using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020.Models.Entities
{
    public class Dinamic
    {
        public long Id { get; set; }
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
        public int PatientId { get; set; }
        public long TreatmentId { get; set; }
    }
}
