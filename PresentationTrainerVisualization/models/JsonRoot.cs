using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainerVisualization.models
{
    internal class JsonRoot
    {

        [JsonProperty("sessions")]
        public List<Session>? Sessions { get; set; }

    }
}
