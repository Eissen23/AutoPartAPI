using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Extension;
using Application.Common.Models;
using Application.Persistence.Repository;
using Domain.Entities.Customers;
using Shared.Common.Exceptions;

namespace Application.Customers;

public class CustomerService(
        IRepositoryWithEvents<Customer> eventRepos,
        IReadRepository<Customer> readRepos
    ) : ICustomerService
{
    private readonly IRepositoryWithEvents<Customer> _eventRepos = eventRepos;
    private readonly IReadRepository<Customer> _readRepos = readRepos;

    public async Task<Guid> CreateAsync(CreateCustomerRequest request, CancellationToken ct)
    {
        var customer = new Customer()
            .Update(
                request.Name,
                request.Email,
                request.PhoneNumber,
                request.CustomerType
            );

        var result = await _eventRepos.AddAsync(customer, ct);

        return result.Id;
    }

    public async Task<Guid> DeleteAsync(Guid customerId, CancellationToken ct)
    {
        var customer = await _readRepos.GetByIdAsync(customerId, ct);

        _ = customer ?? throw new NotFoundException($"Customer with id {customerId} not found.");

        await _eventRepos.DeleteAsync(customer, ct);

        return customer.Id;
    }

    public async Task<List<CustomerDto>> GetAllAsync(CancellationToken ct)
    {
        var customers = await _readRepos.ListAsync(new GetAllCustomers(), ct);

        return customers;
    }

    public async Task<CustomerDto> GetByIdAsync(Guid customerId, CancellationToken ct)
    {
        var customer = await _readRepos.GetByIdAsync(customerId, ct);
        _ = customer ?? throw new NotFoundException($"Customer with id {customerId} not found.");

        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            CustomerType = customer.CustomerType
        };
    }

    public async Task<PaginatedResponse<CustomerDto>> SearchAsync(PaginationFilter filter, CancellationToken ct)
    {
        var spec = new CustomerPaginated(filter);
        var result = await _readRepos.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize, ct);

        return result;
    }

    public async Task<Guid> UpdateAsync(UpdateCustomerRequest request, CancellationToken ct)
    {
        var customer = await _readRepos.GetByIdAsync(request.Id, ct);
        _ = customer ?? throw new NotFoundException($"Customer with id {request.Id} not found.");

        customer.Update(
            request.Name,
            request.Email,
            request.PhoneNumber,
            request.CustomerType
        );

        await _eventRepos.UpdateAsync(customer, ct);

        return customer.Id;
    }
}
