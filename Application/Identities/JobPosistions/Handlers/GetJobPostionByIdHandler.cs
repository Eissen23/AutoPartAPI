using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identities.JobPosistions.Handlers;

public class GetJobPostionByIdHandler(
    IJobPositionService jobPositionService
    ) : IRequestHandler<GetJobPositionByIdRequest, JobPositionDto?>
{
    private readonly IJobPositionService _service = jobPositionService;

    public async Task<JobPositionDto?> Handle(GetJobPositionByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(request.Id, cancellationToken);
        return result;
    }
}
