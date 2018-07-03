namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addModuleAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Modules", "ModuleAddress", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Modules", "ModuleAddress");
        }
    }
}
