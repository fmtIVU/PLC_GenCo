namespace PLC_GenCo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class XML_DB_Major_Delete : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.AIAlarmSetups");
            DropTable("dbo.ComponentLocations");
            DropTable("dbo.Components");
            DropTable("dbo.DIAlarmSetups");
            DropTable("dbo.DIPulseSetups");
            DropTable("dbo.IOs");
            DropTable("dbo.MDirSetups");
            DropTable("dbo.Modules");
            DropTable("dbo.MotFrqSetups");
            DropTable("dbo.MRevSetups");
            DropTable("dbo.PLCs");
            DropTable("dbo.Standards");
            DropTable("dbo.StdVlvSetups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StdVlvSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdComponent = c.Int(nullable: false),
                        OUTOpenSignal = c.Int(),
                        OUTCloseSignal = c.Int(),
                        OUTResetSignal = c.Int(),
                        INOpenedFB = c.Int(),
                        INClosedFB = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Standards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AOIName = c.String(),
                        Description = c.String(),
                        ConnectionType = c.Int(nullable: false),
                        Group = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PLCs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ProductType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MRevSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdComponent = c.Int(nullable: false),
                        OUTStartSignalFW = c.Int(),
                        OUTStartSignalBW = c.Int(),
                        OUTResetSignal = c.Int(),
                        INRunningFBFW = c.Int(),
                        INRunningFBBW = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MotFrqSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdComponent = c.Int(nullable: false),
                        FrqType = c.Int(nullable: false),
                        IPAddress = c.String(),
                        NominalSpeed = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address = c.Int(nullable: false),
                        IOModulesType = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MDirSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdComponent = c.Int(nullable: false),
                        OUTStartSignal = c.Int(),
                        OUTResetSignal = c.Int(),
                        INRunningFB = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IOs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ComponentId = c.Int(),
                        ParentName = c.String(),
                        Location = c.String(),
                        ConnectionType = c.Int(nullable: false),
                        IOAddress_Type = c.Int(nullable: false),
                        IOAddress_IPorMBAddress = c.String(),
                        IOAddress_Rack = c.Int(nullable: false),
                        IOAddress_Module = c.Int(nullable: false),
                        IOAddress_Channel = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        Comment = c.String(maxLength: 255),
                        MatchStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DIPulseSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdComponent = c.Int(nullable: false),
                        IdIO = c.Int(),
                        PulsesPerUnit = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DIAlarmSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdComponent = c.Int(nullable: false),
                        IdIO = c.Int(),
                        Comment = c.String(),
                        TimeDelay = c.Int(),
                        InputType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Components",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StandardId = c.Int(),
                        IOId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 255),
                        Location = c.String(nullable: false),
                        Comment = c.String(maxLength: 255),
                        Dependancy = c.Int(nullable: false),
                        MatchStatus = c.Int(nullable: false),
                        ConnectionType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComponentLocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Prefix = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AIAlarmSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdComponent = c.Int(nullable: false),
                        IdIO = c.Int(),
                        Comment = c.String(),
                        AICType = c.Int(nullable: false),
                        ScaleMax = c.Single(),
                        ScaleMin = c.Single(),
                        TimeDelay = c.Int(),
                        AlarmHH = c.Single(),
                        AlarmH = c.Single(),
                        AlarmL = c.Single(),
                        AlarmLL = c.Single(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
