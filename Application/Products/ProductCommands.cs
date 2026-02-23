using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Domain.Entities.Products;

namespace Application.Products;

public class CreateProductRequest : IRequest<Guid>
{
    public string PartNumber { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal UnitCost { get; set; }
    public decimal? RetailPrice { get; set; }
    public Guid CategoryId { get; set; }
}

public class DeleteProductRequest(Guid id) : IRequest<Guid>
{
    public Guid Id { get; set; } = id;
}

public class GetProductByIdRequest(Guid id) : IRequest<ProductDto?>
{
    public Guid Id { get; set; } = id;
}

public class SearchProductRequest : PaginationFilter, IRequest<PaginatedResponse<ProductDto>>
{
}

public class UpdateProductRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? PartNumber { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? UnitCost { get; set; }
    public decimal? RetailPrice { get; set; }
    public Guid? CategoryId { get; set; }
}
