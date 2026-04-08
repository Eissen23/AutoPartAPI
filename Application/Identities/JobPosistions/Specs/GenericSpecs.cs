using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Application.Common.Specifications;
using Application.Identities.JobPosistions.Models;
using Domain.Entities.Identity;

namespace Application.Identities.JobPosistions;


public class JobPositionPaginated(PaginationFilter filters) : PaginationSpecification<JobPosition, JobPositionDto>(filters)
{
}