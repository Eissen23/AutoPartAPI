using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Extension;
using Application.Common.Models;
using Application.Persistence.Repository;
using Domain.Entities.Warehouses;
using Shared.Common.Exceptions;

namespace Application.Warehouses;

public class WarehouseService(
        IRepositoryWithEvents<WarehouseLocation> eventRepos,
        IReadRepository<WarehouseLocation> readRepos
    ) : IWarehouseService
{
    private readonly IRepositoryWithEvents<WarehouseLocation> _eventRepos = eventRepos;
    private readonly IReadRepository<WarehouseLocation> _readRepos = readRepos;

    public async Task<Guid> CreateAsync(CreateWarehouseLocationRequest request, CancellationToken ct)
    {
        var warehouseLocation = new WarehouseLocation()
            .Update(
                request.ZoneCode,
                request.Aisle,
                request.Shelf,
                request.Bin,
                request.IsOverstocked
            );

        var result = await _eventRepos.AddAsync(warehouseLocation, ct);

        return result.Id;
    }

    public async Task<Guid> DeleteAsync(Guid departmentId, CancellationToken ct)
    {
        var warehouseLocation = await _readRepos.GetByIdAsync(departmentId, ct);

        _ = warehouseLocation ?? throw new NotFoundException($"Warehouse location with id {departmentId} not found.");

        await _eventRepos.DeleteAsync(warehouseLocation, ct);

        return warehouseLocation.Id;
    }

    public async Task<List<WarehouseLocationDto>> GetAllAsync(CancellationToken ct)
    {
        var locations = await _readRepos.ListAsync(new GetAllWarehouseLocations(), ct);

        return locations;
    }

    public async Task<WarehouseLocationDto> GetByIdAsync(Guid departmentId, CancellationToken ct)
    {
        var warehouseLocation = await _readRepos.GetByIdAsync(departmentId, ct);
        _ = warehouseLocation ?? throw new NotFoundException($"Warehouse location with id {departmentId} not found.");

        return new WarehouseLocationDto
        {
            Id = warehouseLocation.Id,
            ZoneCode = warehouseLocation.ZoneCode,
            Aisle = warehouseLocation.Aisle,
            Shelf = warehouseLocation.Shelf,
            Bin = warehouseLocation.Bin,
            IsOverstocked = warehouseLocation.IsOverstocked
        };
    }

    public async Task<PaginatedResponse<WarehouseLocationDto>> SearchAsync(PaginationFilter filter, CancellationToken ct)
    {
        var spec = new WarehouseLocationPaginated(filter);
        var result = await _readRepos.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize, ct);

        return result;
    }

    public async Task<Guid> UpdateAsync(UpdateWarehouseLocationRequest request, CancellationToken ct)
    {
        var warehouseLocation = await _readRepos.GetByIdAsync(request.Id, ct);
        _ = warehouseLocation ?? throw new NotFoundException($"Warehouse location with id {request.Id} not found.");

        warehouseLocation.Update(
            request.ZoneCode,
            request.Aisle,
            request.Shelf,
            request.Bin,
            request.IsOverstocked
        );

        await _eventRepos.UpdateAsync(warehouseLocation, ct);

        return warehouseLocation.Id;
    }
}
