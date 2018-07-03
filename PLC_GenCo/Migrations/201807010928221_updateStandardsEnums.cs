namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateStandardsEnums : DbMigration
    {
        public override void Up()
        {
            Sql("SET IDENTITY_INSERT Standards ON");

            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (1, 0, 3)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (2, 1, 3)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (3, 2, 4)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (4, 3, 1)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (5, 4, 10)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (6, 5, 5)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (7, 6, 10)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (8, 7, 5)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (9, 8, 10)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (10, 9, 1)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (11, 10, 2)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (12, 11, 4)");
            Sql("INSERT INTO Standards (Id, StandardComponent, ConnectionType) VALUES (13, 12, 3)");


            Sql("SET IDENTITY_INSERT Standards OFF");
        }
        
        public override void Down()
        {
        }
    }
}
