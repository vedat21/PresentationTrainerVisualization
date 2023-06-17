using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PresentationTrainerVisualization.models.json
{
    public class Goal
    {
        [JsonProperty("description")]
        public Dictionary<string, dynamic> Description { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("startdate")]
        public DateTime StartDate { get; set; }
    }

    public class GoalsRoot
    {
        [JsonProperty("goals")]
        public List<Goal> Goals { get; set; }
    }
}
