using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainerVisualization.models.json
{
    public class Goal
    {
        [JsonProperty("startdate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public Dictionary<string, dynamic> Description { get; set; }
    }

    public class GoalsRoot
    {
        [JsonProperty("goals")]
        public List<Goal> Goals { get; set; }
    }
}
