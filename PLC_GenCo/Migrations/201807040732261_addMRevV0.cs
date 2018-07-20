namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMRevV0 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MRevSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdComponent = c.Int(nullable: false),
                        OUTStartSignalFW = c.Int(),
                        OUTStartSignalBW = c.Int(),
                        OUTResetSignal = c.Int(),
                        INRunningFBFW = c.Int(),
                        INRunningFBBW = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MRevSetups");
        }
    }
}
