using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;
using Base.Domain.Entities.Customers;

namespace Base.Application.Customers.Models;

public class CustomerDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public CustomerType CustomerType { get; set; }
}
