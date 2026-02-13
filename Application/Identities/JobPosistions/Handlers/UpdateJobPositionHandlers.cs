using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identities.JobPosistions.Handlers;

internal class UpdateJobPositionHandlers(
        IJobPositionService jobPositionService
    ) : IRequestHandler<UpdateJobPositionRequest, Guid>
{
    private readonly IJobPositionService _service = jobPositionService;

    public async Task<Guid> Handle(UpdateJobPositionRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(request, cancellationToken);

        return result;
    }
}
