using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PresentationTrainerVisualization.models.json
{
    public class Session
    {
        [JsonProperty("actions")]
        public List<Action>? Actions { get; set; }

        [JsonProperty("end")]
        public DateTime End { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("sentences")]
        public List<Sentence>? Sentences { get; set; }

        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("scriptVisible")]
        public bool ScriptVisible { get; set; }

        [JsonProperty("videoId")]
        public string? VideoId { get; set; }

        public TimeSpan Duration { get; set; }
        public string TextForComboBox { get; set; }
        public DateTime? StartForComboBox { get; set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Duration = End - Start;
            Duration = new TimeSpan(0, 0, (int)Duration.Minutes, (int)Duration.Seconds);
            StartForComboBox = Start; 
        }

       

    }

    public class SessionsRoot
    {
        [JsonProperty("sessions")]
        public List<Session>? Sessions { get; set; }
    }
}
