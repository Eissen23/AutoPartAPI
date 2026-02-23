using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Handlers;

internal class CreateCustomerHandler(
        ICustomerService customerService
    ) : IRequestHandler<CreateCustomerRequest, Guid>
{
    private readonly ICustomerService _customerService = customerService;

    public async Task<Guid> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await _customerService.CreateAsync(request, cancellationToken);

        return result;
    }
}
