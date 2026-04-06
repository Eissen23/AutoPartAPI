using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Application.Common.Models;
using Application.InvoiceItems.Models;

namespace Application.InvoiceItems.Services;

public interface IInvoiceItemService : ITransientService
{
    Task<PaginatedResponse<InvoiceItemDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default);
    Task<List<InvoiceItemDto>> GetAllAsync(CancellationToken ct = default);
    Task<InvoiceItemDto> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Guid> CreateAsync(CreateInvoiceItemRequest request, CancellationToken ct = default);
    Task<Guid> UpdateAsync(Guid id, UpdateInvoiceItemRequest request, CancellationToken ct = default);
    Task<Guid> DeleteAsync(Guid id, CancellationToken ct = default);
}
