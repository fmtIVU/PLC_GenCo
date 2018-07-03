namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateIdNames : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ComponentLocations");
            DropPrimaryKey("dbo.Components");
            DropPrimaryKey("dbo.Standards");
            DropColumn("dbo.ComponentLocations", "ComponentLocationId");
            DropColumn("dbo.Components", "ComponentId");
            DropColumn("dbo.Standards", "StandardId");
            AddColumn("dbo.ComponentLocations", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Components", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Standards", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ComponentLocations", "Id");
            AddPrimaryKey("dbo.Components", "Id");
            AddPrimaryKey("dbo.Standards", "Id");

        }
        
        public override void Down()
        {
            AddColumn("dbo.Standards", "StandardId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Components", "ComponentId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.ComponentLocations", "ComponentLocationId", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Standards");
            DropPrimaryKey("dbo.Components");
            DropPrimaryKey("dbo.ComponentLocations");
            DropColumn("dbo.Standards", "Id");
            DropColumn("dbo.Components", "Id");
            DropColumn("dbo.ComponentLocations", "Id");
            AddPrimaryKey("dbo.Standards", "StandardId");
            AddPrimaryKey("dbo.Components", "ComponentId");
            AddPrimaryKey("dbo.ComponentLocations", "ComponentLocationId");
        }
    }
}
