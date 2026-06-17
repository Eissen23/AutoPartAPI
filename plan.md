Plan to implement RoleService and PermissionService (Application layer) and their controllers (WebApi) — following the JobPosistions pattern and project conventions.

Summary
- Mirror the JobPosistions folder layout: Models (Commands/DTOs), Services (interface + implementation), Validations, Specs.
- Implement PermissionService as a standard CRUD + search (same shape as JobPositionService).
- Implement RoleService as CRUD + permission assignment endpoints. Internals will operate on ApplicationRole, Permission and RolePermission sets via IApplicationDbContext (use same patterns as JobPositionService).
- Create two WebApi controllers under `Base.WebApi/Controllers/Identity/` that mirror JobPositionsController style, responses, attributes, and authorization patterns.
- Follow project rules: update methods should only assign when values differ; use NotFoundException for missing resources; use PaginatedSearchAsync for search; use the ApiOk helpers in controllers; use CancellationToken defaults.

Files to add (paths and purpose)
- Application layer (follow JobPosistions style)
  - `Base.Application/Identities/Permissions/Models/PermissionCommands.cs`
    - CreatePermissionRequest, UpdatePermissionRequest, SearchPermissionsRequest : PaginationFilter
  - `Base.Application/Identities/Permissions/Models/PermissionDto.cs`
    - PermissionDto : IDto (Id, Name, Description)
  - `Base.Application/Identities/Permissions/Services/IPermissionService.cs`
    - Interface like IJobPositionService (SearchAsync, GetAllAsync, GetByIdAsync, CreateAsync, UpdateAsync, DeleteAsync)
  - `Base.Application/Identities/Permissions/Services/PermissionService.cs`
    - Implementation: inherit BaseService<Permission, PermissionDto> (use IApplicationDbContext Set<Permission>() entity type)
  - `Base.Application/Identities/Permissions/Validations/PermissionValidation.cs`
    - Fluent validation rules for name required, length, etc.
  - `Base.Application/Identities/Permissions/Specs/GenericSpecs.cs` (optional: mirror JobPosistions specs)

  - `Base.Application/Identities/Roles/Models/RoleCommands.cs`
    - CreateRoleRequest (Name, Description, AccessLevel?, IsSystemRole?), UpdateRoleRequest (nullable properties), AssignPermissionsToRoleRequest (roleId, List<Guid> permissionIds), SearchRolesRequest : PaginationFilter
  - `Base.Application/Identities/Roles/Models/RoleDto.cs`
    - RoleDto : IDto (Id, Name, Description, AccessLevel, IsSystemRole, List<PermissionDto> or List<Guid>/List<string>)
  - `Base.Application/Identities/Roles/Services/IRoleService.cs`
    - CRUD + Search + Assign/Remove/GetRolePermissions signatures
  - `Base.Application/Identities/Roles/Services/RoleService.cs`
    - Implementation: follow JobPositionService pattern. Use context.Set<ApplicationRole>(), context.Set<Permission>(), context.Set<RolePermission>().
  - `Base.Application/Identities/Roles/Validations/RoleValidation.cs`
  - `Base.Application/Identities/Roles/Specs/GenericSpecs.cs` (optional)

- WebApi layer (controllers)
  - `Base.WebApi/Controllers/Identity/PermissionsController.cs`
    - Endpoints: Create, Search, Update, Delete, GetById
    - Mirror JobPositionsController attributes, return types (ApiResponse<T>), Authorization
  - `Base.WebApi/Controllers/Identity/RolesController.cs`
    - Endpoints:
      - Create (CreateRoleRequest)
      - Search (SearchRolesRequest)
      - Update (PUT {id})
      - Delete (DELETE {id})
      - GetById (GET {id})
      - AssignPermissions (POST {id}/permissions) — accepts AssignPermissionsToRoleRequest (list of permission ids)
      - RemovePermissions (DELETE {id}/permissions) — accepts list or single id (choose consistent payload)
      - GetRolePermissions (GET {id}/permissions)
    - Use [Authorize] and apply RequiredPermissionAttribute on management endpoints (optional; follow project policy).
    - Mirror response patterns (ApiOk, messages).

Service design and method signatures (recommended)

Permission service interface (IPermissionService)
- Task<PaginatedResponse<PermissionDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default);
- Task<List<PermissionDto>> GetAllAsync(CancellationToken ct = default);
- Task<PermissionDto> GetByIdAsync(Guid permissionId, CancellationToken ct = default);
- Task<Guid> CreateAsync(CreatePermissionRequest request, CancellationToken ct = default);
- Task<Guid> UpdateAsync(Guid id, UpdatePermissionRequest request, CancellationToken ct = default);
- Task<Guid> DeleteAsync(Guid permissionId, CancellationToken ct = default);

PermissionService implementation (pattern)
- CreateAsync: build Permission entity, call base.CreateAsync(entity, ct), return entity.Id.
- GetByIdAsync: FindAsync(id) -> throw NotFoundException if null -> map to PermissionDto.
- SearchAsync: -> PaginatedSearchAsync(filter, ct).
- UpdateAsync: FindAsync, throw if null, call entity.Update(...) but if entity is a simple EF type, update properties manually following project update pattern (assign if changed). Then call base.UpdateAsync(entity, ct).
- DeleteAsync: FindAsync, throw if null, base.DeleteAsync(entity, ct).

Role service interface (IRoleService)
- Task<PaginatedResponse<RoleDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default);
- Task<List<RoleDto>> GetAllAsync(CancellationToken ct = default);
- Task<RoleDto> GetByIdAsync(Guid roleId, CancellationToken ct = default);
- Task<Guid> CreateAsync(CreateRoleRequest request, CancellationToken ct = default);
- Task<Guid> UpdateAsync(Guid id, UpdateRoleRequest request, CancellationToken ct = default);
- Task<Guid> DeleteAsync(Guid roleId, CancellationToken ct = default);
- Task AssignPermissionsAsync(Guid roleId, IEnumerable<Guid> permissionIds, CancellationToken ct = default);
- Task RemovePermissionsAsync(Guid roleId, IEnumerable<Guid> permissionIds, CancellationToken ct = default);
- Task<List<PermissionDto>> GetRolePermissionsAsync(Guid roleId, CancellationToken ct = default);

RoleService implementation (pattern)
- For CreateAsync:
  - Create ApplicationRole instance (new ApplicationRole { Id = Guid.NewGuid(), Name = request.Name, Description = request.Description, AccessLevel = request.AccessLevel, IsSystemRole = request.IsSystemRole })
  - await base.CreateAsync(role, ct);
  - Optionally if request contains permission ids, create RolePermission entries (RolePermission { RoleId = role.Id, PermissionId = pid }) and add to context.Set<RolePermission>().AddRangeAsync(...), then SaveChangesAsync.
- For GetByIdAsync:
  - var role = await context.Set<ApplicationRole>().FindAsync(new object[] { roleId }, ct); throw NotFound if null
  - Query role permissions: join RolePermissions & Permissions to get PermissionDto list
  - Map and return RoleDto (include permission list)
- For UpdateAsync:
  - Find role; throw if null
  - Follow project update pattern: if (request.Description is not null && !role.Description?.Equals(request.Description) is true) role.Description = request.Description; etc.
  - base.UpdateAsync(role, ct)
- For DeleteAsync:
  - Find role; throw if null; base.DeleteAsync(role, ct). Also delete RolePermissions entries referencing role (or rely on cascade configured in EF).
- For AssignPermissionsAsync / RemovePermissionsAsync:
  - Validate role exists; validate permission ids exist
  - For assignment: skip ones that already exist, add new RolePermission entities and save
  - For removal: remove matching RolePermission rows and save
  - After both, return void or updated role id

Important implementation notes / constraints
- Use IApplicationDbContext.Set<T>() to access Permission and RolePermission sets (as with other services).
- If Application layer needs ApplicationRole type, reference `Base.Infrastructure.Identities.ApplicationRole` in the service file. If layering forbids direct reference, consider mapping role operations to an injected IRoleManager wrapper. (Confirm project references; if RoleManager is available you can prefer RoleManager<ApplicationRole>.)
- Always throw NotFoundException when entity not found (JobPosition pattern).
- Update methods must only assign when new value differs (project rule: strings: `if (property is not null && Property?.Equals(property) is not true) Property = property;` and for value types `if (value.HasValue && Property != value) Property = value.Value;`).
- Enforce uniqueness of Permission.Name at validation level to surface earlier (EF already has unique index).
- For Search methods use PaginatedSearchAsync(filter, ct) provided by BaseService<T,Dto> where possible.
- For mapping entities -> DTOs, follow the mapping style used in JobPositionService (populate fields manually).

Controller endpoints and behavior
- PermissionsController (mirror JobPositionsController):
  - POST /permissions -> CreateAsync(CreatePermissionRequest): returns ApiResponse<Guid> (201)
  - POST /permissions/search -> SearchAsync(SearchPermissionsRequest): returns ApiResponse<PaginatedResponse<PermissionDto>>
  - PUT /permissions/{id} -> UpdateAsync(Guid id, UpdatePermissionRequest)
  - DELETE /permissions/{id} -> DeleteAsync(Guid id)
  - GET /permissions/{id} -> GetByIdAsync(Guid id)
  - Use [Authorize] attribute on endpoints
  - Use the same response messages and use this.ApiOk(...) helper (as JobPositionsController does)

- RolesController:
  - POST /roles -> CreateAsync(CreateRoleRequest)
  - POST /roles/search -> SearchAsync(SearchRolesRequest)
  - PUT /roles/{id} -> UpdateAsync(Guid id, UpdateRoleRequest)
  - DELETE /roles/{id} -> DeleteAsync(Guid id)
  - GET /roles/{id} -> GetByIdAsync(Guid id) (should include assigned permissions in response)
  - POST /roles/{id}/permissions -> AssignPermissionsAsync (accept body AssignPermissionsToRoleRequest with list of permission ids)
  - DELETE /roles/{id}/permissions -> RemovePermissionsAsync (accept body with permission ids)
  - GET /roles/{id}/permissions -> GetRolePermissionsAsync returns List<PermissionDto>
  - Use [Authorize] and consider RequiredPermissionAttribute("RoleManagement") on role management endpoints

Validation & Specs
- Add basic validation classes (PermissionValidation, RoleValidation) to check required fields, length, and semantic checks (e.g., cannot remove permission from system role if business rule exists).
- Optionally add GenericSpecs used by JobPosistions for repository filters if needed.

Example skeleton implementations (concise pseudocode)

PermissionService.CreateAsync
- var permission = new Permission { Name = request.Name, Description = request.Description };
- await base.CreateAsync(permission, ct);
- return permission.Id;

RoleService.AssignPermissionsAsync
- var role = await context.Set<ApplicationRole>().FindAsync(roleId); if null throw NotFoundException
- var existingPermissionIds = await context.Set<RolePermission>().Where(rp => rp.RoleId == roleId).Select(rp => rp.PermissionId).ToListAsync()
- var toAdd = permissionIds.Except(existingPermissionIds).ToList()
- create RolePermission entries for toAdd and AddRangeAsync
- await context.SaveChangesAsync(ct)

Controller method (RolesController.AssignPermissions)
- [Authorize]
- [HttpPost("{id:guid}/permissions")]
- public async Task<IActionResult> AssignPermissionsAsync([FromRoute] Guid id, [FromBody] AssignPermissionsToRoleRequest req, CancellationToken ct)
  - await _roleService.AssignPermissionsAsync(id, req.PermissionIds, ct)
  - return this.ApiOk(id, "Assigned permissions to role successfully.");

Testing and validation plan
- Unit tests for both services:
  - Create, Update, Delete, GetById for Permission
  - Create role; assign permissions; verify RolePermissions entries created; remove permissions
  - Edge cases: assign duplicate permission ids (should be idempotent), assign non-existent permission id -> throw or ignore (decide policy)
- Integration test hooking up ApplicationDbContext in-memory for Db access.
- Manual: run seeder to confirm permissions persistence; verify TokenService behavior reading RolePermissions still works.

Migration / EF considerations
- No new migrations if using existing Permission and RolePermission entities. If you add any fields to Infrastructure entities, create/apply EF migration.
- Ensure Role deletion cascades RolePermissions (existing RolePermissionConfiguration sets cascade on delete — review it to confirm).

Estimated implementation steps and time (rough)
1. Create Models and DTOs for Permissions and Roles (0.5-1h)
2. Implement IPermissionService & PermissionService and corresponding validations (1-2h)
3. Implement IRoleService & RoleService (2-3h; permission-assignment logic requires care)
4. Implement PermissionsController & RolesController using the same pattern as JobPositionsController (0.5-1h)
5. Add unit/integration tests (1-3h depending on coverage)
6. Manual verification / small fixes (0.5-1h)
