namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IOclassupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IOs", "Standard", c => c.Int(nullable: false));
            AddColumn("dbo.IOs", "Parent", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.IOs", "Parent");
            DropColumn("dbo.IOs", "Standard");
        }
    }
}
