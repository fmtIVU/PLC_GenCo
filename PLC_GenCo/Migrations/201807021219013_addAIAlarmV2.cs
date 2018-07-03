namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAIAlarmV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AIAlarmSetups", "UseAlarmHigh", c => c.Boolean(nullable: false));
            AddColumn("dbo.AIAlarmSetups", "UseAlarmEqual", c => c.Boolean(nullable: false));
            AddColumn("dbo.AIAlarmSetups", "UseAlarmLow", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AIAlarmSetups", "UseAlarmLow");
            DropColumn("dbo.AIAlarmSetups", "UseAlarmEqual");
            DropColumn("dbo.AIAlarmSetups", "UseAlarmHigh");
        }
    }
}
