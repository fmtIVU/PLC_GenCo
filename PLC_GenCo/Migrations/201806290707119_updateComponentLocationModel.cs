namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateComponentLocationModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComponentLocations", "Name", c => c.String());
            DropColumn("dbo.ComponentLocations", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ComponentLocations", "Location", c => c.String());
            DropColumn("dbo.ComponentLocations", "Name");
        }
    }
}
