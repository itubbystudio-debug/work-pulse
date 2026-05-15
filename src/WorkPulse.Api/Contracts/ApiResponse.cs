namespace WorkPulse.Api.Contracts;

public sealed record ApiResponse<T>(bool Success, T Data);
