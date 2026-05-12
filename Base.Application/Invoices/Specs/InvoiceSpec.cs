using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Models;
using Base.Application.Common.Specifications;
using Base.Application.Invoices.Models;
using Base.Domain.Entities.Invoices;

namespace Base.Application.Invoices.Specs;

public class GetAllInvoices : Specification<Invoice, InvoiceDto>
{
    public GetAllInvoices()
    {
        Query.Select(invoice => new InvoiceDto
        {
            Id = invoice.Id,
            CustomerId = invoice.CustomerId,
            SaleDate = invoice.SaleDate,
            TaxAmount = invoice.TaxAmount,
            TotalAmount = invoice.TotalAmount
        });
    }
}

public class InvoicePaginated(PaginationFilter filter) : PaginationSpecification<Invoice, InvoiceDto>(filter)
{
}
