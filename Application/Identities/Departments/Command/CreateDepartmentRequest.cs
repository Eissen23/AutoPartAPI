using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identities.Departments.Command;

public class CreateDepartmentRequest() : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public Guid? DepartmentId { get; set; }
}
