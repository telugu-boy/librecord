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
            options.UseMySql(Configuration.GetConnectionString("LCApisDatabase"));
        }

        public DbSet<LCMsg> LCMsgs { get; set; }

        public DbSet<LCUser> LCUsers { get; set; }
    }
}
