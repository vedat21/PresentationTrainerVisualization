using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PresentationTrainerVisualization.models.json
{
    public class Configuration
    {
        [JsonProperty("isLastSessions")]
        public bool CompareWithLastSessions { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("lastDaysOrSessions")]
        public int NumberOfSessions { get; set; }

        [JsonProperty("startdate")]
        public DateOnly? StartDate { get; set; }

        [JsonProperty("enddate")]
        public DateOnly? EndDate { get; set; }

    }

    public class ConfigurationsRoot
    {
        [JsonProperty("configurations")]
        public List<Configuration> Configurations { get; set; }
    }
}
