using System;
using System.Collections.Generic;

namespace PresentationTrainerVisualization.models
{
    public class AggregatedSession
    {
        public List<AggregatedObject> AggregatedObjects { get; set; }
        public DateTime SessionDate { get; set; }

        public AggregatedSession(List<AggregatedObject> aggregatedObjects)
        {
            AggregatedObjects = aggregatedObjects;
        }

        public AggregatedSession(List<AggregatedObject> aggregatedObjects, DateTime sessionDate)
        {
            AggregatedObjects = aggregatedObjects;
            SessionDate = sessionDate;
        }
    }
}
