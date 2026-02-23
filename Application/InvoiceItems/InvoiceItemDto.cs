using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;

namespace Application.InvoiceItems;

public class InvoiceItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
