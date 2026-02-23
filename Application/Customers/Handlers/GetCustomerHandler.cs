using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Handlers;

public class GetCustomerHandler(
        ICustomerService customerService
    ) : IRequestHandler<GetCustomerByIdRequest, CustomerDto?>
{
    private readonly ICustomerService _customerService = customerService;

    public async Task<CustomerDto?> Handle(GetCustomerByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _customerService.GetByIdAsync(request.Id, cancellationToken);

        return result;
    }
}
