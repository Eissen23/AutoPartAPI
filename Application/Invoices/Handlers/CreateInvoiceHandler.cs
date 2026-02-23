using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Invoices.Handlers;

internal class CreateInvoiceHandler(
        IInvoiceService invoiceService
    ) : IRequestHandler<CreateInvoiceRequest, Guid>
{
    private readonly IInvoiceService _invoiceService = invoiceService;

    public async Task<Guid> Handle(CreateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.CreateAsync(request, cancellationToken);

        return result;
    }
}
