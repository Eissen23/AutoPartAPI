namespace Application.Identities.Departments.Handlers;

public class CreateDepartmentHandler (
        IDepartmentService service
    ) : IRequestHandler<CreateDepartmentRequest, Guid>
{
    private readonly IDepartmentService _service = service;

    public async Task<Guid> Handle(CreateDepartmentRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);

        return result;
    }
}
