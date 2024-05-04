using Newtonsoft.Json;

namespace MessageStoreService.Domain
{
    public class MaintenanceIncidentWindowAsJson()
    {
        [JsonProperty("start")]
        public string? Start { get; set; }

        [JsonProperty("end")]
        public string? End { get; set; }
    }

    public class MaintenanceIncidentAsJson
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("siteId")]
        public string SiteId { get; set; }

        [JsonProperty("type")]
        public MaintenanceIncidentType Type { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("outline")]
        public string Outline { get; set; }

        [JsonProperty("window")]
        public MaintenanceIncidentWindowAsJson Window { get; set; }

        [JsonProperty("localStartDateTime")]
        public DateTime? LocalStartDateTime { get; set; }

        [JsonProperty("duration")]
        public TimeSpan Duration { get; set; }

        [JsonProperty("state")]
        public MaintenanceState State { get; set; }

        [JsonProperty("outageType")]
        public OutageType OutageType { get; set; }

        [JsonProperty("unplanned")]
        public bool Unplanned { get; set; }
    }
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
