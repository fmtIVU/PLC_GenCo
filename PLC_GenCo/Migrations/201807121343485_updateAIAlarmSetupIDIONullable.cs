namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAIAlarmSetupIDIONullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DIAlarmSetups", "IdIO", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DIAlarmSetups", "IdIO", c => c.Int(nullable: false));
        }
    }
}
