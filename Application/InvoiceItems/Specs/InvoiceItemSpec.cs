using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Application.Common.Specifications;
using Application.InvoiceItems.Models;
using Domain.Entities.Invoices;

namespace Application.InvoiceItems.Specs;

public class GetAllInvoiceItems : Specification<InvoiceItem, InvoiceItemDto>
{
    public GetAllInvoiceItems()
    {
        Query.Select(ii => new InvoiceItemDto
        {
            Id = ii.Id,
            InvoiceId = ii.InvoiceId,
            ProductId = ii.ProductId,
            Quantity = ii.Quantity,
            UnitPrice = ii.UnitPrice
        });
    }
}

public class InvoiceItemPaginated(PaginationFilter filter) : PaginationSpecification<InvoiceItem, InvoiceItemDto>(filter)
{
}
