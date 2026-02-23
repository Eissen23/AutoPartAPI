using Application.Common.Models;
using Application.Invoices;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Invoces;

public class InvoicesController : VersionedApiController
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(CreateInvoiceRequest request, CancellationToken ct)
    {
        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Create Invoice Success");
    }

    [Authorize]
    [HttpPost]
    [Route("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<InvoiceDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(SearchInvoiceRequest request, CancellationToken ct)
    {
        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Search Invoice Success");
    }

    [Authorize]
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(UpdateInvoiceRequest request, [FromRoute] Guid id, CancellationToken ct)
    {
        if (request.Id != id)
        {
            throw new Shared.Common.Exceptions.ValidationException("Id mismatch", ["the route id not the same as the request id"]);
        }
        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Update Invoice Success");
    }

    [Authorize]
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await Mediator.Send(new DeleteInvoiceRequest(id), ct);
        return this.ApiOk(result, "Delete Invoice Success");
    }

    [Authorize]
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<InvoiceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await Mediator.Send(new GetInvoiceByIdRequest(id), ct);
        return this.ApiOk(result, "Get Invoice By Id Success");
    }

}
