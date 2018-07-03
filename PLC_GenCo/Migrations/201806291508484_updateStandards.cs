namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateStandards : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Components", "ComponentType");
            DropColumn("dbo.Standards", "ComponentType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Standards", "ComponentType", c => c.Int(nullable: false));
            AddColumn("dbo.Components", "ComponentType", c => c.Int(nullable: false));
        }
    }
}
