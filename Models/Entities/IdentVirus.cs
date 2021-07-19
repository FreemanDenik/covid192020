using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020.Models.Entities
{
    public class IdentVirus
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsIdent { get; set; }
        public int PatientId { get; set; }
        public long TreatmentId { get; set; }
    }
}
