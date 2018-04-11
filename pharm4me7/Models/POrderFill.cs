namespace pharm4me7.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("POrderFill")]
    public partial class POrderFill
    {
        public int POrderFillId { get; set; }

        public int POrderId { get; set; }

        public int InventoryId { get; set; }

        public DateTime? DateFilled { get; set; }

        public DateTime? DatePicked { get; set; }

        [StringLength(4000)]
        public string Note { get; set; }

        public int? PhamacistId { get; set; }

        public bool? Ready { get; set; }

        public virtual Inventory Inventory { get; set; }

        public virtual POrder POrder { get; set; }
    }
}
