namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PREupdateGenericStdV01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IOs", "IOAddress_Type", c => c.Int(nullable: false));
            AddColumn("dbo.IOs", "IOAddress_IPorMBAddress", c => c.String());
            AddColumn("dbo.IOs", "IOAddress_Rack", c => c.Int(nullable: false));
            AddColumn("dbo.IOs", "IOAddress_Module", c => c.Int(nullable: false));
            AddColumn("dbo.IOs", "IOAddress_Channel", c => c.Int(nullable: false));
            AddColumn("dbo.Standards", "AOIName", c => c.String());
            DropColumn("dbo.IOs", "PLCAddress_Rack");
            DropColumn("dbo.IOs", "PLCAddress_Module");
            DropColumn("dbo.IOs", "PLCAddress_Channel");
            DropColumn("dbo.Standards", "StandardAOI");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Standards", "StandardAOI", c => c.String());
            AddColumn("dbo.IOs", "PLCAddress_Channel", c => c.Int(nullable: false));
            AddColumn("dbo.IOs", "PLCAddress_Module", c => c.Int(nullable: false));
            AddColumn("dbo.IOs", "PLCAddress_Rack", c => c.Int(nullable: false));
            DropColumn("dbo.Standards", "AOIName");
            DropColumn("dbo.IOs", "IOAddress_Channel");
            DropColumn("dbo.IOs", "IOAddress_Module");
            DropColumn("dbo.IOs", "IOAddress_Rack");
            DropColumn("dbo.IOs", "IOAddress_IPorMBAddress");
            DropColumn("dbo.IOs", "IOAddress_Type");
        }
    }
}
