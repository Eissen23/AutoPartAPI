using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Extension;
using Application.Common.Models;
using Application.Persistence.Repository;
using Domain.Entities.Invoices;
using Shared.Common.Exceptions;

namespace Application.InvoiceItems;

public class InvoiceItemService(
        IRepositoryWithEvents<InvoiceItem> eventRepos,
        IReadRepository<InvoiceItem> readRepos
    ) : IInvoiceItemService
{
    private readonly IRepositoryWithEvents<InvoiceItem> _eventRepos = eventRepos;
    private readonly IReadRepository<InvoiceItem> _readRepos = readRepos;

    public async Task<Guid> CreateAsync(CreateInvoiceItemRequest request, CancellationToken ct)
    {
        var invoiceItem = new InvoiceItem()
            .Update(
                request.Quantity,
                request.UnitPrice,
                request.ProductId,
                request.InvoiceId
            );

        var result = await _eventRepos.AddAsync(invoiceItem, ct);

        return result.Id;
    }

    public async Task<Guid> DeleteAsync(Guid invoiceItemId, CancellationToken ct)
    {
        var invoiceItem = await _readRepos.GetByIdAsync(invoiceItemId, ct);

        _ = invoiceItem ?? throw new NotFoundException($"Invoice item with id {invoiceItemId} not found.");

        await _eventRepos.DeleteAsync(invoiceItem, ct);

        return invoiceItem.Id;
    }

    public async Task<List<InvoiceItemDto>> GetAllAsync(CancellationToken ct)
    {
        var invoiceItems = await _readRepos.ListAsync(new GetAllInvoiceItems(), ct);

        return invoiceItems;
    }

    public async Task<InvoiceItemDto> GetByIdAsync(Guid invoiceItemId, CancellationToken ct)
    {
        var invoiceItem = await _readRepos.GetByIdAsync(invoiceItemId, ct);
        _ = invoiceItem ?? throw new NotFoundException($"Invoice item with id {invoiceItemId} not found.");

        return new InvoiceItemDto
        {
            Id = invoiceItem.Id,
            InvoiceId = invoiceItem.InvoiceId,
            ProductId = invoiceItem.ProductId,
            Quantity = invoiceItem.Quantity,
            UnitPrice = invoiceItem.UnitPrice
        };
    }

    public async Task<PaginatedResponse<InvoiceItemDto>> SearchAsync(PaginationFilter filter, CancellationToken ct)
    {
        var spec = new InvoiceItemPaginated(filter);
        var result = await _readRepos.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize, ct);

        return result;
    }

    public async Task<Guid> UpdateAsync(UpdateInvoiceItemRequest request, CancellationToken ct)
    {
        var invoiceItem = await _readRepos.GetByIdAsync(request.Id, ct);
        _ = invoiceItem ?? throw new NotFoundException($"Invoice item with id {request.Id} not found.");

        invoiceItem.Update(
            request.Quantity,
            request.UnitPrice,
            request.ProductId,
            request.InvoiceId
        );

        await _eventRepos.UpdateAsync(invoiceItem, ct);

        return invoiceItem.Id;
    }
}
