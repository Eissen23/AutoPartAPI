using System;
using System.Collections.Generic;
using System.Text;

namespace Application.InvoiceItems.Handlers;

public class GetInvoiceItemHandler(
        IInvoiceItemService invoiceItemService
    ) : IRequestHandler<GetInvoiceItemByIdRequest, InvoiceItemDto?>
{
    private readonly IInvoiceItemService _invoiceItemService = invoiceItemService;

    public async Task<InvoiceItemDto?> Handle(GetInvoiceItemByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _invoiceItemService.GetByIdAsync(request.Id, cancellationToken);

        return result;
    }
}
