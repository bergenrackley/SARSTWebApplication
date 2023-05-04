namespace SARSTWebApplication.Migrations
{
    using SARSTWebApplication.Models;
    using System.Data.Entity.Migrations;

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
                new SarstUser() { userName = "Root_User", password = "password", changePassword = 1 });

            context.ServicesOffered.AddOrUpdate(x => x.serviceName,
                new Service() { serviceName = "Snacks" },
                new Service() { serviceName = "Dinners" },
                new Service() { serviceName = "LaundryServices" },
                new Service() { serviceName = "HygieneProducts" },
                new Service() { serviceName = "MentalHealthServices" },
                new Service() { serviceName = "Other" });
        }
    }
}
