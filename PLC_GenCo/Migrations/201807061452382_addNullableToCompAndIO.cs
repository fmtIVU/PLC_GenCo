namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNullableToCompAndIO : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Components", "StandardId", c => c.Int());
            AlterColumn("dbo.Components", "IOId", c => c.Int());
            AlterColumn("dbo.IOs", "ComponentId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.IOs", "ComponentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Components", "IOId", c => c.Int(nullable: false));
            AlterColumn("dbo.Components", "StandardId", c => c.Int(nullable: false));
        }
    }
}
