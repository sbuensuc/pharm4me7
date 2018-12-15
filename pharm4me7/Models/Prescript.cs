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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Prescript()
        {
            POrders = new HashSet<POrder>();
        }

        public int PrescriptId { get; set; }

        public int PatientId { get; set; }

        public int ItemId { get; set; }

        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "The Amount field is required.")]
        public int? Disp { get; set; }

        [StringLength(10)]
        public string DispType { get; set; }

        [StringLength(4000)]
        public string Sig { get; set; }
        
        [StringLength(256)]
        public string Sub { get; set; }

        public int? Refill { get; set; }

        public int? DoctorId { get; set; }

        public bool? Sent { get; set; }

        public int? RefillsUsed { get; set; }

        public virtual Doctor Doctor { get; set; }

        public virtual item item { get; set; }

        public virtual Patient Patient { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<POrder> POrders { get; set; }
    }
}
