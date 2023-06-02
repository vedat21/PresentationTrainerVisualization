using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainerVisualization.models.json
{
    public class Sentence
    {
        [JsonProperty("wasIdentified")]
        public bool WasIdentified { get; set; }

        [JsonProperty("sentence")]
        public string? SentenceS { get; set; }
    }
}
