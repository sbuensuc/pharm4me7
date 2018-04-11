namespace pharm4me7.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Pharm4MeContext : DbContext
    {
        public Pharm4MeContext()
            : base("name=Pharm4MeConnection")
        {
        }

        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<item> items { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Pharmacist> Pharmacists { get; set; }
        public virtual DbSet<Pharmacy> Pharmacies { get; set; }
        public virtual DbSet<POrder> POrders { get; set; }
        public virtual DbSet<POrderFill> POrderFills { get; set; }
        public virtual DbSet<Prescript> Prescripts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>()
                .HasMany(e => e.POrderFills)
                .WithRequired(e => e.Inventory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<item>()
                .HasMany(e => e.Inventories)
                .WithRequired(e => e.item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<item>()
                .HasMany(e => e.Prescripts)
                .WithRequired(e => e.item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.Prescripts)
                .WithRequired(e => e.Patient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pharmacist>()
                .HasMany(e => e.POrderFills)
                .WithOptional(e => e.Pharmacist)
                .HasForeignKey(e => e.Pharmacist_PharmacistId);

            modelBuilder.Entity<Pharmacist>()
                .HasMany(e => e.POrderFills1)
                .WithOptional(e => e.Pharmacist1)
                .HasForeignKey(e => e.PhamacistId);

            modelBuilder.Entity<Pharmacy>()
                .HasMany(e => e.POrders)
                .WithRequired(e => e.Pharmacy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<POrder>()
                .HasMany(e => e.POrderFills)
                .WithRequired(e => e.POrder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Prescript>()
                .HasMany(e => e.POrders)
                .WithRequired(e => e.Prescript)
                .WillCascadeOnDelete(false);
        }
    }
}
