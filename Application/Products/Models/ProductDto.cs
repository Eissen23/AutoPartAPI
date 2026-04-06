using System;
using System.Collections.Generic;
using System.Text;
using Application.Categories.Models;
using Application.Common.Interface;
using Application.Warehouses;
using Domain.Entities.Categories;

namespace Application.Products.Models;

public class ProductDto : IDto
{
    public Guid Id { get; set; }
    public string PartNumber { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal UnitCost { get; set; }
    public decimal? RetailPrice { get; set; }
    public Guid CategoryId { get; set; }
}

public class ProductDetailDto
{
    public Guid Id { get; set; }
    public string PartNumber { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal UnitCost { get; set; }
    public decimal? RetailPrice { get; set; }
    public CategoryDto? Category { get; set; }
    public IEnumerable<WarehouseStockDto>? WarehouseStocks { get; set; }
}

public class WarehouseStockDto
{
    public Guid Id { get; set; }
    public string ZoneCode { get; set; } = default!;
    public int Aisle { get; set; }
    public int Shelf { get; set; }
    public string? Bin { get; set; }
    public int Quantity { get; set; }
}