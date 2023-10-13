using Microsoft.EntityFrameworkCore;
using TNC_API.Interfaces;
using TNC_API.Models;

namespace TNC_API.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PCR> PCRs { get; set; }
        public DbSet<PCRL> PCRLs { get; set; }
        public DbSet<PCL> PCLs { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<PCR>().ToTable("PettyCashRequests");
            modelBuilder.Entity<PCRL>().ToTable("PettyCashRequestLists");
            modelBuilder.Entity<PCL>().ToTable("PettyCashLogs");
            modelBuilder.Entity<RequestType>().ToTable("RequestTypes");
            modelBuilder.Entity<Site>().ToTable("Sites");
            modelBuilder.Entity<Status>().ToTable("Statuses");
        }
    }
}
