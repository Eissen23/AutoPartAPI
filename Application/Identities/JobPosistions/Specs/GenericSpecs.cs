using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Models;
using Base.Application.Common.Specifications;
using Base.Application.Identities.JobPosistions.Models;
using Base.Domain.Entities.Identity;

namespace Base.Application.Identities.JobPosistions;


public class JobPositionPaginated(PaginationFilter filters) : PaginationSpecification<JobPosition, JobPositionDto>(filters)
{
}