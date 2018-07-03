namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editIOV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IOs", "MatchStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.IOs", "MatchStatus");
        }
    }
}
