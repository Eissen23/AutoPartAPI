using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Extension;
using Application.Common.Models;
using Application.Persistence.Repository;
using Domain.Entities.Warehouses;
using Shared.Common.Exceptions;

namespace Application.PartLocations;

public class PartLocationService(
        IRepositoryWithEvents<PartLocation> eventRepos,
        IReadRepository<PartLocation> readRepos
    ) : IPartLocationService
{
    private readonly IRepositoryWithEvents<PartLocation> _eventRepos = eventRepos;
    private readonly IReadRepository<PartLocation> _readRepos = readRepos;

    public async Task<Guid> CreateAsync(CreatePartLocationRequest request, CancellationToken ct)
    {
        var partLocation = new PartLocation()
            .Update(
                request.PartId,
                request.WarehouseLocationId,
                request.QuantityAtLocation
            );

        var result = await _eventRepos.AddAsync(partLocation, ct);

        return result.Id;
    }

    public async Task<Guid> DeleteAsync(Guid departmentId, CancellationToken ct)
    {
        var partLocation = await _readRepos.GetByIdAsync(departmentId, ct);

        _ = partLocation ?? throw new NotFoundException($"Part location with id {departmentId} not found.");

        await _eventRepos.DeleteAsync(partLocation, ct);

        return partLocation.Id;
    }

    public async Task<List<PartLocationDto>> GetAllAsync(CancellationToken ct)
    {
        var locations = await _readRepos.ListAsync(new GetAllPartLocations(), ct);

        return locations;
    }

    public async Task<PartLocationDto> GetByIdAsync(Guid departmentId, CancellationToken ct)
    {
        var partLocation = await _readRepos.GetByIdAsync(departmentId, ct);
        _ = partLocation ?? throw new NotFoundException($"Part location with id {departmentId} not found.");

        return new PartLocationDto
        {
            Id = partLocation.Id,
            PartId = partLocation.PartId,
            WarehouseLocationId = partLocation.WarehouseLocationId,
            QuantityAtLocation = partLocation.QuantityAtLocation
        };
    }

    public async Task<PaginatedResponse<PartLocationDto>> SearchAsync(PaginationFilter filter, CancellationToken ct)
    {
        var spec = new PartLocationPaginated(filter);
        var result = await _readRepos.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize, ct);

        return result;
    }

    public async Task<Guid> UpdateAsync(UpdatePartLocationRequest request, CancellationToken ct)
    {
        var partLocation = await _readRepos.GetByIdAsync(request.Id, ct);
        _ = partLocation ?? throw new NotFoundException($"Part location with id {request.Id} not found.");

        partLocation.Update(
            request.PartId,
            request.WarehouseLocationId,
            request.QuantityAtLocation
        );

        await _eventRepos.UpdateAsync(partLocation, ct);

        return partLocation.Id;
    }
}
