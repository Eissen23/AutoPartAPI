using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;

namespace Application.Identities.Departments;

public class DepartmentDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public Guid? ParentId { get; set; }
}

