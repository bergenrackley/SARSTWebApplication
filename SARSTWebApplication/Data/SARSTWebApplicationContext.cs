using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SARSTWebApplication.Models;

namespace SARSTWebApplication.Data
{
    public class SARSTWebApplicationContext : DbContext
    {
        public SARSTWebApplicationContext (DbContextOptions<SARSTWebApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<SARSTWebApplication.Models.ResidentStay> ResidentStay { get; set; } = default!;
    }
}
