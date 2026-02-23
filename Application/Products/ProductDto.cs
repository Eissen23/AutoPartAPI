using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;

namespace Application.Products;

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
