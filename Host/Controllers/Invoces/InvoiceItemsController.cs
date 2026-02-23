using Application.Common.Models;
using Application.InvoiceItems;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Invoces;

public class InvoiceItemsController : VersionedApiController
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(CreateInvoiceItemRequest request, CancellationToken ct)
    {
        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Create Invoice Item Success");
    }

    [Authorize]
    [HttpPost]
    [Route("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<InvoiceItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(SearchInvoiceItemRequest request, CancellationToken ct)
    {
        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Search Invoice Item Success");
    }

    [Authorize]
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(UpdateInvoiceItemRequest request, [FromRoute] Guid id, CancellationToken ct)
    {
        if (request.Id != id)
        {
            throw new Shared.Common.Exceptions.ValidationException("Id mismatch", ["the route id not the same as the request id"]);
        }
        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Update Invoice Item Success");
    }

    [Authorize]
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await Mediator.Send(new DeleteInvoiceItemRequest(id), ct);
        return this.ApiOk(result, "Delete Invoice Item Success");
    }

    [Authorize]
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<InvoiceItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await Mediator.Send(new GetInvoiceItemByIdRequest(id), ct);
        return this.ApiOk(result, "Get Invoice Item By Id Success");
    }
}
