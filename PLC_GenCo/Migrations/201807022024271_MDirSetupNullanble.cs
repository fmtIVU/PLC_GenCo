namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MDirSetupNullanble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MDirSetups", "OUTStartSignal", c => c.Int());
            AlterColumn("dbo.MDirSetups", "OUTResetSignal", c => c.Int());
            AlterColumn("dbo.MDirSetups", "INRunningFB", c => c.Int());
            AlterColumn("dbo.MDirSetups", "INExtFault01", c => c.Int());
            AlterColumn("dbo.MDirSetups", "INExtFault02", c => c.Int());
            AlterColumn("dbo.MDirSetups", "INExtFault03", c => c.Int());
            AlterColumn("dbo.MDirSetups", "INExtFault04", c => c.Int());
            AlterColumn("dbo.MDirSetups", "INExtFault05", c => c.Int());
            AlterColumn("dbo.MDirSetups", "INExtFault06", c => c.Int());
            AlterColumn("dbo.MDirSetups", "INExtFault07", c => c.Int());
            AlterColumn("dbo.MDirSetups", "INExtFault08", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MDirSetups", "INExtFault08", c => c.Int(nullable: false));
            AlterColumn("dbo.MDirSetups", "INExtFault07", c => c.Int(nullable: false));
            AlterColumn("dbo.MDirSetups", "INExtFault06", c => c.Int(nullable: false));
            AlterColumn("dbo.MDirSetups", "INExtFault05", c => c.Int(nullable: false));
            AlterColumn("dbo.MDirSetups", "INExtFault04", c => c.Int(nullable: false));
            AlterColumn("dbo.MDirSetups", "INExtFault03", c => c.Int(nullable: false));
            AlterColumn("dbo.MDirSetups", "INExtFault02", c => c.Int(nullable: false));
            AlterColumn("dbo.MDirSetups", "INExtFault01", c => c.Int(nullable: false));
            AlterColumn("dbo.MDirSetups", "INRunningFB", c => c.Int(nullable: false));
            AlterColumn("dbo.MDirSetups", "OUTResetSignal", c => c.Int(nullable: false));
            AlterColumn("dbo.MDirSetups", "OUTStartSignal", c => c.Int(nullable: false));
        }
    }
}
