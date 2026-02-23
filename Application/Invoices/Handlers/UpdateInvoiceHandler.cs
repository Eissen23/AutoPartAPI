using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Invoices.Handlers;

public class UpdateInvoiceHandler(
        IInvoiceService invoiceService
    ) : IRequestHandler<UpdateInvoiceRequest, Guid>
{
    private readonly IInvoiceService _invoiceService = invoiceService;

    public async Task<Guid> Handle(UpdateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.UpdateAsync(request, cancellationToken);

        return result;
    }
}
