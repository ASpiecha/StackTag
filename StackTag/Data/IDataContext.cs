using StackTag.Entities;
using Microsoft.EntityFrameworkCore;

namespace StackTag.Data
{
    public interface IDataContext
    {
        DbSet<Tag> Tags { get; }
        DbSet<Collective> Collectives { get; }
        DbSet<ExternalLink> ExternalLinks { get; }

        List<Tag> GetTags();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
