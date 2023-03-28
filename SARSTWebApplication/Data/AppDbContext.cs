﻿using SARSTWebApplication.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SARSTWebApplication.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(string connectionString): base(connectionString) { }

        public IDbSet<UserProfile> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>().ToTable("Users");
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