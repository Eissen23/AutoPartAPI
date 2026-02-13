using System;
using System.Collections.Generic;
using System.Text;
using Application.Persistence.Repository;
using Domain.Entities.Identity;

namespace Application.Identities.Departments;

/// <summary>
/// Only handler the things not in range of 
/// </summary>
public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentRequestValidator(
        IReadRepository<Department> readRepository
        )
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Department name is required.")
            .MaximumLength(100)
            .WithMessage("Department name cannot exceed 100 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters.");

        When(x => x.ParentId.HasValue, () =>
        {
            RuleFor(x => x.ParentId)
                .NotEqual(Guid.Empty)
                .WithMessage("ParentId cannot be an empty GUID.")
                .MustAsync(async (dp, ct) =>
                    await readRepository.GetByIdAsync(dp!.Value, ct) is not null
                );
        });
    }
}

public class UpdateDepartmentRequestValidator : AbstractValidator<UpdateDepartmentRequest>
{
    public UpdateDepartmentRequestValidator(
        IReadRepository<Department> readRepository
        )
    {
        When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
        {
            RuleFor(x => x.Name)
                .MaximumLength(100)
                .WithMessage("Name cannot exceed 100 characters.");
        });

        When(x => x.Description != null, () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.");
        });

        When(x => x.ParentId.HasValue, () =>
        {
            RuleFor(x => x.ParentId)
                .NotEmpty() 
                .NotEqual(Guid.Empty)
                .WithMessage("ParentId cannot be an empty GUID.")
                .MustAsync(async (dp, ct) =>
                    await readRepository.GetByIdAsync(dp!.Value, ct) is not null
                );
        });
    }
}

public class DeleteDepartmentRequestValidator : AbstractValidator<DeleteDepartmentRequest>
{
    public DeleteDepartmentRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Department Id cannot be an empty GUID.");
    }
}

public class GetDepartmentByIdRequestValidator : AbstractValidator<GetDepartmentByIdRequest>
{
    public GetDepartmentByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Department Id cannot be an empty GUID.");
    }
}