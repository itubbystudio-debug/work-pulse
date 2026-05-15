namespace WorkPulse.Application.Common.Interfaces;

public interface IRawSqlQueryCapability
{
    string PrimaryDatabase { get; }
    string PrimaryOrm { get; }
    string RawSqlTechnology { get; }
    bool IsConfigured { get; }
}
