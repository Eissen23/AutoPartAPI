using System;
using System.Collections.Generic;
using System.Text;

namespace Application.PartLocations.Handlers;

public class DeletePartLocationHandler(
        IPartLocationService partLocationService
    ) : IRequestHandler<DeletePartLocationRequest, Guid>
{
    private readonly IPartLocationService _partLocationService = partLocationService;

    public async Task<Guid> Handle(DeletePartLocationRequest request, CancellationToken cancellationToken)
    {
        var result = await _partLocationService.DeleteAsync(request.Id, cancellationToken);

        return result;
    }
}
