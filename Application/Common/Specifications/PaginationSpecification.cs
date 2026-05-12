using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Extension;
using Base.Application.Common.Models;

namespace Base.Application.Common.Specifications;

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
