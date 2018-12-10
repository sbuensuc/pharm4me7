namespace pharm4me7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prescriptaddcolumnRefillsUsed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prescript", "RefillsUsed", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prescript", "RefillsUsed");
        }
    }
}
