namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableParent : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.IOs", "Parent", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.IOs", "Parent", c => c.Int(nullable: false));
        }
    }
}
