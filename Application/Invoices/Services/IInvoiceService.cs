using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.Invoices.Models;

namespace Base.Application.Invoices.Services;

public interface IInvoiceService : ITransientService
{
    Task<PaginatedResponse<InvoiceDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default);
    Task<List<InvoiceDto>> GetAllAsync(CancellationToken ct = default);
    Task<InvoiceDto> GetByIdAsync(Guid invoiceId, CancellationToken ct = default);
    Task<Guid> CreateAsync(CreateInvoiceRequest request, CancellationToken ct = default);
    Task<Guid> UpdateAsync(Guid id, UpdateInvoiceRequest request, CancellationToken ct = default);
    Task<Guid> DeleteAsync(Guid invoiceId, CancellationToken ct = default);
}
