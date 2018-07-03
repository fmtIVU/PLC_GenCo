namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fillStandardTableV3 : DbMigration
    {
        public override void Up()
        {
            Sql("SET IDENTITY_INSERT Standards ON");

            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (1, 0, 2)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (2, 1, 2)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (3, 2, 3)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (4, 3, 0)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (5, 4, 9)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (6, 5, 4)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (7, 6, 9)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (8, 7, 4)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (9, 8, 9)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (10, 9, 0)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (11, 10, 1)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (12, 11, 3)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (13, 12, 2)");


            Sql("SET IDENTITY_INSERT Standards OFF");
        }
        
        public override void Down()
        {
        }
    }
}
