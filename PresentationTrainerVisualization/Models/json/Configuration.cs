using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainerVisualization.models.json
{
    class Configuration
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("startdate")]
        public DateOnly? StartDate { get; set; }

        [JsonProperty("enddate")]
        public DateOnly? EndDate { get; set; }

        [JsonProperty("lastDaysOrSessions")]
        public int LastDaysOrSessions { get; set; }

        [JsonProperty("isLastSessions")]
        public bool IsLastSessions { get; set; }
    }

    class ConfigurationRoot
    {
        [JsonProperty("configurations")]
        public List<Configuration> Configurations { get; set; }
    }
}
