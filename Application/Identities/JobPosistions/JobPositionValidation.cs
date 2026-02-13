using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identities.JobPosistions;

public class CreateJobPositionRequestValidator : AbstractValidator<CreateJobPositionRequest>
{
    public CreateJobPositionRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotNull().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("DepartmentId is required.");

        RuleFor(x => x.Salary)
            .NotNull()
            .GreaterThanOrEqualTo(0).WithMessage("Salary must be a positive value.");

        RuleFor(x => x.AccessLevel)
            .NotNull()
            .WithMessage("AccessLevel need to be established");
    }
}

public class UpdateJobPositionRequestValidator : AbstractValidator<UpdateJobPositionRequest>
{
    public UpdateJobPositionRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        When(x => !string.IsNullOrWhiteSpace(x.Title), () =>
        {
            RuleFor(x => x.Title)
                .MaximumLength(100)
                .WithMessage("Title cannot exceed 100 characters.");
        });

        When(x => x.Description != null, () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.");
        });

        When(x => x.DepartmentId.HasValue, () =>
        {
            RuleFor(x => x.DepartmentId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("DepartmentId is invalid.");
        });

        When(x => x.Salary.HasValue, () =>
        {
            RuleFor(x => x.Salary)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Salary must be a positive value.");
        });

        When(x => x.AccessLevel.HasValue, () =>
        {
            RuleFor(x => x.AccessLevel)
                .NotNull()
                .WithMessage("AccessLevel need to be established");
        });

    }
}

public class DeleteJobPositionRequestValidator : AbstractValidator<DeleteJobPositionRequest>
{
    public DeleteJobPositionRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("JobPosition Id cannot be an empty GUID.");
    }
}
public class GetJobPositionByIdRequestValidator : AbstractValidator<GetJobPositionByIdRequest>
{
    public GetJobPositionByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("JobPosition Id cannot be an empty GUID.");
    }
}