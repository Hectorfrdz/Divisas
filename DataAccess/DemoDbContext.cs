using Divisas.Models;
using Divisas.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Divisas.DataAccess
{
    public class DemoDbContext : DbContext
    {
        public DbSet<Currency> Currency { get; set; }

        public DbSet<Transaction> Transaction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conexionDb = $"Filename={ConnectionDB.GetRouteFromDatabase("Divisas2.db")}";
            optionsBuilder.UseSqlite(conexionDb);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
