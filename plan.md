# Proposed Change Plan (Response Envelope + Pagination Consistency)

## Assumed scope
Based on the discovery pass, this plan targets **consistency and correctness** of:
1. API response envelope behavior (`ApiResponse` / `ApiResult`)
2. Pagination response construction
3. Controller response status code alignment with declared `ProducesResponseType`

If this scope is not what you intended, I will adjust before implementation.

---

## Files to be CREATED

### None planned
I can complete this with additive/targeted edits to existing files.

---

## Files to be MODIFIED

## 1) Core response/pagination infrastructure

### `AutoPart/Base.Application/Common/Extension/PaginationResponseExtension.cs`
- **Reason:** Constructor arguments are currently passed in the wrong order.
- **Specific change:**
  - Change:
    - `new PaginatedResponse<TDestination>(list, count, pageNumber, pageSize)`
  - To:
    - `new PaginatedResponse<TDestination>(list, pageNumber, count, pageSize)`
- **Impact:** Fixes incorrect pagination metadata (`CurrentPage`, `TotalCount`) where this extension is used.

### `AutoPart/Base.WebApi/Extensions/ApiResult.cs`
- **Reason:** `ApiResult<T>.Success` defaults to HTTP 200 only; create endpoints declare 201.
- **Specific change:**
  - Keep existing methods.
  - Ensure helper path supports explicit status code cleanly for generic results (already partly present, will normalize usage).
  - Remove unused `using Microsoft.CodeAnalysis.CSharp.Syntax;` (cleanup).

### `AutoPart/Base.WebApi/Extensions/ControllerExtension.cs`
- **Reason:** Controllers need a first-class helper for `201 Created` to match annotations.
- **Specific change:**
  - Add `ApiCreated<T>(...)` extension that returns `ApiResult<T>` with `HttpStatusCode.Created`.
  - Keep existing `ApiOk` methods unchanged for compatibility.

### `AutoPart/Base.Infrastructure/Middlewares/ExceptionMiddleware.cs`
- **Reason:** `BaseApiException.ErrorCode` exists but is currently not surfaced to clients.
- **Specific change:**
  - Extend failure payload generation for `BaseApiException` path to include `ErrorCode` in `ApiResponse.Meta` (additive, non-breaking field).
  - Keep message/status behavior unchanged.

### `AutoPart/Shared/Common/ApiResponse.cs`
- **Reason:** Small additive helpers may be needed to keep status code inside envelope aligned with HTTP status for non-200 responses.
- **Specific change:**
  - Add additive factory overload(s) only if required by the extension changes (no breaking rename/removal).
  - Preserve current JSON shape (`isSuccess`, `message`, `data`, `statusCode`, `timestamp`, `meta`).

---

## 2) Controller status code alignment

### `AutoPart/Base.WebApi/Controllers/Categories/CategoriesController.cs`
- **Reason:** `CreateAsync` advertises 201 but returns 200 via `ApiOk`.
- **Specific change:** Use `ApiCreated` for create action.

### `AutoPart/Base.WebApi/Controllers/Customers/CustomersController.cs`
- **Reason:** Same create mismatch (201 declared, 200 returned).
- **Specific change:** Use `ApiCreated` in create action.

### `AutoPart/Base.WebApi/Controllers/Identity/DepartmentsController.cs`
- **Reason:**
  - Create mismatch (201 declared, 200 returned)
  - Search endpoint currently declares 201 for paginated read operation
- **Specific change:**
  - Use `ApiCreated` in create action
  - Change search `ProducesResponseType` from `Status201Created` to `Status200OK`

### `AutoPart/Base.WebApi/Controllers/Identity/JobPositionsController.cs`
- **Reason:** Same issues as Departments.
- **Specific change:**
  - Use `ApiCreated` in create action
  - Change search response annotation to `Status200OK`

### `AutoPart/Base.WebApi/Controllers/Identity/PermissionsController.cs`
- **Reason:** Same issues as Departments.
- **Specific change:**
  - Use `ApiCreated` in create action
  - Change search response annotation to `Status200OK`

### `AutoPart/Base.WebApi/Controllers/Identity/RolesController.cs`
- **Reason:** Same issues as Departments.
- **Specific change:**
  - Use `ApiCreated` in create action
  - Change search response annotation to `Status200OK`

### `AutoPart/Base.WebApi/Controllers/Identity/TokenController.cs`
- **Reason:** Declares 201 but currently returns 200.
- **Specific change:** Decide and enforce one behavior:
  - Option A: return `ApiCreated` (keep 201 annotation)
  - Option B: keep 200 runtime and change annotation to 200

### `AutoPart/Base.WebApi/Controllers/Identity/UserController.cs`
- **Reason:** Create endpoints declare 201 but return 200.
- **Specific change:** Use `ApiCreated` in user creation actions.

### `AutoPart/Base.WebApi/Controllers/Invoces/InvoiceItemsController.cs`
- **Reason:** Create mismatch (201 declared, 200 returned).
- **Specific change:** Use `ApiCreated` in create action.

### `AutoPart/Base.WebApi/Controllers/Invoces/InvoicesController.cs`
- **Reason:** Create mismatch (201 declared, 200 returned).
- **Specific change:** Use `ApiCreated` in create action.

### `AutoPart/Base.WebApi/Controllers/Products/ProductsController.cs`
- **Reason:** Create mismatch (201 declared, 200 returned).
- **Specific change:** Use `ApiCreated` in create action.

### `AutoPart/Base.WebApi/Controllers/Warehouses/PartLocationsController.cs`
- **Reason:** Create mismatch (201 declared, 200 returned).
- **Specific change:** Use `ApiCreated` in create action.

### `AutoPart/Base.WebApi/Controllers/Warehouses/WarehousesController.cs`
- **Reason:** Create mismatch (201 declared, 200 returned).
- **Specific change:** Use `ApiCreated` in create action.

---

## Files to be DELETED

### None planned
No deletions are required for this scope.

---

## Conflicts / Risks

1. **Behavioral change risk (HTTP status):**
   - If create endpoints move from 200 -> 201, clients/tests that assert 200 may fail.

2. **Token endpoint semantics:**
   - `TokenController.CreateToken` is not always treated as a resource creation endpoint in all APIs.
   - Needs explicit decision (keep 200 vs move to 201).

3. **Envelope shape compatibility:**
   - Existing clients may deserialize the current `ApiResponse` shape.
   - Any envelope additions must be additive only.

4. **Pagination extension bug fix side effects:**
   - If any consumer accidentally relied on the incorrect values, behavior will change to correct values.

5. **Naming confusion already present:**
   - File name `PaginationResponse.cs` contains class `PaginatedResponse<T>`.
   - No immediate break, but possible developer confusion.

6. **Potential double-wrapping (if a global response filter is later introduced):**
   - Current controllers already wrap with `ApiOk`/`ApiResult`.
   - A global wrapper would need explicit skip rules.

---

## Will any existing controller break after this change?

- **Compile-time break:** Not expected (changes are additive and usage updates are straightforward).
- **Runtime/contract break risk:**
  - Endpoints listed above that switch from 200 to 201 may break strict client expectations.
  - Search annotation corrections (201 -> 200) are documentation/Swagger alignment; runtime for those remains 200.

---

## Approval gate

I will **wait for your approval** before making any code changes.
