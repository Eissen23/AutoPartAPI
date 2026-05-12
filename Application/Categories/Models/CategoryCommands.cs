using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Models;
using Base.Domain.Entities.Categories;

namespace Base.Application.Categories.Models;

public class CreateCategoryRequest
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public SystemType? Type { get; set; }
    public decimal? DefaultMarkupPercentage { get; set; }
}


public class SearchCategoryRequest : PaginationFilter
{
}

public class UpdateCategoryRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public SystemType? Type { get; set; }
    public decimal? DefaultMarkupPercentage { get; set; }
}
