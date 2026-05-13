using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Extension;
using Base.Application.Common.Models;
using Base.Application.PartLocations;
using Base.Application.PartLocations.Specs;
using Base.Application.Persistence.Repository;
using Base.Application.Warehouses.Models;
using Base.Application.Warehouses.Spec;
using Base.Domain.Entities.Warehouses;
using Shared.Common.Exceptions;

namespace Base.Application.Warehouses.Services;

public class WarehouseService(
        IRepositoryWithEvents<WarehouseLocation> eventRepos,
        IReadRepository<WarehouseLocation> readRepos,
        IReadRepository<PartLocation> partLocationReadRepos
    ) : IWarehouseService
{
    private readonly IRepositoryWithEvents<WarehouseLocation> _eventRepos = eventRepos;
    private readonly IReadRepository<WarehouseLocation> _readRepos = readRepos;
    private readonly IReadRepository<PartLocation> _partLocationReadRepos = partLocationReadRepos;

    public async Task<Guid> CreateAsync(CreateWarehouseLocationRequest request, CancellationToken ct)
    {
        var warehouseLocation = WarehouseLocation.Create(
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

    public async Task<WarehouseLocationDetailDto> GetByIdAsync(Guid departmentId, CancellationToken ct)
    {
        var warehouseLocation = await _readRepos.GetByIdAsync(departmentId, ct);
        _ = warehouseLocation ?? throw new NotFoundException($"Warehouse location with id {departmentId} not found.");

        var partLocations = await _partLocationReadRepos.ListAsync(
            new GetPartLocationsByWarehouseLocationId(departmentId),
            ct);

        var existingParts = partLocations
            .Select(pl => new ExistingPart
            {
                Id = pl.Part.Id,
                PartName = pl.Part.Name,
                PartNumber = pl.Part.PartNumber,
                Quantity = pl.QuantityAtLocation
            })
            .ToList();

        return new WarehouseLocationDetailDto
        {
            Id = warehouseLocation.Id,
            ZoneCode = warehouseLocation.ZoneCode,
            Aisle = warehouseLocation.Aisle,
            Shelf = warehouseLocation.Shelf,
            Bin = warehouseLocation.Bin,
            IsOverstocked = warehouseLocation.IsOverstocked,
            ExistingPart = existingParts
        };
    }

    public async Task<PaginatedResponse<WarehouseLocationDto>> SearchAsync(PaginationFilter filter, CancellationToken ct)
    {
        var spec = new WarehouseLocationPaginated(filter);
        var result = await _readRepos.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize, ct);

        return result;
    }

    public async Task<Guid> UpdateAsync(Guid id, UpdateWarehouseLocationRequest request, CancellationToken ct)
    {
        var warehouseLocation = await _readRepos.GetByIdAsync(id, ct);
        _ = warehouseLocation ?? throw new NotFoundException($"Warehouse location with id {id} not found.");

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
