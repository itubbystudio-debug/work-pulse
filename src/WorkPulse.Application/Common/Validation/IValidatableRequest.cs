namespace WorkPulse.Application.Common.Validation;

public interface IValidatableRequest
{
    IReadOnlyDictionary<string, string[]> Validate();
}
