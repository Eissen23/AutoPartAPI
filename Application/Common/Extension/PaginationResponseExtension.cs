using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Application.Common.Models;
using Application.Persistence.Repository;

namespace Application.Common.Extension;

public static class PaginationResponseExtension
{
    extension<T>(IReadRepositoryBase<T> repository) where T : class
    {
        public async Task<PaginatedResponse<TDestination>> PaginatedListAsync<TDestination>(
            ISpecification<T, TDestination> spec,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
       where TDestination : class, IDto
        {
            var list = await repository.ListAsync(spec, cancellationToken);
            int count = await repository.CountAsync(spec, cancellationToken);

            return new PaginatedResponse<TDestination>(list, count, pageNumber, pageSize);
        }
    }
}
