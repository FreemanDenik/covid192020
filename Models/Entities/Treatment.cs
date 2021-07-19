using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020.Models.Entities
{
    public class Treatment
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Diagnosis { get; set; }
        public DateTime? LastChangeDate { get; set; }
        public int PatientId { get; set; }
    }
}
