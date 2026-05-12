using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Models;
using Base.Application.Common.Specifications;
using Base.Application.InvoiceItems.Models;
using Base.Domain.Entities.Invoices;

namespace Base.Application.InvoiceItems.Specs;

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
