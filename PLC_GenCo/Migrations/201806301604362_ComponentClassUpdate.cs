namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComponentClassUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Components", "IsParent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Components", "IsParent");
        }
    }
}
