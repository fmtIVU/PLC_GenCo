namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAIAlarmSetup_IOid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AIAlarmSetups", "IdIO", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AIAlarmSetups", "IdIO");
        }
    }
}
