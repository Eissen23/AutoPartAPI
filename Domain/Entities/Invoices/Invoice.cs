using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Customers;

namespace Domain.Entities.Invoices;

public class Invoice : AuditableEntity, IAggregateRoot
{
    public Guid CustomerId { get; private set; }
    public DateTime? SaleDate { get; private set; }
    public decimal? TaxAmount { get; private set; }
    public decimal? TotalAmount { get; private set; }

    // Navigation property
    public virtual Customer Customer { get; private set; } = default!;

    public Invoice Update(
        Guid? customerId,
        DateTime? saleDate,
        decimal? taxAmount,
        decimal? totalAmount)
    {
        if (customerId.HasValue && CustomerId != customerId)
            CustomerId = customerId.Value;
        if (saleDate.HasValue && SaleDate != saleDate)
            SaleDate = saleDate.Value;
        if (taxAmount.HasValue && TaxAmount != taxAmount)
            TaxAmount = taxAmount.Value;
        if (totalAmount.HasValue && TotalAmount != totalAmount)
            TotalAmount = totalAmount.Value;

        return this;
    }
}
