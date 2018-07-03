namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DevideComponentStandard : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Standards", "ComponentId");
            DropColumn("dbo.Standards", "Name");
            DropColumn("dbo.Standards", "Location");
            DropColumn("dbo.Standards", "Comment");
            DropColumn("dbo.Standards", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Standards", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Standards", "Comment", c => c.String());
            AddColumn("dbo.Standards", "Location", c => c.String());
            AddColumn("dbo.Standards", "Name", c => c.String());
            AddColumn("dbo.Standards", "ComponentId", c => c.Int());
        }
    }
}
