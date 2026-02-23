using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.Invoices;

public class CreateInvoiceRequest : IRequest<Guid>
{
    public Guid CustomerId { get; set; }
    public DateTime? SaleDate { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? TotalAmount { get; set; }
}

public class DeleteInvoiceRequest(Guid id) : IRequest<Guid>
{
    public Guid Id { get; set; } = id;
}

public class GetInvoiceByIdRequest(Guid id) : IRequest<InvoiceDto?>
{
    public Guid Id { get; set; } = id;
}

public class SearchInvoiceRequest : PaginationFilter, IRequest<PaginatedResponse<InvoiceDto>>
{
}

public class UpdateInvoiceRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    public DateTime? SaleDate { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? TotalAmount { get; set; }
}
