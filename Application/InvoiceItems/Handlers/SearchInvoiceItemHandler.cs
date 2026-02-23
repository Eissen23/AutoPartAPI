using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.InvoiceItems.Handlers;

public class SearchInvoiceItemHandler(
        IInvoiceItemService invoiceItemService
    ) : IRequestHandler<SearchInvoiceItemRequest, PaginatedResponse<InvoiceItemDto>>
{
    private readonly IInvoiceItemService _invoiceItemService = invoiceItemService;

    public async Task<PaginatedResponse<InvoiceItemDto>> Handle(SearchInvoiceItemRequest request, CancellationToken cancellationToken)
    {
        var result = await _invoiceItemService.SearchAsync(request, cancellationToken);

        return result;
    }
}
