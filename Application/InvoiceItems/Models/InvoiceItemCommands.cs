using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.InvoiceItems.Models;

public class CreateInvoiceItemRequest
{
    public Guid InvoiceId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class DeleteInvoiceItemRequest(Guid id)
{
    public Guid Id { get; set; } = id;
}

public class GetInvoiceItemByIdRequest(Guid id)
{
   public Guid Id { get; set; } = id;
}

public class SearchInvoiceItemRequest : PaginationFilter
{
}

public class UpdateInvoiceItemRequest
{
    public Guid? InvoiceId { get; set; }
    public Guid? ProductId { get; set; }
    public int? Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
}
