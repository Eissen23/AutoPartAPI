using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Models;
using Base.Domain.Entities.Customers;

namespace Base.Application.Customers.Models;

public class CreateCustomerRequest
{
    public string Name { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public CustomerType CustomerType { get; set; }
}

public class DeleteCustomerRequest(Guid id)
{
    public Guid Id { get; set; } = id;
}

public class GetCustomerByIdRequest(Guid id) 
{
    public Guid Id { get; set; } = id;
}

public class SearchCustomerRequest : PaginationFilter
{
}

public class UpdateCustomerRequest
{
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public CustomerType? CustomerType { get; set; }
}

