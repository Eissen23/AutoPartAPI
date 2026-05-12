using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;

namespace Base.Application.InvoiceItems.Models;

public class InvoiceItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
