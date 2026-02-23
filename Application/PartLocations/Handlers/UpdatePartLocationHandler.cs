using System;
using System.Collections.Generic;
using System.Text;

namespace Application.PartLocations.Handlers;

public class UpdatePartLocationHandler(
    IPartLocationService partLocationService
    ) : IRequestHandler<UpdatePartLocationRequest, Guid>
{
    private readonly IPartLocationService _partLocationService = partLocationService;

    public async Task<Guid> Handle(UpdatePartLocationRequest request, CancellationToken cancellationToken)
    {
        var result = await _partLocationService.UpdateAsync(request, cancellationToken);

        return result;
    }
}
