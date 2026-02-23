using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Application.Common.Models;

namespace Application.Invoices;

public interface IInvoiceService : ITransientService
{
    Task<PaginatedResponse<InvoiceDto>> SearchAsync(PaginationFilter filter, CancellationToken ct);
    Task<List<InvoiceDto>> GetAllAsync(CancellationToken ct);
    Task<InvoiceDto> GetByIdAsync(Guid invoiceId, CancellationToken ct);
    Task<Guid> CreateAsync(CreateInvoiceRequest request, CancellationToken ct);
    Task<Guid> UpdateAsync(UpdateInvoiceRequest request, CancellationToken ct);
    Task<Guid> DeleteAsync(Guid invoiceId, CancellationToken ct);
}
