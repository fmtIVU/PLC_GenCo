namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIOinDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IOs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Location = c.String(),
                        ConnectionType = c.Int(nullable: false),
                        PLCAddress_Rack = c.Int(nullable: false),
                        PLCAddress_Module = c.Int(nullable: false),
                        PLCAddress_Channel = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        Comment = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.IOs");
        }
    }
}
