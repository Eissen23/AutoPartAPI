using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Extension;
using Base.Application.Common.Models;
using Base.Application.Invoices.Models;
using Base.Application.Invoices.Specs;
using Base.Application.Persistence.Repository;
using Base.Domain.Entities.Invoices;
using Shared.Common.Exceptions;

namespace Base.Application.Invoices.Services;

public class InvoiceService(
        IRepositoryWithEvents<Invoice> eventRepos,
        IReadRepository<Invoice> readRepos
    ) : IInvoiceService
{
    private readonly IRepositoryWithEvents<Invoice> _eventRepos = eventRepos;
    private readonly IReadRepository<Invoice> _readRepos = readRepos;

    public async Task<Guid> CreateAsync(CreateInvoiceRequest request, CancellationToken ct  = default)
    {
        var invoice = Invoice.Create(
                request.CustomerId,
                request.SaleDate,
                request.TaxAmount,
                request.TotalAmount
            );

        var result = await _eventRepos.AddAsync(invoice, ct);

        return result.Id;
    }

    public async Task<Guid> DeleteAsync(Guid invoiceId, CancellationToken ct = default)
    {
        var invoice = await _readRepos.GetByIdAsync(invoiceId, ct);

        _ = invoice ?? throw new NotFoundException($"Invoice with id {invoiceId} not found.");

        await _eventRepos.DeleteAsync(invoice, ct);

        return invoice.Id;
    }

    public async Task<List<InvoiceDto>> GetAllAsync(CancellationToken ct = default)
    {
        var invoices = await _readRepos.ListAsync(new GetAllInvoices(), ct);

        return invoices;
    }

    public async Task<InvoiceDto> GetByIdAsync(Guid invoiceId, CancellationToken ct = default)
    {
        var invoice = await _readRepos.GetByIdAsync(invoiceId, ct);
        _ = invoice ?? throw new NotFoundException($"Invoice with id {invoiceId} not found.");

        return new InvoiceDto
        {
            Id = invoice.Id,
            CustomerId = invoice.CustomerId,
            SaleDate = invoice.SaleDate,
            TaxAmount = invoice.TaxAmount,
            TotalAmount = invoice.TotalAmount
        };
    }

    public async Task<PaginatedResponse<InvoiceDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default)
    {
        var spec = new InvoicePaginated(filter);
        var result = await _readRepos.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize, ct);

        return result;
    }

    public async Task<Guid> UpdateAsync(Guid id, UpdateInvoiceRequest request, CancellationToken ct = default)
    {
        var invoice = await _readRepos.GetByIdAsync(id, ct);
        _ = invoice ?? throw new NotFoundException($"Invoice with id {id} not found.");

        invoice.Update(
            request.CustomerId,
            request.SaleDate,
            request.TaxAmount,
            request.TotalAmount
        );

        await _eventRepos.UpdateAsync(invoice, ct);

        return invoice.Id;
    }
}
