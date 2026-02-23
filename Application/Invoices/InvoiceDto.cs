using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;

namespace Application.Invoices;

public class InvoiceDto : IDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime? SaleDate { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? TotalAmount { get; set; }
}
