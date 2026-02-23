using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Domain.Entities.Categories;

namespace Application.Categories;

public class CreateCategoryRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public SystemType? Type { get; set; }
    public decimal? DefaultMarkupPercentage { get; set; }
}

public class DeleteCategoryRequest(Guid id) : IRequest<Guid>
{
    public Guid Id { get; set; } = id;
}

public class GetCategoryByIdRequest(Guid id) : IRequest<CategoryDto?>
{
    public Guid Id { get; set; } = id;
}

public class SearchCategoryRequest : PaginationFilter, IRequest<PaginatedResponse<CategoryDto>>
{
}

public class UpdateCategoryRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public SystemType? Type { get; set; }
    public decimal? DefaultMarkupPercentage { get; set; }
}

