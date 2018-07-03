namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class populateLocationDB : DbMigration
    {
        public override void Up()
        {
            Sql("SET IDENTITY_INSERT ComponentLocations ON");

            Sql("INSERT INTO ComponentLocations (ComponentLocationId, Location, Prefix) VALUES (1, 'Tavle', 'TV01')");
            Sql("INSERT INTO ComponentLocations (ComponentLocationId, Location, Prefix) VALUES (2, 'Sump', 'PS01')");
            Sql("INSERT INTO ComponentLocations (ComponentLocationId, Location, Prefix) VALUES (3, 'Bassin', 'BS01')");

            Sql("SET IDENTITY_INSERT ComponentLocations OFF");
        }
        
        public override void Down()
        {
        }
    }
}
