using System;
using System.Collections.Generic;
using System.Text;

namespace Application.InvoiceItems.Handlers;

internal class CreateInvoiceItemHandler(
        IInvoiceItemService invoiceItemService
    ) : IRequestHandler<CreateInvoiceItemRequest, Guid>
{
    private readonly IInvoiceItemService _invoiceItemService = invoiceItemService;

    public async Task<Guid> Handle(CreateInvoiceItemRequest request, CancellationToken cancellationToken)
    {
        var result = await _invoiceItemService.CreateAsync(request, cancellationToken);

        return result;
    }
}
