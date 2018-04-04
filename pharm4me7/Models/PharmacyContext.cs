namespace pharm4me7.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PharmacyContext : DbContext
    {
        public PharmacyContext()
            : base("name=PharmacyConnection")
        {
        }

        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<item> items { get; set; }
        public virtual DbSet<Pharmacist> Pharmacists { get; set; }
        public virtual DbSet<Pharmacy> Pharmacies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<item>()
                .HasMany(e => e.Inventories)
                .WithRequired(e => e.item)
                .WillCascadeOnDelete(false);
        }
    }
}
