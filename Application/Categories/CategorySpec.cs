using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Application.Common.Specifications;
using Domain.Entities.Categories;

namespace Application.Categories;

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