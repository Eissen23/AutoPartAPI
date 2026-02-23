using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Domain.Entities.Categories;

namespace Application.Categories;

public class CategoryDto : IDto
{
    public Guid Id { get; set; }
    public string CategoryCode { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public SystemType? Type { get; set; }
    public decimal DefaultMarkupPercentage { get; set; }
}
