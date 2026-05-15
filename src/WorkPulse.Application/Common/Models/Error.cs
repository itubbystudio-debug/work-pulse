namespace WorkPulse.Application.Common.Models;

public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error Validation(string code, string message) => new(code, message);
}
