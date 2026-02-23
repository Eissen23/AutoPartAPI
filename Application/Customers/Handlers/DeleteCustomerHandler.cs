using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Handlers;

public class DeleteCustomerHandler(
        ICustomerService customerService
    ) : IRequestHandler<DeleteCustomerRequest, Guid>
{
    private readonly ICustomerService _customerService = customerService;

    public async Task<Guid> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await _customerService.DeleteAsync(request.Id, cancellationToken);

        return result;
    }
}
