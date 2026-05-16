using FluentValidation;

namespace WorkPulse.Application.Features.CompanyProfiles.Commands.UpdateCompanyProfile;

public sealed class UpdateCompanyProfileCommandValidator : AbstractValidator<UpdateCompanyProfileCommand>
{
    public UpdateCompanyProfileCommandValidator()
    {
        RuleFor(command => command.CompanyName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.TaxId)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(command => command.BranchName)
            .MaximumLength(120);

        RuleFor(command => command.Email)
            .MaximumLength(254)
            .EmailAddress()
            .When(command => !string.IsNullOrWhiteSpace(command.Email));

        RuleFor(command => command.PhoneNumber)
            .MaximumLength(50);

        RuleFor(command => command.Address)
            .MaximumLength(500);

        RuleFor(command => command.WebsiteUrl)
            .MaximumLength(300);
    }
}
