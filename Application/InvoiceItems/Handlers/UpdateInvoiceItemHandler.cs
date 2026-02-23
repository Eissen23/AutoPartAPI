using System;
using System.Collections.Generic;
using System.Text;

namespace Application.InvoiceItems.Handlers;

public class UpdateInvoiceItemHandler(
        IInvoiceItemService invoiceItemService
    ) : IRequestHandler<UpdateInvoiceItemRequest, Guid>
{
    private readonly IInvoiceItemService _invoiceItemService = invoiceItemService;

    public async Task<Guid> Handle(UpdateInvoiceItemRequest request, CancellationToken cancellationToken)
    {
        var result = await _invoiceItemService.UpdateAsync(request, cancellationToken);

        return result;
    }
}
