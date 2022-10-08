namespace ArandaSoftLab.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ArandaSoftLab.Infrastructure.Data.ArandaSoftLabContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ArandaSoftLab.Infrastructure.Data.ArandaSoftLabContext";
        }

        protected override void Seed(ArandaSoftLab.Infrastructure.Data.ArandaSoftLabContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
