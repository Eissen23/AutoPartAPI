using System;
using System.Collections.Generic;
using System.Text;

namespace Application.PartLocations.Handlers;

internal class CreatePartLocationHandler(
        IPartLocationService partLocationService
    ) : IRequestHandler<CreatePartLocationRequest, Guid>
{
    private readonly IPartLocationService _partLocationService = partLocationService;

    public async Task<Guid> Handle(CreatePartLocationRequest request, CancellationToken cancellationToken)
    {
        var result = await _partLocationService.CreateAsync(request, cancellationToken);

        return result;
    }
}
