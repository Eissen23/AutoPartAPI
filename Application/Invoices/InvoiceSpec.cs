using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Application.Common.Specifications;
using Domain.Entities.Invoices;

namespace Application.Invoices;

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
