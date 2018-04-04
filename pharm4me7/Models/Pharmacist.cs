namespace pharm4me7.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pharmacist
    {
        public int PharmacistId { get; set; }

        [Required]
        [StringLength(256)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(256)]
        public string LastName { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        public int? PharmacyId { get; set; }

        public virtual Pharmacy Pharmacy { get; set; }
    }
}
