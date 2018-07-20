namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAIAlarmSetup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AIAlarmSetups", "AlarmHH", c => c.Single());
            AddColumn("dbo.AIAlarmSetups", "UseAlarmHH", c => c.Boolean(nullable: false));
            AddColumn("dbo.AIAlarmSetups", "AlarmH", c => c.Single());
            AddColumn("dbo.AIAlarmSetups", "UseAlarmH", c => c.Boolean(nullable: false));
            AddColumn("dbo.AIAlarmSetups", "AlarmL", c => c.Single());
            AddColumn("dbo.AIAlarmSetups", "UseAlarmL", c => c.Boolean(nullable: false));
            AddColumn("dbo.AIAlarmSetups", "AlarmLL", c => c.Single());
            AddColumn("dbo.AIAlarmSetups", "UseAlarmLL", c => c.Boolean(nullable: false));
            DropColumn("dbo.AIAlarmSetups", "AlarmHigh");
            DropColumn("dbo.AIAlarmSetups", "UseAlarmHigh");
            DropColumn("dbo.AIAlarmSetups", "AlarmEqual");
            DropColumn("dbo.AIAlarmSetups", "UseAlarmEqual");
            DropColumn("dbo.AIAlarmSetups", "AlarmLow");
            DropColumn("dbo.AIAlarmSetups", "UseAlarmLow");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AIAlarmSetups", "UseAlarmLow", c => c.Boolean(nullable: false));
            AddColumn("dbo.AIAlarmSetups", "AlarmLow", c => c.Single());
            AddColumn("dbo.AIAlarmSetups", "UseAlarmEqual", c => c.Boolean(nullable: false));
            AddColumn("dbo.AIAlarmSetups", "AlarmEqual", c => c.Single());
            AddColumn("dbo.AIAlarmSetups", "UseAlarmHigh", c => c.Boolean(nullable: false));
            AddColumn("dbo.AIAlarmSetups", "AlarmHigh", c => c.Single());
            DropColumn("dbo.AIAlarmSetups", "UseAlarmLL");
            DropColumn("dbo.AIAlarmSetups", "AlarmLL");
            DropColumn("dbo.AIAlarmSetups", "UseAlarmL");
            DropColumn("dbo.AIAlarmSetups", "AlarmL");
            DropColumn("dbo.AIAlarmSetups", "UseAlarmH");
            DropColumn("dbo.AIAlarmSetups", "AlarmH");
            DropColumn("dbo.AIAlarmSetups", "UseAlarmHH");
            DropColumn("dbo.AIAlarmSetups", "AlarmHH");
        }
    }
}
