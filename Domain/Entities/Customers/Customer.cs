using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Customers;

public class Customer : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string? PhoneNumber { get; private set; }
    public string? Email { get; private set; }
    public CustomerType CustomerType { get; private set; }

    public Customer Update(
        string? name,
        string? email,
        string? phoneNumber,
        CustomerType? customerType)
    {
        if (name is not null && Name?.Equals(name) is not true)
            Name = name;
        if (email is not null && Email?.Equals(email) is not true)
            Email = email;
        if (phoneNumber is not null && PhoneNumber?.Equals(phoneNumber) is not true)
            PhoneNumber = phoneNumber;
        if (customerType.HasValue && CustomerType != customerType)
            CustomerType = customerType.Value;

        return this;
    }
}
