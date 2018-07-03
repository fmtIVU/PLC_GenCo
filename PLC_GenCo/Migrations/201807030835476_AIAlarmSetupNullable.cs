namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AIAlarmSetupNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AIAlarmSetups", "ScaleMax", c => c.Single());
            AlterColumn("dbo.AIAlarmSetups", "ScaleMin", c => c.Single());
            AlterColumn("dbo.AIAlarmSetups", "TimeDelay", c => c.Int());
            AlterColumn("dbo.AIAlarmSetups", "AlarmHigh", c => c.Single());
            AlterColumn("dbo.AIAlarmSetups", "AlarmEqual", c => c.Single());
            AlterColumn("dbo.AIAlarmSetups", "AlarmLow", c => c.Single());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AIAlarmSetups", "AlarmLow", c => c.Single(nullable: false));
            AlterColumn("dbo.AIAlarmSetups", "AlarmEqual", c => c.Single(nullable: false));
            AlterColumn("dbo.AIAlarmSetups", "AlarmHigh", c => c.Single(nullable: false));
            AlterColumn("dbo.AIAlarmSetups", "TimeDelay", c => c.Int(nullable: false));
            AlterColumn("dbo.AIAlarmSetups", "ScaleMin", c => c.Single(nullable: false));
            AlterColumn("dbo.AIAlarmSetups", "ScaleMax", c => c.Single(nullable: false));
        }
    }
}
