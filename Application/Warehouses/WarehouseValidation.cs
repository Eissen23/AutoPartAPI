using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Application.Warehouses;

public class CreateWarehouseLocationRequestValidator : AbstractValidator<CreateWarehouseLocationRequest>
{
    public CreateWarehouseLocationRequestValidator()
    {
        RuleFor(x => x.ZoneCode)
            .NotEmpty().WithMessage("ZoneCode is required.");

        RuleFor(x => x.Aisle)
            .GreaterThanOrEqualTo(0).WithMessage("Aisle must be a non-negative value.");

        RuleFor(x => x.Shelf)
            .GreaterThanOrEqualTo(0).WithMessage("Shelf must be a non-negative value.");

        RuleFor(x => x.IsOverstocked)
            .NotNull()
            .WithMessage("IsOverstocked is required.");
    }
}

public class UpdateWarehouseLocationRequestValidator : AbstractValidator<UpdateWarehouseLocationRequest>
{
    public UpdateWarehouseLocationRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        When(x => !string.IsNullOrWhiteSpace(x.ZoneCode), () =>
        {
            RuleFor(x => x.ZoneCode)
                .NotEmpty()
                .WithMessage("ZoneCode is required.");
        });

        When(x => x.Aisle.HasValue, () =>
        {
            RuleFor(x => x.Aisle)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Aisle must be a non-negative value.");
        });

        When(x => x.Shelf.HasValue, () =>
        {
            RuleFor(x => x.Shelf)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Shelf must be a non-negative value.");
        });

        When(x => x.IsOverstocked.HasValue, () =>
        {
            RuleFor(x => x.IsOverstocked)
                .NotNull()
                .WithMessage("IsOverstocked is required.");
        });
    }
}

public class DeleteWarehouseLocationRequestValidator : AbstractValidator<DeleteWarehouseLocationRequest>
{
    public DeleteWarehouseLocationRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("WarehouseLocation Id cannot be an empty GUID.");
    }
}

public class GetWarehouseLocationByIdRequestValidator : AbstractValidator<GetWarehouseLocationByIdRequest>
{
    public GetWarehouseLocationByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("WarehouseLocation Id cannot be an empty GUID.");
    }
}

