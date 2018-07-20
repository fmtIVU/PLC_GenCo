namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MAJORupdateV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Components", "Depandancy", c => c.Int(nullable: false));
            AddColumn("dbo.Components", "MatchStatus", c => c.Int(nullable: false));
            AddColumn("dbo.IOs", "ComponentId", c => c.Int(nullable: false));
            DropColumn("dbo.Components", "StandardComponent");
            DropColumn("dbo.Components", "IsParent");
            DropColumn("dbo.IOs", "Standard");
            DropColumn("dbo.IOs", "Parent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IOs", "Parent", c => c.Int());
            AddColumn("dbo.IOs", "Standard", c => c.Int(nullable: false));
            AddColumn("dbo.Components", "IsParent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Components", "StandardComponent", c => c.Int(nullable: false));
            DropColumn("dbo.IOs", "ComponentId");
            DropColumn("dbo.Components", "MatchStatus");
            DropColumn("dbo.Components", "Depandancy");
        }
    }
}
