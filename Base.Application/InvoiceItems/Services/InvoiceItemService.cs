using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.Common.Services;
using Base.Application.InvoiceItems.Models;
using Base.Domain.Entities.Invoices;
using Shared.Common.Exceptions;

namespace Base.Application.InvoiceItems.Services;

public class InvoiceItemService(IApplicationDbContext context)
    : BaseService<InvoiceItem, InvoiceItemDto>(context), IInvoiceItemService
{
    public async Task<Guid> CreateAsync(CreateInvoiceItemRequest request, CancellationToken ct)
    {
        var invoiceItem = InvoiceItem.Create(
            request.ProductId,
            request.InvoiceId,
            request.Quantity,
            request.UnitPrice);

        await base.CreateAsync(invoiceItem, ct);

        return invoiceItem.Id;
    }

    public async Task<Guid> DeleteAsync(Guid id, CancellationToken ct)
    {
        var invoiceItem = await FindAsync(id, ct);
        _ = invoiceItem ?? throw new NotFoundException($"Invoice item with id {id} not found.");

        await base.DeleteAsync(invoiceItem, ct);

        return invoiceItem.Id;
    }

    public Task<List<InvoiceItemDto>> GetAllAsync(CancellationToken ct)
        => ListAsync(ct);

    public async Task<InvoiceItemDto> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var invoiceItem = await FindAsync(id, ct);
        _ = invoiceItem ?? throw new NotFoundException($"Invoice item with id {id} not found.");

        return new InvoiceItemDto
        {
            Id = invoiceItem.Id,
            InvoiceId = invoiceItem.InvoiceId,
            ProductId = invoiceItem.ProductId,
            Quantity = invoiceItem.Quantity,
            UnitPrice = invoiceItem.UnitPrice
        };
    }

    public Task<PaginatedResponse<InvoiceItemDto>> SearchAsync(PaginationFilter filter, CancellationToken ct)
        => PaginatedSearchAsync(filter, ct);

    public async Task<Guid> UpdateAsync(Guid id, UpdateInvoiceItemRequest request, CancellationToken ct)
    {
        var invoiceItem = await FindAsync(id, ct);
        _ = invoiceItem ?? throw new NotFoundException($"Invoice item with id {id} not found.");

        invoiceItem.Update(
            request.Quantity,
            request.UnitPrice,
            request.ProductId,
            request.InvoiceId);

        await base.UpdateAsync(invoiceItem, ct);

        return invoiceItem.Id;
    }
}
