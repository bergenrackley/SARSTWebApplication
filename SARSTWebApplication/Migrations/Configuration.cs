namespace SARSTWebApplication.Migrations
{
    using SARSTWebApplication.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SARSTWebApplication.Data.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SARSTWebApplication.Data.AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            context.SarstUsers.AddOrUpdate(x => x.userName,
                new SarstUser() { userName = "Root_User", password = "password" });

            context.ServicesOffered.AddOrUpdate(x => x.serviceName,
                new Service() { serviceName = "Laundry" },
                new Service() { serviceName = "Food" },
                new Service() { serviceName = "Shower" },
                new Service() { serviceName = "Bed" },
                new Service() { serviceName = "Other" });
        }
    }
}
