using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.Common.Services;
using Base.Application.Invoices.Models;
using Base.Domain.Entities.Invoices;
using Shared.Common.Exceptions;

namespace Base.Application.Invoices.Services;

public class InvoiceService(IApplicationDbContext context)
    : BaseService<Invoice, InvoiceDto>(context), IInvoiceService
{
    public async Task<Guid> CreateAsync(CreateInvoiceRequest request, CancellationToken ct = default)
    {
        var invoice = Invoice.Create(
            request.CustomerId,
            request.SaleDate,
            request.TaxAmount,
            request.TotalAmount);

        await base.CreateAsync(invoice, ct);

        return invoice.Id;
    }

    public async Task<Guid> DeleteAsync(Guid invoiceId, CancellationToken ct = default)
    {
        var invoice = await FindAsync(invoiceId, ct);
        _ = invoice ?? throw new NotFoundException($"Invoice with id {invoiceId} not found.");

        await base.DeleteAsync(invoice, ct);

        return invoice.Id;
    }

    public Task<List<InvoiceDto>> GetAllAsync(CancellationToken ct = default)
        => ListAsync(ct);

    public async Task<InvoiceDto> GetByIdAsync(Guid invoiceId, CancellationToken ct = default)
    {
        var invoice = await FindAsync(invoiceId, ct);
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

    public Task<PaginatedResponse<InvoiceDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default)
        => PaginatedSearchAsync(filter, ct);

    public async Task<Guid> UpdateAsync(Guid id, UpdateInvoiceRequest request, CancellationToken ct = default)
    {
        var invoice = await FindAsync(id, ct);
        _ = invoice ?? throw new NotFoundException($"Invoice with id {id} not found.");

        invoice.Update(
            request.CustomerId,
            request.SaleDate,
            request.TaxAmount,
            request.TotalAmount);

        await base.UpdateAsync(invoice, ct);

        return invoice.Id;
    }
}
