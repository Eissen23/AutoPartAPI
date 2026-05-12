using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Models;
using Base.Application.Common.Specifications;
using Base.Application.Products.Models;
using Base.Domain.Entities.Products;

namespace Base.Application.Products.Specs;

public class GetAllProducts : Specification<Product, ProductDto>
{
    public GetAllProducts()
    {
        Query.Select(p => new ProductDto
        {
            Id = p.Id,
            PartNumber = p.PartNumber,
            Name = p.Name,
            Description = p.Description,
            UnitCost = p.UnitCost,
            RetailPrice = p.RetailPrice,
            CategoryId = p.CategoryId
        });
    }
}

public class ProductPaginated(PaginationFilter filter) : PaginationSpecification<Product, ProductDto>(filter)
{
}
