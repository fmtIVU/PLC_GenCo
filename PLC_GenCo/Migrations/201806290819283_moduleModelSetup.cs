namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moduleModelSetup : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Modules", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Modules", "Name", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
