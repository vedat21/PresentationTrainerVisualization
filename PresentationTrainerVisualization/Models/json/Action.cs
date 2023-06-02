using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainerVisualization.models.json
{
    public class Action
    {
        [JsonProperty("start")]
        public string? Start { get; set; }

        [JsonProperty("end")]
        public string? End { get; set; }

        [JsonProperty("id")]
        public double Id { get; set; }

        [JsonProperty("logAction")]
        public string? LogAction { get; set; }

        [JsonProperty("mistake")]
        public bool Mistake { get; set; }
    }
}
