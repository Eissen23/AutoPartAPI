using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identities.Departments.Command;

public class UpdateDepartmentRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid? DepartmentId { get; set; }
}
