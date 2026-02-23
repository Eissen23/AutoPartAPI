using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Application.Common.Models;

namespace Application.Customers;

public interface ICustomerService : ITransientService
{
    Task<PaginatedResponse<CustomerDto>> SearchAsync(PaginationFilter filter, CancellationToken ct);
    Task<List<CustomerDto>> GetAllAsync(CancellationToken ct);
    Task<CustomerDto> GetByIdAsync(Guid customerId, CancellationToken ct);
    Task<Guid> CreateAsync(CreateCustomerRequest request, CancellationToken ct);
    Task<Guid> UpdateAsync(UpdateCustomerRequest request, CancellationToken ct);
    Task<Guid> DeleteAsync(Guid customerId, CancellationToken ct);
}
