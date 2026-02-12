using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identities.Departments;

public class DepartmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public Guid? ParentId { get; set; }
}

