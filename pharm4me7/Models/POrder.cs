namespace pharm4me7.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("POrder")]
    public partial class POrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public POrder()
        {
            POrderFills = new HashSet<POrderFill>();
        }

        public int POrderId { get; set; }

        public int PrescriptId { get; set; }

        public int PharmacyId { get; set; }

        public DateTime? DateOrdered { get; set; }

        [StringLength(4000)]
        public string Note { get; set; }

        public bool? Fill { get; set; }

        public bool? Deny { get; set; }

        public bool? Accept { get; set; }

        public virtual Pharmacy Pharmacy { get; set; }

        public virtual Prescript Prescript { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<POrderFill> POrderFills { get; set; }
    }
}
