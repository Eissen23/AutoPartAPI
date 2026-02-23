using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Domain.Entities.Customers;

namespace Application.Customers;

public class CustomerDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public CustomerType CustomerType { get; set; }
}
