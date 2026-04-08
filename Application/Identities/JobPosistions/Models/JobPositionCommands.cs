using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Domain.Entities.Identity;

namespace Application.Identities.JobPosistions.Models;

public class CreateJobPositionRequest 
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public AccessLevel AccessLevel { get; set; }
}

public class UpdateJobPositionRequest 
{
    public string? Title { get; set; } 
    public string? Description { get; set; }
    public decimal? Salary { get; set; }
    public AccessLevel? AccessLevel { get; set; }
}

public class SearchJobPositionsRequest : PaginationFilter
{
}   