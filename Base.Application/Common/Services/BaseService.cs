using System.Linq.Expressions;
using Base.Application.Common.Extension;
using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Domain.Entities.Common.Contracts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Base.Application.Common.Services;

public abstract class BaseService<TEntity, TDto>(IApplicationDbContext context)
    where TEntity : class, IAggregateRoot, IEntity<Guid>
    where TDto : class, IDto
{
    protected IApplicationDbContext Context { get; } = context;

    protected DbSet<TEntity> Entities => Context.Set<TEntity>();

    protected virtual async Task<TEntity?> FindAsync(Guid id, CancellationToken cancellationToken = default)
        => await Entities.FindAsync([id], cancellationToken);

    protected virtual async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await Entities.FirstOrDefaultAsync(predicate, cancellationToken);

    protected virtual async Task<TDto?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => await Entities
            .AsNoTracking()
            .Where(e => e.Id == id)
            .ProjectToType<TDto>()
            .FirstOrDefaultAsync(cancellationToken);

    protected virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Entities.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    protected virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    protected virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Entities.Remove(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    protected virtual async Task<List<TDto>> ListAsync(CancellationToken cancellationToken = default)
        => await Entities
            .AsNoTracking()
            .ProjectToType<TDto>()
            .ToListAsync(cancellationToken);

    protected virtual async Task<PaginatedResponse<TDto>> PaginatedSearchAsync(
        PaginationFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = Entities.AsNoTracking().ApplyBaseFilter(filter);
        var count = await query.CountAsync(cancellationToken);
        var list = await query
            .Paginate(filter)
            .ProjectToType<TDto>()
            .ToListAsync(cancellationToken);

        return new PaginatedResponse<TDto>(list, filter.PageNumber, count, filter.PageSize);
    }
}
