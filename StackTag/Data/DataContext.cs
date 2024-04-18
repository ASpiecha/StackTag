using StackTag.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace StackTag.Data
{
    public class DataContext : IdentityDbContext<User>, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DataContext() : base() { }

        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Collective> Collectives => Set<Collective>();
        public DbSet<ExternalLink> ExternalLinks => Set<ExternalLink>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public List<Tag> GetTags()
        {
            return Tags.ToList();
        }
    }
}
