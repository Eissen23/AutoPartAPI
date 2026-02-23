using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.PartLocations.Handlers;

public class SearchPartLocationHandler(
        IPartLocationService partLocationService
    ) : IRequestHandler<SearchPartLocationRequest, PaginatedResponse<PartLocationDto>>
{
    private readonly IPartLocationService _partLocationService = partLocationService;

    public async Task<PaginatedResponse<PartLocationDto>> Handle(SearchPartLocationRequest request, CancellationToken cancellationToken)
    {
        var result = await _partLocationService.SearchAsync(request, cancellationToken);

        return result;
    }
}
