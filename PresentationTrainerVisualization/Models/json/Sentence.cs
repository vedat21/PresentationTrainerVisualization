using Newtonsoft.Json;
using System;

namespace PresentationTrainerVisualization.models.json
{
    public class Sentence
    {
        [JsonProperty("end")]
        public TimeSpan End { get; set; }

        [JsonProperty("sentence")]
        public string? SentenceText { get; set; }

        [JsonProperty("start")]
        public TimeSpan Start { get; set; }

        [JsonProperty("wasIdentified")]
        public bool WasIdentified { get; set; }
    }
}
