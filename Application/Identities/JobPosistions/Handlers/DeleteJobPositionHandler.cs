using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identities.JobPosistions.Handlers;

public class DeleteJobPositionHandler(
        IJobPositionService jobPositionService
    ) : IRequestHandler<DeleteJobPositionRequest, Guid>
{
    private readonly IJobPositionService _service = jobPositionService;

    public async Task<Guid> Handle(DeleteJobPositionRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(request.Id, cancellationToken);

        return result;
    }
}
