using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Products;

namespace Domain.Entities.Invoices;

public class InvoiceItem : AuditableEntity, IAggregateRoot
{
    public Guid InvoiceId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    // Navigation property
    public virtual Invoice Invoice { get; private set; } = default!;
    public virtual Product Product { get; private set; } = default!;

    public InvoiceItem Update(
        int? quantity,
        decimal? unitPrice,
        Guid? productId,
        Guid? invoiceId)
    {
        if (quantity.HasValue && Quantity != quantity)
            Quantity = quantity.Value;
        if (unitPrice.HasValue && UnitPrice != unitPrice)
            UnitPrice = unitPrice.Value;
        if (productId.HasValue && ProductId != productId)
            ProductId = productId.Value;
        if (invoiceId.HasValue && InvoiceId != productId)
            ProductId = invoiceId.Value;

        return this;
    }
}
