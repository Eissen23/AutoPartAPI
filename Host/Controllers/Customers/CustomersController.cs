using Application.Common.Models;
using Application.Customers;
using Application.Identities.Departments;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Customers;

public class CustomersController : VersionedApiController   
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(CreateCustomerRequest request)
    {
        var result = await Mediator.Send(request);
        return this.ApiOk(result, "Create Customer Success");
    }

    [Authorize]
    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<CustomerDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(SearchCustomerRequest request)
    {
        var result = await Mediator.Send(request);
        return this.ApiOk(result, "Search Customer Success");
    }

    [Authorize]
    [HttpPut("{id: Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateCustomerRequest request, [FromRoute] Guid id, CancellationToken ct)
    {
        if (request.Id != id)
        {
            throw new Shared.Common.Exceptions.ValidationException("Id mismatch", ["the route id not the same as the request id"]);
        }

        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Update Customer success.");
    }

    [Authorize]
    [HttpDelete("{id: Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new DeleteCustomerRequest(id);
        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Delete Customer success.");
    }

    [Authorize]
    [HttpGet("{id: Guid}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new GetCustomerByIdRequest(id);
        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Get Customer success.");
    }
}
