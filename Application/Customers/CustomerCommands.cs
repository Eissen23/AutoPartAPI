using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Domain.Entities.Customers;

namespace Application.Customers;

public class CreateCustomerRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public CustomerType CustomerType { get; set; }
}

public class DeleteCustomerRequest(Guid id) : IRequest<Guid>
{
    public Guid Id { get; set; } = id;
}

public class GetCustomerByIdRequest(Guid id) : IRequest<CustomerDto?>
{
    public Guid Id { get; set; } = id;
}

public class SearchCustomerRequest : PaginationFilter, IRequest<PaginatedResponse<CustomerDto>>
{
}

public class UpdateCustomerRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public CustomerType? CustomerType { get; set; }
}

