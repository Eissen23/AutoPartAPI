using Base.Application.Common.Models;
using Base.Application.InvoiceItems.Models;
using Base.Application.InvoiceItems.Services;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Invoces;

public class InvoiceItemsController (
        IInvoiceItemService invoiceItemService
    ) : VersionedApiController
{
    private readonly IInvoiceItemService _invoiceItemService = invoiceItemService;

    [Authorize, HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(CreateInvoiceItemRequest request, CancellationToken ct)
    {
        var result = await _invoiceItemService.CreateAsync(request, ct);
        return this.ApiOk(result, "Create Invoice Item Success");
    }

    [Authorize, HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<InvoiceItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(SearchInvoiceItemRequest request, CancellationToken ct)
    {
        var result = await _invoiceItemService.SearchAsync(request, ct);
        return this.ApiOk(result, "Search Invoice Item Success");
    }

    [Authorize, HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, UpdateInvoiceItemRequest request , CancellationToken ct)
    {
        var result = await _invoiceItemService.UpdateAsync(id, request, ct);
        return this.ApiOk(result, "Update Invoice Item Success");
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _invoiceItemService.DeleteAsync(id, ct);
        return this.ApiOk(result, "Delete Invoice Item Success");
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<InvoiceItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _invoiceItemService.GetByIdAsync(id, ct);
        return this.ApiOk(result, "Get Invoice Item By Id Success");
    }
}
