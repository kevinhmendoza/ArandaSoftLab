namespace ArandaSoftLab.Infrastructure.Audit.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ArandaSoftLab.Infrastructure.Audit.ArandaSoftLabAuditLogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ArandaSoftLab.Infrastructure.Audit.ArandaSoftLabAuditLogContext";
        }

        protected override void Seed(ArandaSoftLab.Infrastructure.Audit.ArandaSoftLabAuditLogContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
