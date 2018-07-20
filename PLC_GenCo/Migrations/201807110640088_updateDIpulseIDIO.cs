namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDIpulseIDIO : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DIPulseSetups", "IdIO", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DIPulseSetups", "IdIO");
        }
    }
}
