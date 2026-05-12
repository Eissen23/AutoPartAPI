using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Models;
using Base.Domain.Entities.Products;

namespace Base.Application.Products.Models;

public class CreateProductRequest 
{
    public string PartNumber { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal UnitCost { get; set; }
    public decimal? RetailPrice { get; set; }
    public Guid CategoryId { get; set; }
}

public class SearchProductRequest : PaginationFilter, IRequest<PaginatedResponse<ProductDto>>
{
}

public class UpdateProductRequest 
{
    public string? PartNumber { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? UnitCost { get; set; }
    public decimal? RetailPrice { get; set; }
    public Guid? CategoryId { get; set; }
}
