using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Domain.Entities.Identity;

namespace Application.Identities.JobPosistions;

public class CreateJobPositionRequest : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public AccessLevel AccessLevel { get; set; }
}

public class UpdateJobPositionRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? Title { get; set; } 
    public string? Description { get; set; }
    public decimal? Salary { get; set; }
    public AccessLevel? AccessLevel { get; set; }
}

public class DeleteJobPositionRequest(Guid id) : IRequest<Guid>
{
    public Guid Id { get; set; } = id; 
}

public class GetJobPositionByIdRequest(Guid id) : IRequest<JobPositionDto?>
{
    public Guid Id { get; set; } = id;
}

public class SearchJobPositionsRequest : PaginationFilter, IRequest<PaginatedResponse<JobPositionDto>>
{
}   