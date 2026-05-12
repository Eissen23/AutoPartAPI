using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Models;
using Base.Application.Common.Specifications;
using Base.Application.Customers.Models;
using Base.Domain.Entities.Customers;

namespace Base.Application.Customers.Specs;

public class GetAllCustomers : Specification<Customer, CustomerDto>
{
    public GetAllCustomers()
    {
        Query.Select(customer => new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            CustomerType = customer.CustomerType
        });
    }
}

public class CustomerPaginated(PaginationFilter filter) : PaginationSpecification<Customer, CustomerDto>(filter)
{
}
