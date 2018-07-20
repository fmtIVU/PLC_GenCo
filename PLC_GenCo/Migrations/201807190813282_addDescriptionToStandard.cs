namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDescriptionToStandard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Standards", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Standards", "Description");
        }
    }
}
