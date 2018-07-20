namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGroupToStandard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Standards", "Group", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Standards", "Group");
        }
    }
}
