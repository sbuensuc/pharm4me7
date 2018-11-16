namespace pharm4me7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditPhoneDataType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Patients", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patients", "Phone", c => c.String(maxLength: 50));
        }
    }
}
