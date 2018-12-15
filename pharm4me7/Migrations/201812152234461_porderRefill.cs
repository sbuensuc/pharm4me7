namespace pharm4me7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class porderRefill : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.POrder", "Refill", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.POrder", "Refill");
        }
    }
}
