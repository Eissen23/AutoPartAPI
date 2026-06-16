using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.Common.Services;
using Base.Application.Customers.Models;
using Base.Domain.Entities.Customers;
using Shared.Common.Exceptions;

namespace Base.Application.Customers.Services;

public class CustomerService(IApplicationDbContext context)
    : BaseService<Customer, CustomerDto>(context), ICustomerService
{
    public async Task<Guid> CreateAsync(CreateCustomerRequest request, CancellationToken ct)
    {
        var customer = Customer.Create(
            request.Name,
            request.Email,
            request.PhoneNumber,
            request.CustomerType);

        await base.CreateAsync(customer, ct);

        return customer.Id;
    }

    public async Task<Guid> DeleteAsync(Guid customerId, CancellationToken ct)
    {
        var customer = await FindAsync(customerId, ct);
        _ = customer ?? throw new NotFoundException($"Customer with id {customerId} not found.");

        await base.DeleteAsync(customer, ct);

        return customer.Id;
    }

    public Task<List<CustomerDto>> GetAllAsync(CancellationToken ct)
        => ListAsync(ct);

    public async Task<CustomerDto> GetByIdAsync(Guid customerId, CancellationToken ct)
    {
        var customer = await FindAsync(customerId, ct);
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

    public Task<PaginatedResponse<CustomerDto>> SearchAsync(PaginationFilter filter, CancellationToken ct)
        => PaginatedSearchAsync(filter, ct);

    public async Task<Guid> UpdateAsync(Guid id, UpdateCustomerRequest request, CancellationToken ct)
    {
        var customer = await FindAsync(id, ct);
        _ = customer ?? throw new NotFoundException($"Customer with id {id} not found.");

        customer.Update(
            request.Name,
            request.Email,
            request.PhoneNumber,
            request.CustomerType);

        await base.UpdateAsync(customer, ct);

        return customer.Id;
    }
}
