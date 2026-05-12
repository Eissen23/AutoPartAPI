using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Categories.Models;
using Base.Application.Common.Models;
using Base.Application.Common.Specifications;
using Base.Domain.Entities.Categories;

namespace Base.Application.Categories.Specs;

public class GetAllCategories : Specification<Category, CategoryDto>
{
    public GetAllCategories()
    {
        Query.Select(category => new CategoryDto
        {
            Id = category.Id,
            CategoryCode = category.CategoryCode,
            Name = category.Name,
            Description = category.Description,
            Type = category.Type,
            DefaultMarkupPercentage = category.DefaultMarkupPercentage
        });
    }
}

public class CategoryPaginated(PaginationFilter filter) : PaginationSpecification<Category, CategoryDto>(filter)
{
}

public class GetCategoryById : Specification<Category, CategoryDto>
{
    public GetCategoryById(Guid categoryId)
    {
        Query.Where(category => category.Id == categoryId)
             .Select(category => new CategoryDto
             {
                 Id = category.Id,
                 CategoryCode = category.CategoryCode,
                 Name = category.Name,
                 Description = category.Description,
                 Type = category.Type,
                 DefaultMarkupPercentage = category.DefaultMarkupPercentage
             });
    }
}

