using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Application.Common.Models;
using Application.Customers.Models;

namespace Application.Customers.Services;

public interface ICustomerService : ITransientService
{
    Task<PaginatedResponse<CustomerDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default);
    Task<List<CustomerDto>> GetAllAsync(CancellationToken ct = default);
    Task<CustomerDto> GetByIdAsync(Guid customerId, CancellationToken ct = default);
    Task<Guid> CreateAsync(CreateCustomerRequest request, CancellationToken ct = default);
    Task<Guid> UpdateAsync(Guid id, UpdateCustomerRequest request, CancellationToken ct = default);
    Task<Guid> DeleteAsync(Guid customerId, CancellationToken ct = default);
}
