using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.InvoiceItems;

public class CreateInvoiceItemRequest : IRequest<Guid>
{
    public Guid InvoiceId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class DeleteInvoiceItemRequest(Guid id) : IRequest<Guid>
{
    public Guid Id { get; set; } = id;
}

public class GetInvoiceItemByIdRequest(Guid id) : IRequest<InvoiceItemDto?>
{
    public Guid Id { get; set; } = id;
}

public class SearchInvoiceItemRequest : PaginationFilter, IRequest<PaginatedResponse<InvoiceItemDto>>
{
}

public class UpdateInvoiceItemRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public Guid? InvoiceId { get; set; }
    public Guid? ProductId { get; set; }
    public int? Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
}
