namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AIAlarmSetupNullableV2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AIAlarmSetups", "IdIO", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AIAlarmSetups", "IdIO", c => c.Int(nullable: false));
        }
    }
}
