using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.Customers.Handlers;

public class SearchCustomerHandler(
        ICustomerService customerService
    ) : IRequestHandler<SearchCustomerRequest, PaginatedResponse<CustomerDto>>
{
    private readonly ICustomerService _customerService = customerService;

    public async Task<PaginatedResponse<CustomerDto>> Handle(SearchCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await _customerService.SearchAsync(request, cancellationToken);

        return result;
    }
}
