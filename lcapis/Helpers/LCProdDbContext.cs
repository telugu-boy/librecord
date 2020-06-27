using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using lcapis.Entities;

namespace lcapis.Helpers
{
    public class LCProdDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public LCProdDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseLazyLoadingProxies().
                    UseMySql(Configuration.GetConnectionString("LCApisDatabase"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserServer>()
                .HasKey(us => new { us.UserID , us.ServerID });
            //many to many, users <-> servers

            modelBuilder.Entity<UserServer>()
                .HasOne(us => us.Server)
                .WithMany(s => s.UserServers)
                .HasForeignKey(us => us.UserID);

            modelBuilder.Entity<UserServer>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserServers)
                .HasForeignKey(us => us.ServerID);

            modelBuilder.Entity<LCServer>()
                .HasMany(s => s.Invites)
                .WithOne(i => i.Server)
                .IsRequired();
        }
        public DbSet<LCUser> LCUsers { get; set; }
        public DbSet<LCServer> LCServers { get; set; }
        public DbSet<UserServer> UserServers { get; set; }
        public DbSet<LCInvite> LCInvites { get; set; }
    }
}
