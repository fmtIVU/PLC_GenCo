namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateModelAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Modules", "Address", c => c.Int(nullable: false));
            DropColumn("dbo.Modules", "ModuleAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Modules", "ModuleAddress", c => c.Int(nullable: false));
            DropColumn("dbo.Modules", "Address");
        }
    }
}
