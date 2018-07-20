namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MDirSetups", "INMeasurement01_Id", "dbo.AIAlarmSetups");
            DropForeignKey("dbo.MDirSetups", "INMeasurement02_Id", "dbo.AIAlarmSetups");
            DropForeignKey("dbo.MDirSetups", "INMeasurement03_Id", "dbo.AIAlarmSetups");
            DropForeignKey("dbo.MDirSetups", "INMeasurement04_Id", "dbo.AIAlarmSetups");
            DropForeignKey("dbo.MDirSetups", "INMeasurement05_Id", "dbo.AIAlarmSetups");
            DropForeignKey("dbo.MDirSetups", "INMeasurement06_Id", "dbo.AIAlarmSetups");
            DropIndex("dbo.MDirSetups", new[] { "INMeasurement01_Id" });
            DropIndex("dbo.MDirSetups", new[] { "INMeasurement02_Id" });
            DropIndex("dbo.MDirSetups", new[] { "INMeasurement03_Id" });
            DropIndex("dbo.MDirSetups", new[] { "INMeasurement04_Id" });
            DropIndex("dbo.MDirSetups", new[] { "INMeasurement05_Id" });
            DropIndex("dbo.MDirSetups", new[] { "INMeasurement06_Id" });
            AddColumn("dbo.AIAlarmSetups", "Comment", c => c.String());
            AddColumn("dbo.DIAlarmSetups", "IdIO", c => c.Int(nullable: false));
            AddColumn("dbo.DIAlarmSetups", "Comment", c => c.String());
            DropColumn("dbo.MDirSetups", "InputType01");
            DropColumn("dbo.MDirSetups", "InputType02");
            DropColumn("dbo.MDirSetups", "InputType03");
            DropColumn("dbo.MDirSetups", "InputType04");
            DropColumn("dbo.MDirSetups", "InputType05");
            DropColumn("dbo.MDirSetups", "InputType06");
            DropColumn("dbo.MDirSetups", "InputType07");
            DropColumn("dbo.MDirSetups", "InputType08");
            DropColumn("dbo.MDirSetups", "INExtFault01");
            DropColumn("dbo.MDirSetups", "INExtFault02");
            DropColumn("dbo.MDirSetups", "INExtFault03");
            DropColumn("dbo.MDirSetups", "INExtFault04");
            DropColumn("dbo.MDirSetups", "INExtFault05");
            DropColumn("dbo.MDirSetups", "INExtFault06");
            DropColumn("dbo.MDirSetups", "INExtFault07");
            DropColumn("dbo.MDirSetups", "INExtFault08");
            DropColumn("dbo.MDirSetups", "INMeasurement01_Id");
            DropColumn("dbo.MDirSetups", "INMeasurement02_Id");
            DropColumn("dbo.MDirSetups", "INMeasurement03_Id");
            DropColumn("dbo.MDirSetups", "INMeasurement04_Id");
            DropColumn("dbo.MDirSetups", "INMeasurement05_Id");
            DropColumn("dbo.MDirSetups", "INMeasurement06_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MDirSetups", "INMeasurement06_Id", c => c.Int());
            AddColumn("dbo.MDirSetups", "INMeasurement05_Id", c => c.Int());
            AddColumn("dbo.MDirSetups", "INMeasurement04_Id", c => c.Int());
            AddColumn("dbo.MDirSetups", "INMeasurement03_Id", c => c.Int());
            AddColumn("dbo.MDirSetups", "INMeasurement02_Id", c => c.Int());
            AddColumn("dbo.MDirSetups", "INMeasurement01_Id", c => c.Int());
            AddColumn("dbo.MDirSetups", "INExtFault08", c => c.Int());
            AddColumn("dbo.MDirSetups", "INExtFault07", c => c.Int());
            AddColumn("dbo.MDirSetups", "INExtFault06", c => c.Int());
            AddColumn("dbo.MDirSetups", "INExtFault05", c => c.Int());
            AddColumn("dbo.MDirSetups", "INExtFault04", c => c.Int());
            AddColumn("dbo.MDirSetups", "INExtFault03", c => c.Int());
            AddColumn("dbo.MDirSetups", "INExtFault02", c => c.Int());
            AddColumn("dbo.MDirSetups", "INExtFault01", c => c.Int());
            AddColumn("dbo.MDirSetups", "InputType08", c => c.Int(nullable: false));
            AddColumn("dbo.MDirSetups", "InputType07", c => c.Int(nullable: false));
            AddColumn("dbo.MDirSetups", "InputType06", c => c.Int(nullable: false));
            AddColumn("dbo.MDirSetups", "InputType05", c => c.Int(nullable: false));
            AddColumn("dbo.MDirSetups", "InputType04", c => c.Int(nullable: false));
            AddColumn("dbo.MDirSetups", "InputType03", c => c.Int(nullable: false));
            AddColumn("dbo.MDirSetups", "InputType02", c => c.Int(nullable: false));
            AddColumn("dbo.MDirSetups", "InputType01", c => c.Int(nullable: false));
            DropColumn("dbo.DIAlarmSetups", "Comment");
            DropColumn("dbo.DIAlarmSetups", "IdIO");
            DropColumn("dbo.AIAlarmSetups", "Comment");
            CreateIndex("dbo.MDirSetups", "INMeasurement06_Id");
            CreateIndex("dbo.MDirSetups", "INMeasurement05_Id");
            CreateIndex("dbo.MDirSetups", "INMeasurement04_Id");
            CreateIndex("dbo.MDirSetups", "INMeasurement03_Id");
            CreateIndex("dbo.MDirSetups", "INMeasurement02_Id");
            CreateIndex("dbo.MDirSetups", "INMeasurement01_Id");
            AddForeignKey("dbo.MDirSetups", "INMeasurement06_Id", "dbo.AIAlarmSetups", "Id");
            AddForeignKey("dbo.MDirSetups", "INMeasurement05_Id", "dbo.AIAlarmSetups", "Id");
            AddForeignKey("dbo.MDirSetups", "INMeasurement04_Id", "dbo.AIAlarmSetups", "Id");
            AddForeignKey("dbo.MDirSetups", "INMeasurement03_Id", "dbo.AIAlarmSetups", "Id");
            AddForeignKey("dbo.MDirSetups", "INMeasurement02_Id", "dbo.AIAlarmSetups", "Id");
            AddForeignKey("dbo.MDirSetups", "INMeasurement01_Id", "dbo.AIAlarmSetups", "Id");
        }
    }
}
