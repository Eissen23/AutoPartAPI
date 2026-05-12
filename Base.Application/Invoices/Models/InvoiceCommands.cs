using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Models;

namespace Base.Application.Invoices.Models;

public class CreateInvoiceRequest
{
    public Guid CustomerId { get; set; }
    public DateTime? SaleDate { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? TotalAmount { get; set; }
}

public class DeleteInvoiceRequest(Guid id)
{
    public Guid Id { get; set; } = id;
}

public class GetInvoiceByIdRequest(Guid id)
{
    public Guid Id { get; set; } = id;
}

public class SearchInvoiceRequest : PaginationFilter
{
}

public class UpdateInvoiceRequest 
{
    public Guid? CustomerId { get; set; }
    public DateTime? SaleDate { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? TotalAmount { get; set; }
}
