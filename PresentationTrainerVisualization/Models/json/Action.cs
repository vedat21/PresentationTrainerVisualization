using Newtonsoft.Json;
using PresentationTrainerVisualization.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        public string? LogActionDisplay { get; set; }
        [JsonProperty("mistake")]
        public bool Mistake { get; set; }
        public bool isSelected { get; set; } // Only for listbox needed


        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            LogActionDisplay = Constants.ACTION_FROM_VIDEO[LogAction];

            // actions that are labeled as mistake have negative timestamp
            if (Start.StartsWith("-"))
                Start = Start.Substring(1, Start.Length - 1);
            if (End.StartsWith("-"))
                End = End.Substring(1, End.Length - 1);
        }

        // Only for ListBox items needed
        public Action(string? logAction, string? logActionDisplay, bool isSelected)
        {
            LogAction = logAction;
            LogActionDisplay = logActionDisplay;
            this.isSelected = isSelected;
        }
    }
}
