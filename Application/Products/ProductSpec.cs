using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Application.Common.Specifications;
using Domain.Entities.Products;

namespace Application.Products;

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
