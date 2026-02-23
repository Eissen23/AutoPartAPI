using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Application.Common.Specifications;
using Domain.Entities.Customers;

namespace Application.Customers;

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
