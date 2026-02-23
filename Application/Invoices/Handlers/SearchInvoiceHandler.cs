using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.Invoices.Handlers;

public class SearchInvoiceHandler(
        IInvoiceService invoiceService
    ) : IRequestHandler<SearchInvoiceRequest, PaginatedResponse<InvoiceDto>>
{
    private readonly IInvoiceService _invoiceService = invoiceService;

    public async Task<PaginatedResponse<InvoiceDto>> Handle(SearchInvoiceRequest request, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.SearchAsync(request, cancellationToken);

        return result;
    }
}
