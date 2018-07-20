namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateNullables : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DIAlarmSetups", "TimeDelay", c => c.Int());
            AlterColumn("dbo.DIPulseSetups", "PulsesPerUnit", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DIPulseSetups", "PulsesPerUnit", c => c.Int(nullable: false));
            AlterColumn("dbo.DIAlarmSetups", "TimeDelay", c => c.Int(nullable: false));
        }
    }
}
