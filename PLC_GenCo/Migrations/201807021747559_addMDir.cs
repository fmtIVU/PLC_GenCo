namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMDir : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MDirSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdComponent = c.Int(nullable: false),
                        OUTStartSignal = c.Int(nullable: false),
                        OUTResetSignal = c.Int(nullable: false),
                        INRunningFB = c.Int(nullable: false),
                        InputType01 = c.Int(nullable: false),
                        InputType02 = c.Int(nullable: false),
                        InputType03 = c.Int(nullable: false),
                        InputType04 = c.Int(nullable: false),
                        InputType05 = c.Int(nullable: false),
                        InputType06 = c.Int(nullable: false),
                        InputType07 = c.Int(nullable: false),
                        InputType08 = c.Int(nullable: false),
                        INExtFault01 = c.Int(nullable: false),
                        INExtFault02 = c.Int(nullable: false),
                        INExtFault03 = c.Int(nullable: false),
                        INExtFault04 = c.Int(nullable: false),
                        INExtFault05 = c.Int(nullable: false),
                        INExtFault06 = c.Int(nullable: false),
                        INExtFault07 = c.Int(nullable: false),
                        INExtFault08 = c.Int(nullable: false),
                        INMeasurement01_Id = c.Int(),
                        INMeasurement02_Id = c.Int(),
                        INMeasurement03_Id = c.Int(),
                        INMeasurement04_Id = c.Int(),
                        INMeasurement05_Id = c.Int(),
                        INMeasurement06_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AIAlarmSetups", t => t.INMeasurement01_Id)
                .ForeignKey("dbo.AIAlarmSetups", t => t.INMeasurement02_Id)
                .ForeignKey("dbo.AIAlarmSetups", t => t.INMeasurement03_Id)
                .ForeignKey("dbo.AIAlarmSetups", t => t.INMeasurement04_Id)
                .ForeignKey("dbo.AIAlarmSetups", t => t.INMeasurement05_Id)
                .ForeignKey("dbo.AIAlarmSetups", t => t.INMeasurement06_Id)
                .Index(t => t.INMeasurement01_Id)
                .Index(t => t.INMeasurement02_Id)
                .Index(t => t.INMeasurement03_Id)
                .Index(t => t.INMeasurement04_Id)
                .Index(t => t.INMeasurement05_Id)
                .Index(t => t.INMeasurement06_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MDirSetups", "INMeasurement06_Id", "dbo.AIAlarmSetups");
            DropForeignKey("dbo.MDirSetups", "INMeasurement05_Id", "dbo.AIAlarmSetups");
            DropForeignKey("dbo.MDirSetups", "INMeasurement04_Id", "dbo.AIAlarmSetups");
            DropForeignKey("dbo.MDirSetups", "INMeasurement03_Id", "dbo.AIAlarmSetups");
            DropForeignKey("dbo.MDirSetups", "INMeasurement02_Id", "dbo.AIAlarmSetups");
            DropForeignKey("dbo.MDirSetups", "INMeasurement01_Id", "dbo.AIAlarmSetups");
            DropIndex("dbo.MDirSetups", new[] { "INMeasurement06_Id" });
            DropIndex("dbo.MDirSetups", new[] { "INMeasurement05_Id" });
            DropIndex("dbo.MDirSetups", new[] { "INMeasurement04_Id" });
            DropIndex("dbo.MDirSetups", new[] { "INMeasurement03_Id" });
            DropIndex("dbo.MDirSetups", new[] { "INMeasurement02_Id" });
            DropIndex("dbo.MDirSetups", new[] { "INMeasurement01_Id" });
            DropTable("dbo.MDirSetups");
        }
    }
}
