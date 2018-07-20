namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStdVlvV0 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StdVlvSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdComponent = c.Int(nullable: false),
                        OUTOpenSignal = c.Int(),
                        OUTCloseSignal = c.Int(),
                        OUTResetSignal = c.Int(),
                        INOpenedFB = c.Int(),
                        INClosedFB = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StdVlvSetups");
        }
    }
}
