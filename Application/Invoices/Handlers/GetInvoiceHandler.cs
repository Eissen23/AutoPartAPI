using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Invoices.Handlers;

public class GetInvoiceHandler(
        IInvoiceService invoiceService
    ) : IRequestHandler<GetInvoiceByIdRequest, InvoiceDto?>
{
    private readonly IInvoiceService _invoiceService = invoiceService;

    public async Task<InvoiceDto?> Handle(GetInvoiceByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.GetByIdAsync(request.Id, cancellationToken);

        return result;
    }
}
