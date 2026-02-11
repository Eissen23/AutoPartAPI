using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using MediatR;

namespace Application.Identities.Departments;

public class CreateDepartmentRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public Guid? ParentId { get; set; } 
}

public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(256);
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(512);
    }
}

