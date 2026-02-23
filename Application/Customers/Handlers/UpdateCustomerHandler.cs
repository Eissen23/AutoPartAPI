using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Handlers;

public class UpdateCustomerHandler(
        ICustomerService customerService
    ) : IRequestHandler<UpdateCustomerRequest, Guid>
{
    private readonly ICustomerService _customerService = customerService;

    public async Task<Guid> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await _customerService.UpdateAsync(request, cancellationToken);

        return result;
    }
}
