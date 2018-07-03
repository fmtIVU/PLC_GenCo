namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateStandards : DbMigration
    {
        public override void Up()
        {
            Sql("SET IDENTITY_INSERT Standards ON");
             
            Sql("INSERT INTO Standards (StandardId, StandardComponent, ComponentType, ConnectionType) VALUES (1, 6, 1, 9 )");
            Sql("INSERT INTO Standards (StandardId, StandardComponent, ComponentType, ConnectionType) VALUES (2, 6, 1, 2 )");
            Sql("INSERT INTO Standards (StandardId, StandardComponent, ComponentType, ConnectionType) VALUES (3, 7, 1, 1 )");
            Sql("INSERT INTO Standards (StandardId, StandardComponent, ComponentType, ConnectionType) VALUES (4, 7, 1, 6 )");
            Sql("INSERT INTO Standards (StandardId, StandardComponent, ComponentType, ConnectionType) VALUES (5, 5, 2, 2 )");
            Sql("INSERT INTO Standards (StandardId, StandardComponent, ComponentType, ConnectionType) VALUES (6, 4, 2, 2 )");
            Sql("INSERT INTO Standards (StandardId, StandardComponent, ComponentType, ConnectionType) VALUES (7, 3, 2, 2 )");
            Sql("INSERT INTO Standards (StandardId, StandardComponent, ComponentType, ConnectionType) VALUES (8, 2, 0, 0 )");
            Sql("INSERT INTO Standards (StandardId, StandardComponent, ComponentType, ConnectionType) VALUES (9, 1, 2, 2 )");
            Sql("INSERT INTO Standards (StandardId, StandardComponent, ComponentType, ConnectionType) VALUES (10, 0, 0, 0 )");
            Sql("INSERT INTO Standards (StandardId, StandardComponent, ComponentType, ConnectionType) VALUES (11, 0, 0, 7 )");
            Sql("INSERT INTO Standards (StandardId, StandardComponent, ComponentType, ConnectionType) VALUES (12, 0, 0, 8 )");

            Sql("SET IDENTITY_INSERT Standards OFF");
        }
        
        public override void Down()
        {
        }
    }
}
