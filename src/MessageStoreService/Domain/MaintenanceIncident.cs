namespace MessageStoreService.Domain;

public sealed record MaintenanceIncidentWindow(DateTime Start, DateTime End);

// blows up with "MaintenanceIncidentWindow? Window", can remove and see
public sealed record MaintenanceIncidentDTO(string Id, string SiteId, MaintenanceIncidentType Type, string? Title, string Outline, string[] Window, DateTime? LocalStartDateTime, TimeSpan Duration, MaintenanceState State, OutageType OutageType, bool Unplanned = false)
{
    public const string Container = "MaintenanceIncidents";
}

public enum MaintenanceIncidentType
{
    Unknown,
    Annual,
    Hardware,
    Software,
    Site
}

public enum MaintenanceState
{
    Unknown,
    Draft,
    Pending,
    Confirmed,
    Completed,
    Cancelled
}
public enum OutageType
{
    Unknown,
    None,
    Partial,
    Full
}
