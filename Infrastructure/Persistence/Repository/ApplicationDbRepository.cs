using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Persistence.Repository;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Base.Domain.Entities.Common.Contracts;
using Mapster;
using Base.Infrastructure.Persistence.Context;

namespace Base.Infrastructure.Persistence.Repository;

public class ApplicationDbRepository<T>(ApplicationDbContext dbContext) : RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
        specification.Selector is not null
            ? base.ApplySpecification(specification)
            : ApplySpecification(specification, false)
                .ProjectToType<TResult>();
}
