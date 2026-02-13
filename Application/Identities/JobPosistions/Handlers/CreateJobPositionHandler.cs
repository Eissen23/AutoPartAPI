using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identities.JobPosistions.Handlers;

public class CreateJobPositionHandler(
        IJobPositionService jobPositionService
    ) : IRequestHandler<CreateJobPositionRequest, Guid>
{
    private readonly IJobPositionService _service = jobPositionService;

    public async Task<Guid> Handle(CreateJobPositionRequest request, CancellationToken cancellationToken)
    {
        var  result  = await _service.CreateAsync( request, cancellationToken );

        return result;
    }
}
