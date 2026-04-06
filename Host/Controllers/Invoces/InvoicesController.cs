using Application.Common.Models;
using Application.Invoices.Models;
using Application.Invoices.Services;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Invoces;

public class InvoicesController(
        IInvoiceService invoiceService
    ) : VersionedApiController
{
    private readonly IInvoiceService _invoiceService = invoiceService;

    [Authorize, HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(CreateInvoiceRequest request, CancellationToken ct)
    {
        var result = await _invoiceService.CreateAsync(request, ct);
        return this.ApiOk(result, "Create Invoice Success");
    }

    [Authorize, HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<InvoiceDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(SearchInvoiceRequest request, CancellationToken ct)
    {
        var result = await _invoiceService.SearchAsync(request, ct);
        return this.ApiOk(result, "Search Invoice Success");
    }

    [Authorize, HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(UpdateInvoiceRequest request, [FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _invoiceService.UpdateAsync(id, request, ct);
        return this.ApiOk(result, "Update Invoice Success");
    }

    [Authorize, HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _invoiceService.DeleteAsync(id, ct);
        return this.ApiOk(result, "Delete Invoice Success");
    }

    [Authorize, HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<InvoiceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _invoiceService.GetByIdAsync(id, ct);
        return this.ApiOk(result, "Get Invoice By Id Success");
    }

}
