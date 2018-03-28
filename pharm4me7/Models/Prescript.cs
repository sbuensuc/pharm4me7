namespace pharm4me7.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Prescript")]
    public partial class Prescript
    {
        public int PrescriptId { get; set; }

        public int PatientId { get; set; }

        public int ItemId { get; set; }

        public DateTime? Date { get; set; }

        public int? Disp { get; set; }

        [StringLength(10)]
        public string DispType { get; set; }

        [StringLength(4000)]
        public string Sig { get; set; }

        [StringLength(3)]
        public string Sub { get; set; }

        public int? Refill { get; set; }

        public virtual item item { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
