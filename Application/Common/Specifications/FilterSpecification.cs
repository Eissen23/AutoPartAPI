using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Extension;
using Base.Application.Common.Models;

namespace Base.Application.Common.Specifications;

public class FilterSpecification<T, TResult> : Specification<T, TResult>
    where T : class
{
    public FilterSpecification(BaseFilter filters) =>
        Query
            .SearchBy(filters);
}

public class FilterSpecification<T> : Specification<T>
    where T : class
{
    public FilterSpecification(BaseFilter filters) =>
        Query
            .SearchBy(filters);
}