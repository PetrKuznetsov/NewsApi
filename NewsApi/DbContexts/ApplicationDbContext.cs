using Microsoft.EntityFrameworkCore;
using NewsApi.Models;
using NewsApi.Models.MapConfigurations;

namespace NewsApi.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<News> News { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new NewsDbMap());
        }
    }
}
