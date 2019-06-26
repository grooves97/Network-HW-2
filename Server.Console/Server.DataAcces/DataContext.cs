using System;
using System.Data.Entity;
using System.Linq;
using Server.Models;

namespace Server.DataAcces
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
            Database.SetInitializer(new DataInitilizer());
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<Street> Streets { get; set; }
    }
}