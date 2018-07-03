namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDIAlarm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DIAlarmSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdComponent = c.Int(nullable: false),
                        TimeDelay = c.Int(nullable: false),
                        InputType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DIAlarmSetups");
        }
    }
}
