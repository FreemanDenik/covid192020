using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020.Models.Entities
{
    public class TreatmentHistory
    {
        public long Id { get; set; }
        public DateTime SetDate { get; set; }
        public string History { get; set; }
        public int PatientId { get; set; }
    }
}
