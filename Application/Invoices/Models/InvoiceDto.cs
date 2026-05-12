using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;

namespace Base.Application.Invoices.Models;

public class InvoiceDto : IDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime? SaleDate { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? TotalAmount { get; set; }
}
