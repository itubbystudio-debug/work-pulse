using FluentValidation;

namespace WorkPulse.Application.Features.Diagnostics.Commands.CreateWorkspace;

public sealed class CreateWorkspaceCommandValidator : AbstractValidator<CreateWorkspaceCommand>
{
    public CreateWorkspaceCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(120);
    }
}
