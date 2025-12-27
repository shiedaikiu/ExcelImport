using ExcelImport.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ExcelImport.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Bill> Bills => Set<Bill>();
        public DbSet<Stuff> Stuffs => Set<Stuff>();
        public DbSet<Log> Logs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bill>()
                .HasMany(x => x.Items)
                .WithOne(x => x.Bill)
                .HasForeignKey(x => x.BillSerial)
                .HasPrincipalKey(x => x.BillSerial);

            modelBuilder.Entity<Bill>()
                .HasIndex(x => x.BillSerial)
                .IsUnique(false);
        }

    }

}
