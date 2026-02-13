using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.Common.Extension;

public static class PaginationSpecificationExtension
{
    extension<T>(ISpecificationBuilder<T> query) where T : class
    {
        public ISpecificationBuilder<T> PaginateBy(
                PaginationFilter paginationFilter
            )
        {
            if ( paginationFilter.PageNumber <= 0 )
            {
                paginationFilter.PageNumber = 1;
            }

            if (paginationFilter.PageSize <= 0)
            {
                paginationFilter.PageSize = 10;
            }

            if (paginationFilter.PageNumber > 1)
            {
                query = query.Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize);
            }

            return
                query
                .Take(paginationFilter.PageSize);
        }
    }
}
