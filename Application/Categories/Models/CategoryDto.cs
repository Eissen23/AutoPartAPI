using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;
using Base.Domain.Entities.Categories;

namespace Base.Application.Categories.Models;

public class CategoryDto : IDto
{
    public Guid Id { get; set; }
    public string CategoryCode { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public SystemType? Type { get; set; }
    public decimal DefaultMarkupPercentage { get; set; }
}

public class CategoryNameDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}