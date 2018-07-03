namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIOIDComponent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Components", "IOId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Components", "IOId");
        }
    }
}
