using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Invoices.Handlers;

public class DeleteInvoiceHandler(
        IInvoiceService invoiceService
    ) : IRequestHandler<DeleteInvoiceRequest, Guid>
{
    private readonly IInvoiceService _invoiceService = invoiceService;

    public async Task<Guid> Handle(DeleteInvoiceRequest request, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.DeleteAsync(request.Id, cancellationToken);

        return result;
    }
}
