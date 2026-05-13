using Microsoft.EntityFrameworkCore;

namespace Base.Application.Common.Interface;

public interface IApplicationDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
