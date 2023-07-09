using Newtonsoft.Json;
using PresentationTrainerVisualization.Helper;
using System;
using System.Runtime.Serialization;

namespace PresentationTrainerVisualization.models.json
{
    public class Action
    {
        [JsonProperty("id")]
        public double Id { get; set; }
        [JsonProperty("logAction")]
        public string? LogAction { get; set; }
        public string? LogActionDisplay { get; set; }
        [JsonProperty("mistake")]
        public bool Mistake { get; set; }
        [JsonProperty("start")]
        public TimeSpan Start { get; set; }
        [JsonProperty("end")]
        public TimeSpan End { get; set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            LogActionDisplay = Constants.ACTIONS_FROM_VIDEO[LogAction];

            // Actions that are labeled as mistake have negative timestamp in json file. 
            Start = Start.Duration();
            End = End.Duration();

        }

        // Only for ListBox items needed
        public Action(string? logAction, string? logActionDisplay)
        {
            LogAction = logAction;
            LogActionDisplay = logActionDisplay;
        }
    }
}
