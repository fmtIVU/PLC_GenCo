namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAIAlarmSetup_UseOUT : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AIAlarmSetups", "UseAlarmHH");
            DropColumn("dbo.AIAlarmSetups", "UseAlarmH");
            DropColumn("dbo.AIAlarmSetups", "UseAlarmL");
            DropColumn("dbo.AIAlarmSetups", "UseAlarmLL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AIAlarmSetups", "UseAlarmLL", c => c.Boolean(nullable: false));
            AddColumn("dbo.AIAlarmSetups", "UseAlarmL", c => c.Boolean(nullable: false));
            AddColumn("dbo.AIAlarmSetups", "UseAlarmH", c => c.Boolean(nullable: false));
            AddColumn("dbo.AIAlarmSetups", "UseAlarmHH", c => c.Boolean(nullable: false));
        }
    }
}
