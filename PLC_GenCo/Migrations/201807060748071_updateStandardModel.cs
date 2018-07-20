namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateStandardModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Standards", "StandardAOI", c => c.String());
            DropColumn("dbo.Standards", "StandardComponent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Standards", "StandardComponent", c => c.Int(nullable: false));
            DropColumn("dbo.Standards", "StandardAOI");
        }
    }
}
