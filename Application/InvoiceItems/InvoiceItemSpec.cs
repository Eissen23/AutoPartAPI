using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Application.Common.Specifications;
using Domain.Entities.Invoices;

namespace Application.InvoiceItems;

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
