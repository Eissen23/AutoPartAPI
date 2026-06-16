using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.Common.Services;
using Base.Application.Warehouses.Models;
using Base.Domain.Entities.Warehouses;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;

namespace Base.Application.Warehouses.Services;

public class WarehouseService(IApplicationDbContext context)
    : BaseService<WarehouseLocation, WarehouseLocationDto>(context), IWarehouseService
{
    public async Task<Guid> CreateAsync(CreateWarehouseLocationRequest request, CancellationToken ct)
    {
        var warehouseLocation = WarehouseLocation.Create(
            request.ZoneCode,
            request.Aisle,
            request.Shelf,
            request.Bin,
            request.IsOverstocked);

        await base.CreateAsync(warehouseLocation, ct);

        return warehouseLocation.Id;
    }

    public async Task<Guid> DeleteAsync(Guid departmentId, CancellationToken ct)
    {
        var warehouseLocation = await FindAsync(departmentId, ct);
        _ = warehouseLocation ?? throw new NotFoundException($"Warehouse location with id {departmentId} not found.");

        await base.DeleteAsync(warehouseLocation, ct);

        return warehouseLocation.Id;
    }

    public Task<List<WarehouseLocationDto>> GetAllAsync(CancellationToken ct)
        => ListAsync(ct);

    public async Task<WarehouseLocationDetailDto> GetByIdAsync(Guid departmentId, CancellationToken ct)
    {
        var warehouseLocation = await FindAsync(departmentId, ct);
        _ = warehouseLocation ?? throw new NotFoundException($"Warehouse location with id {departmentId} not found.");

        var existingParts = await Context.Set<PartLocation>()
            .AsNoTracking()
            .Where(pl => pl.WarehouseLocationId == departmentId)
            .Select(pl => new ExistingPart
            {
                Id = pl.Part.Id,
                PartName = pl.Part.Name,
                PartNumber = pl.Part.PartNumber,
                Quantity = pl.QuantityAtLocation
            })
            .ToListAsync(ct);

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

    public Task<PaginatedResponse<WarehouseLocationDto>> SearchAsync(PaginationFilter filter, CancellationToken ct)
        => PaginatedSearchAsync(filter, ct);

    public async Task<Guid> UpdateAsync(Guid id, UpdateWarehouseLocationRequest request, CancellationToken ct)
    {
        var warehouseLocation = await FindAsync(id, ct);
        _ = warehouseLocation ?? throw new NotFoundException($"Warehouse location with id {id} not found.");

        warehouseLocation.Update(
            request.ZoneCode,
            request.Aisle,
            request.Shelf,
            request.Bin,
            request.IsOverstocked);

        await base.UpdateAsync(warehouseLocation, ct);

        return warehouseLocation.Id;
    }
}
