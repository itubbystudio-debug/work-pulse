using FluentValidation;

namespace WorkPulse.Application.Features.OrganizationStructure.Commands.UpdateOrganizationNode;

public sealed class UpdateOrganizationNodeCommandValidator : AbstractValidator<UpdateOrganizationNodeCommand>
{
    public UpdateOrganizationNodeCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(160);
    }
}
