namespace pharm4me7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class patientaddemailcolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "Email", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patients", "Email");
        }
    }
}
