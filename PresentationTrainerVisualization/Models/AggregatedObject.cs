using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainerVisualization.models
{
    public class AggregatedObject
    {
        public string? Label { get; set; }
        public double Count { get; set; }

        public AggregatedObject(string? label, double count)
        {
            Label = label;
            Count = count;
        }
    }
}
