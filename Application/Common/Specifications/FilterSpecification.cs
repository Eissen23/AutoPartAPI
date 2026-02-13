using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Extension;
using Application.Common.Models;

namespace Application.Common.Specifications;

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