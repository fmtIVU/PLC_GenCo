namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dataValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Components", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Components", "Location", c => c.String(nullable: false));
            AlterColumn("dbo.Components", "Comment", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Components", "Comment", c => c.String());
            AlterColumn("dbo.Components", "Location", c => c.String());
            AlterColumn("dbo.Components", "Name", c => c.String());
        }
    }
}
