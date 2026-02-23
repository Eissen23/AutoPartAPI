using System;
using System.Collections.Generic;
using System.Text;

namespace Application.InvoiceItems.Handlers;

public class DeleteInvoiceItemHandler(
        IInvoiceItemService invoiceItemService
    ) : IRequestHandler<DeleteInvoiceItemRequest, Guid>
{
    private readonly IInvoiceItemService _invoiceItemService = invoiceItemService;

    public async Task<Guid> Handle(DeleteInvoiceItemRequest request, CancellationToken cancellationToken)
    {
        var result = await _invoiceItemService.DeleteAsync(request.Id, cancellationToken);

        return result;
    }
}
