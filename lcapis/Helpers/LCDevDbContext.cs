using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using lcapis.Entities;

namespace lcapis.Helpers
{
    public class LCDevDbContext : LCProdDbContext
    {

        public LCDevDbContext(IConfiguration configuration) : base(configuration) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseLazyLoadingProxies()
                .UseSqlite(Configuration.GetConnectionString("LCApisDatabase"));
        }
    }
}
