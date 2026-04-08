using System;
using System.Collections.Generic;
using System.Text;
using Application.Identities.JobPosistions.Models;

namespace Application.Identities.JobPosistions.Validations;

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