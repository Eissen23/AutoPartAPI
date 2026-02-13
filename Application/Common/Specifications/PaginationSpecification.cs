using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Extension;
using Application.Common.Models;

namespace Application.Common.Specifications;

public class PaginationSpecification<T, TResult> : FilterSpecification<T, TResult>
    where T : class
{
    public PaginationSpecification(PaginationFilter filters) : base(filters) =>
        Query
            .PaginateBy(filters);
}

public class PaginationSpecification<T> : FilterSpecification<T>
    where T : class
{
    public PaginationSpecification(PaginationFilter filters) : base(filters) =>
        Query
            .PaginateBy(filters);
}
