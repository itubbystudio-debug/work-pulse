using FluentValidation;

namespace WorkPulse.Application.Features.OrganizationStructure.Commands.CreateOrganizationNode;

public sealed class CreateOrganizationNodeCommandValidator : AbstractValidator<CreateOrganizationNodeCommand>
{
    public CreateOrganizationNodeCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(160);
    }
}
