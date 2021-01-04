using TheWardrobe.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TheWardrobe.Models
{
    public class AppDBContext : DbContext
    {
        public AppDBContext() : base("DBConnectionString")
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDBContext, TheWardrobe.Migrations.Configuration>("DBConnectionString"));
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}