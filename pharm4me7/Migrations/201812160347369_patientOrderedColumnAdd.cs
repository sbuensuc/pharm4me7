namespace pharm4me7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class patientOrderedColumnAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prescript", "Ordered", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prescript", "Ordered");
        }
    }
}
