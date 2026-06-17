using FluentValidation;
using Base.Application.Identities.Roles.Models;

namespace Base.Application.Identities.Roles.Validations;

public class RoleValidation : AbstractValidator<CreateRoleRequest>
{
    public RoleValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => x.Description is not null);
    }
}
