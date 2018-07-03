namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addComponentAndLocationToDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComponentLocations",
                c => new
                    {
                        ComponentLocationId = c.Int(nullable: false, identity: true),
                        Location = c.String(),
                        Prefix = c.String(),
                    })
                .PrimaryKey(t => t.ComponentLocationId);
            
            CreateTable(
                "dbo.Components",
                c => new
                    {
                        ComponentId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                        Comment = c.String(),
                        StandardId = c.Int(nullable: false),
                        StandardComponent = c.Int(nullable: false),
                        ComponentType = c.Int(nullable: false),
                        ConnectionType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ComponentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Components");
            DropTable("dbo.ComponentLocations");
        }
    }
}
