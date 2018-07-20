namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateIOParentName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IOs", "ParentName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.IOs", "ParentName");
        }
    }
}
