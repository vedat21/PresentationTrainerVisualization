using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainerVisualization.models
{
    internal class Session
    {
        [JsonProperty("actions")]
        public List<Action>? Actions { get; set; }
        
        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("sentences")]
        public List<Sentence>? Sentences { get; set; }

        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("scriptVisible")]
        public bool ScriptVisible { get; set; }

        [JsonProperty("videoId")]
        public string VideoId { get; set; }

    }
}
