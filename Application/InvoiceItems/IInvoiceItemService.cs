using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Application.Common.Models;

namespace Application.InvoiceItems;

public interface IInvoiceItemService : ITransientService
{
    Task<PaginatedResponse<InvoiceItemDto>> SearchAsync(PaginationFilter filter, CancellationToken ct);
    Task<List<InvoiceItemDto>> GetAllAsync(CancellationToken ct);
    Task<InvoiceItemDto> GetByIdAsync(Guid invoiceItemId, CancellationToken ct);
    Task<Guid> CreateAsync(CreateInvoiceItemRequest request, CancellationToken ct);
    Task<Guid> UpdateAsync(UpdateInvoiceItemRequest request, CancellationToken ct);
    Task<Guid> DeleteAsync(Guid invoiceItemId, CancellationToken ct);
}
