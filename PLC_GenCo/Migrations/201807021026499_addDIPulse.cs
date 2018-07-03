namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDIPulse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DIPulseSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdComponent = c.Int(nullable: false),
                        PulsesPerUnit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DIPulseSetups");
        }
    }
}
