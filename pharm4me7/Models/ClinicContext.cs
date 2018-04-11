namespace pharm4me7.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ClinicContext : DbContext
    {
        public ClinicContext()
            : base("name=ClinicConnection")
        {
        }

        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<item> items { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Prescript> Prescripts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<item>()
                .HasMany(e => e.Prescripts)
                .WithRequired(e => e.item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.Prescripts)
                .WithRequired(e => e.Patient)
                .WillCascadeOnDelete(false);
        }

        public System.Data.Entity.DbSet<pharm4me7.Models.POrder> POrders { get; set; }

        public System.Data.Entity.DbSet<pharm4me7.Models.Pharmacy> Pharmacies { get; set; }

        public System.Data.Entity.DbSet<pharm4me7.Models.POrderFill> POrderFills { get; set; }

        public System.Data.Entity.DbSet<pharm4me7.Models.Inventory> Inventories { get; set; }
    }
}
