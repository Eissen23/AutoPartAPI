using System;
using System.Collections.Generic;
using System.Text;
using Base.Domain.Entities.Products;

namespace Base.Domain.Entities.Invoices;

public class InvoiceItem : AuditableEntity, IAggregateRoot
{
    public Guid InvoiceId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    // Navigation property
    public virtual Invoice Invoice { get; private set; } = default!;
    public virtual Product Product { get; private set; } = default!;

    public static InvoiceItem Create(
        Guid invoiceId,
        Guid productId,
        int quantity,
        decimal unitPrice)
    {
        var entity = new InvoiceItem()
        {
            InvoiceId = invoiceId,
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
        return entity;
    }

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
        if (invoiceId.HasValue && InvoiceId != invoiceId)
            InvoiceId = invoiceId.Value;

        return this;
    }
}
