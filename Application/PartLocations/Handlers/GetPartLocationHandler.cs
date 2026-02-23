using System;
using System.Collections.Generic;
using System.Text;

namespace Application.PartLocations.Handlers;

public class GetPartLocationHandler(
        IPartLocationService partLocationService
    ) : IRequestHandler<GetPartLocationByIdRequest, PartLocationDto?>
{
    private readonly IPartLocationService _partLocationService = partLocationService;

    public async Task<PartLocationDto?> Handle(GetPartLocationByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _partLocationService.GetByIdAsync(request.Id, cancellationToken);

        return result;
    }
}
