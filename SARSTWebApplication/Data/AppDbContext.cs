using SARSTWebApplication.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SARSTWebApplication.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(string connectionString): base(connectionString) { }

        public IDbSet<SarstUser> SarstUsers { get; set; }
        public IDbSet<RegistrationRequest> RegistrationRequests { get; set; }
        public IDbSet<Service> ServicesOffered { get; set; }
        public IDbSet<Resident> Residents { get; set; }
        public IDbSet<ResidentStay> ResidentStays { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SarstUser>().ToTable("SarstUsers");
            modelBuilder.Entity<RegistrationRequest>().ToTable("RegistrationRequests");
            modelBuilder.Entity<Service>().ToTable("ServicesOffered");
            modelBuilder.Entity<Resident>().ToTable("Residents");
            modelBuilder.Entity<ResidentStay>().ToTable("ResidentStays");
        }
    }
    public class MyContextFactory : IDbContextFactory<AppDbContext>
    {
        public AppDbContext Create()
        {
            return new AppDbContext("ConnectionName");
        }
    }
}
