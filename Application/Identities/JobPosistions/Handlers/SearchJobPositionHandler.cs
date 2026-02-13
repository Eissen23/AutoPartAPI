using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.Identities.JobPosistions.Handlers;

internal class SearchJobPositionHandler (
        IJobPositionService jobPositionService
    ): IRequestHandler<SearchJobPositionsRequest, PaginatedResponse<JobPositionDto>>
{
    private readonly IJobPositionService _jobPositionService = jobPositionService;

    public Task<PaginatedResponse<JobPositionDto>> Handle(SearchJobPositionsRequest request, CancellationToken cancellationToken)
    {
        var result = _jobPositionService.SearchAsync(request, cancellationToken);

        return result;
    }
}
