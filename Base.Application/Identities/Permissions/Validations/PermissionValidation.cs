using FluentValidation;
using Base.Application.Identities.Permissions.Models;

namespace Base.Application.Identities.Permissions.Validations;

public class PermissionValidation : AbstractValidator<CreatePermissionRequest>
{
    public PermissionValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => x.Description is not null);
    }
}
