namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMotFrqV0 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MotFrqSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdComponent = c.Int(nullable: false),
                        FrqType = c.Int(nullable: false),
                        IPAddress = c.String(),
                        NominalSpeed = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MotFrqSetups");
        }
    }
}
