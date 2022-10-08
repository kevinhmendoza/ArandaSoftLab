namespace ArandaSoftLab.Infrastructure.Audit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditLog",
                c => new
                    {
                        AuditLogID = c.Guid(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 100),
                        EventDateUTC = c.DateTime(nullable: false),
                        EventDateLocalTime = c.DateTime(nullable: false),
                        EventType = c.String(nullable: false, maxLength: 20),
                        TableName = c.String(nullable: false, maxLength: 100),
                        RecordID = c.String(nullable: false, maxLength: 100),
                        ColumnName = c.String(nullable: false, maxLength: 100),
                        OriginalValue = c.String(),
                        NewValue = c.String(),
                        Module = c.String(nullable: false, maxLength: 100),
                        Interactor = c.String(nullable: false, maxLength: 100),
                        IpAddress = c.String(nullable: false, maxLength: 100),
                        HostName = c.String(),
                    })
                .PrimaryKey(t => t.AuditLogID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AuditLog");
        }
    }
}
