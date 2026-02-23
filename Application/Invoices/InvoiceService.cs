using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Extension;
using Application.Common.Models;
using Application.Persistence.Repository;
using Domain.Entities.Invoices;
using Shared.Common.Exceptions;

namespace Application.Invoices;

public class InvoiceService(
        IRepositoryWithEvents<Invoice> eventRepos,
        IReadRepository<Invoice> readRepos
    ) : IInvoiceService
{
    private readonly IRepositoryWithEvents<Invoice> _eventRepos = eventRepos;
    private readonly IReadRepository<Invoice> _readRepos = readRepos;

    public async Task<Guid> CreateAsync(CreateInvoiceRequest request, CancellationToken ct)
    {
        var invoice = new Invoice()
            .Update(
                request.CustomerId,
                request.SaleDate,
                request.TaxAmount,
                request.TotalAmount
            );

        var result = await _eventRepos.AddAsync(invoice, ct);

        return result.Id;
    }

    public async Task<Guid> DeleteAsync(Guid invoiceId, CancellationToken ct)
    {
        var invoice = await _readRepos.GetByIdAsync(invoiceId, ct);

        _ = invoice ?? throw new NotFoundException($"Invoice with id {invoiceId} not found.");

        await _eventRepos.DeleteAsync(invoice, ct);

        return invoice.Id;
    }

    public async Task<List<InvoiceDto>> GetAllAsync(CancellationToken ct)
    {
        var invoices = await _readRepos.ListAsync(new GetAllInvoices(), ct);

        return invoices;
    }

    public async Task<InvoiceDto> GetByIdAsync(Guid invoiceId, CancellationToken ct)
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

    public async Task<PaginatedResponse<InvoiceDto>> SearchAsync(PaginationFilter filter, CancellationToken ct)
    {
        var spec = new InvoicePaginated(filter);
        var result = await _readRepos.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize, ct);

        return result;
    }

    public async Task<Guid> UpdateAsync(UpdateInvoiceRequest request, CancellationToken ct)
    {
        var invoice = await _readRepos.GetByIdAsync(request.Id, ct);
        _ = invoice ?? throw new NotFoundException($"Invoice with id {request.Id} not found.");

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
