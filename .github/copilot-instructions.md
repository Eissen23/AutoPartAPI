# Copilot Instructions

## General Guidelines
- In update methods, only reassign properties if the new value is different from the current value to avoid unnecessary assignments.

## Code Style
- For strings, use: `if (property is not null && Property?.Equals(property) is not true) Property = property;`
- For value types, use: `if (value.HasValue && Property != value) Property = value.Value;`

## Authentication Configuration
- For JWT Bearer authentication configuration, use `IConfigureNamedOptions<JwtBearerOptions>` instead of `IConfigureOptions<JwtBearerOptions>`. This allows proper named configuration with scheme name validation.

## CRUD Handler Creation Process for Entities
1. **Create Commands/Requests in [EntityName]Commands.cs:**
   - CreateXxxRequest (required properties)
   - DeleteXxxRequest(Guid id)
   - GetXxxByIdRequest(Guid id)
   - SearchXxxRequest : PaginationFilter
   - UpdateXxxRequest (nullable properties, includes Id)

2. **Create Service in [EntityName]Service.cs:**
   - Constructor with `IRepositoryWithEvents<Entity>` and `IReadRepository<Entity>` injection
   - Implement `CreateAsync`, `DeleteAsync`, `GetByIdAsync`, `GetAllAsync`, `SearchAsync`, `UpdateAsync`
   - Create entity using `Update()` method, handle `NotFoundException` on deletes/updates
   - Map entities to DTOs in `GetByIdAsync` and `GetAllAsync`

3. **Create Specifications in [EntityName]Spec.cs:**
   - `GetAll[Entity]s` : `Specification<Entity, DTO>` with LINQ Select mapping
   - `[Entity]Paginated(PaginationFilter)` : `PaginationSpecification<Entity, DTO>`

4. **Create 5 Handlers in Handlers/ folder, each:**
   - Constructor: inject `IXxxService`
   - Implement `IRequestHandler<XxxRequest, TResponse>`
   - `Handle()` method: call service method and return result
   - Files: `Create[Entity]Handler`, `Delete[Entity]Handler`, `Get[Entity]Handler`, `Search[Entity]Handler`, `Update[Entity]Handler`

- Follow the exact pattern from `Application.Warehouses.Handlers` and `Application.PartLocations.Handlers` for consistency.