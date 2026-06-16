# Copilot Instructions

## General Guidelines
- In update methods, only reassign properties if the new value is different from the current value to avoid unnecessary assignments.

## Code Style
- For strings, use: `if (property is not null && Property?.Equals(property) is not true) Property = property;`
- For value types, use: `if (value.HasValue && Property != value) Property = value.Value;`

## Authentication Configuration
- For JWT Bearer authentication configuration, use `IConfigureNamedOptions<JwtBearerOptions>` instead of `IConfigureOptions<JwtBearerOptions>`. This allows proper named configuration with scheme name validation.

## CRUD Service Creation Process for Entities

1. **Create Commands/Requests in `[EntityName]Commands.cs`:**
   - `CreateXxxRequest` (required properties)
   - `DeleteXxxRequest(Guid id)` — or use the id parameter directly in the controller
   - `GetXxxByIdRequest(Guid id)` — or use the id parameter directly
   - `SearchXxxRequest : PaginationFilter`
   - `UpdateXxxRequest` (nullable properties, includes `Id`)

2. **Create DTO in `[EntityName]Dto.cs`:**
   - Implement `IDto`
   - Flat properties matching the entity for list/search views
   - Separate detail DTO (e.g. `XxxDetailDto`) for single-item views when additional related data is fetched

3. **Create Service in `[EntityName]Service.cs`:**
   - Inherit `BaseService<TEntity, TDto>(IApplicationDbContext context)` — constructor injection via primary constructor
   - Implement `IXxxService`
   - Use `BaseService` protected helpers: `FindAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`, `ListAsync`, `PaginatedSearchAsync`, `GetAsync`
   - For `GetByIdAsync` that needs related data, use `Context.Set<T>().AsNoTracking()...` directly
   - Handle `NotFoundException` on delete/update

4. **Create Interface in `IXxxService.cs`:**
   - Define the service contract: `CreateAsync`, `DeleteAsync`, `GetByIdAsync`, `GetAllAsync`, `SearchAsync`, `UpdateAsync`

5. **Register the service** in `Base.Application/Startup.cs` or the appropriate module startup.

### Canonical example: `WarehouseService`

```csharp
public class WarehouseService(IApplicationDbContext context)
    : BaseService<WarehouseLocation, WarehouseLocationDto>(context), IWarehouseService
{
    public async Task<Guid> CreateAsync(CreateWarehouseLocationRequest request, CancellationToken ct)
    {
        var entity = WarehouseLocation.Create(...);
        await base.CreateAsync(entity, ct);
        return entity.Id;
    }

    public async Task<Guid> DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await FindAsync(id, ct);
        _ = entity ?? throw new NotFoundException($"... with id {id} not found.");
        await base.DeleteAsync(entity, ct);
        return entity.Id;
    }

    public Task<List<WarehouseLocationDto>> GetAllAsync(CancellationToken ct)
        => ListAsync(ct);

    public Task<PaginatedResponse<WarehouseLocationDto>> SearchAsync(PaginationFilter filter, CancellationToken ct)
        => PaginatedSearchAsync(filter, ct);

    public async Task<Guid> UpdateAsync(Guid id, UpdateWarehouseLocationRequest request, CancellationToken ct)
    {
        var entity = await FindAsync(id, ct);
        _ = entity ?? throw new NotFoundException($"... with id {id} not found.");
        entity.Update(...);
        await base.UpdateAsync(entity, ct);
        return entity.Id;
    }
}
```
