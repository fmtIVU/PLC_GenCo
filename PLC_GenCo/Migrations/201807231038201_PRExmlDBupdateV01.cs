namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PRExmlDBupdateV01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Components", "Dependancy", c => c.Int(nullable: false));
            DropColumn("dbo.Components", "Depandancy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Components", "Depandancy", c => c.Int(nullable: false));
            DropColumn("dbo.Components", "Dependancy");
        }
    }
}
