using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;

namespace Application.Identities.JobPosistions.Models;

public class JobPositionDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
