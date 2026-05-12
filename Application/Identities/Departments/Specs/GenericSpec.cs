using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Models;
using Base.Application.Common.Specifications;
using Base.Application.Identities.Departments.Models;
using Base.Domain.Entities.Identity;

namespace Base.Application.Identities.Departments.Specs;

public class DepartmentByParentId : Specification<Department>
{
    public DepartmentByParentId(Guid parentId)
    {
        Query.Where(d => d.ParentId == parentId);
    }
}

public class GetAllDepartment : Specification<Department, DepartmentDto>
{
    public GetAllDepartment()
    {
        Query.Select(d => new DepartmentDto
        {
            Id = d.Id,
            Name = d.Name,
            Description = d.Description,
            ParentId = d.ParentId
        });
    }
}

public class DepartmentPaginated(PaginationFilter filter) : PaginationSpecification<Department, DepartmentDto>(filter)
{
}