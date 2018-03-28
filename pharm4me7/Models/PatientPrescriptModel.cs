using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pharm4me7.Models
{
    public class PatientPrescriptModel
    {
        public IEnumerable<Prescript> Prescripts { get; set; }
        public Patient Patient { get; set; }
    }
}