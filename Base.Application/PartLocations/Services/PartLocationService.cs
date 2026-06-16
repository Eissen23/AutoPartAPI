using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.Common.Services;
using Base.Application.PartLocations.Models;
using Base.Domain.Entities.Warehouses;
using Shared.Common.Exceptions;

namespace Base.Application.PartLocations.Services;

public class PartLocationService(IApplicationDbContext context)
    : BaseService<PartLocation, PartLocationDto>(context), IPartLocationService
{
    public async Task<Guid> CreateAsync(CreatePartLocationRequest request, CancellationToken ct = default)
    {
        var partLocation = PartLocation.Create(
            request.PartId,
            request.WarehouseLocationId,
            request.QuantityAtLocation);

        await base.CreateAsync(partLocation, ct);

        return partLocation.Id;
    }

    public async Task<Guid> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var partLocation = await FindAsync(id, ct);
        _ = partLocation ?? throw new NotFoundException($"Part location with id {id} not found.");

        await base.DeleteAsync(partLocation, ct);

        return partLocation.Id;
    }

    public Task<List<PartLocationDto>> GetAllAsync(CancellationToken ct = default)
        => ListAsync(ct);

    public async Task<PartLocationDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var partLocation = await FindAsync(id, ct);
        _ = partLocation ?? throw new NotFoundException($"Part location with id {id} not found.");

        return new PartLocationDto
        {
            Id = partLocation.Id,
            PartId = partLocation.PartId,
            WarehouseLocationId = partLocation.WarehouseLocationId,
            QuantityAtLocation = partLocation.QuantityAtLocation
        };
    }

    public Task<PaginatedResponse<PartLocationDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default)
        => PaginatedSearchAsync(filter, ct);

    public async Task<Guid> UpdateAsync(Guid id, UpdatePartLocationRequest request, CancellationToken ct = default)
    {
        var partLocation = await FindAsync(id, ct);
        _ = partLocation ?? throw new NotFoundException($"Part location with id {id} not found.");

        partLocation.Update(
            request.PartId,
            request.WarehouseLocationId,
            request.QuantityAtLocation);

        await base.UpdateAsync(partLocation, ct);

        return partLocation.Id;
    }
}
