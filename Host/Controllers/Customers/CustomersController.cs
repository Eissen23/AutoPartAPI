using Application.Common.Models;
using Application.Customers.Models;
using Application.Customers.Services;
using Application.Identities.Departments;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Customers;

public class CustomersController(
        ICustomerService customerService
    ) : VersionedApiController
{
    private readonly ICustomerService _customerService = customerService;

    [Authorize, HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(CreateCustomerRequest request, CancellationToken ct)
    {
        var result = await _customerService.CreateAsync(request, ct);
        return this.ApiOk(result, "Create Customer Success");
    }

    [Authorize, HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<CustomerDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(SearchCustomerRequest request, CancellationToken ct)
    {
        var result = await _customerService.SearchAsync(request, ct);
        return this.ApiOk(result, "Search Customer Success");
    }

    [Authorize, HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateCustomerRequest request,  CancellationToken ct)
    {
        var result = await _customerService.UpdateAsync(id, request, ct);
        return this.ApiOk(result, "Update Customer success.");
    }

    [Authorize, HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _customerService.DeleteAsync(id, ct);
        return this.ApiOk(result, "Delete Customer success.");
    }

    [Authorize, HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _customerService.GetByIdAsync(id, ct);
        return this.ApiOk(result, "Get Customer success.");
    }
}
