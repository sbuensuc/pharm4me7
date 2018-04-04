namespace pharm4me7.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Inventory")]
    public partial class Inventory
    {
        public int InventoryId { get; set; }

        public int ItemId { get; set; }

        public int? Amount { get; set; }

        [StringLength(20)]
        public string DispType { get; set; }

        [StringLength(256)]
        public string Brand { get; set; }

        public int? PharmacyId { get; set; }

        public virtual item item { get; set; }

        public virtual Pharmacy Pharmacy { get; set; }
    }
}
