namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAIAlarm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AIAlarmSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdComponent = c.Int(nullable: false),
                        AICType = c.Int(nullable: false),
                        ScaleMax = c.Single(nullable: false),
                        ScaleMin = c.Single(nullable: false),
                        TimeDelay = c.Int(nullable: false),
                        AlarmHigh = c.Single(nullable: false),
                        AlarmEqual = c.Single(nullable: false),
                        AlarmLow = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AIAlarmSetups");
        }
    }
}
