using PresentationTrainerVisualization.Helper;
using PresentationTrainerVisualization.models;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Color = System.Drawing.Color;

namespace PresentationTrainerVisualization.DashboardComponents.Progress
{
    public partial class PercentageOfIdentifiedInTimeLine : UserControl
    {
        private ProcessedSessions processedSessions;

        public PercentageOfIdentifiedInTimeLine()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();

            PlotPercentageOfIfdentifiedSentencesInTimeLine();
        }

        /// <summary>
        /// Plot Line chart that shows the percentage of identified sentences by session in timeline.
        /// </summary>
        private void PlotPercentageOfIfdentifiedSentencesInTimeLine()
        {
            List<AggregatedSession> data = processedSessions.GetPercentageOfRecongnisedSentenceBySession();
            // Convert DateTime[] to double[] before plotting
            double[] xs = data.Select(x => x.SessionDate.ToOADate()).ToArray();
            double[] ys = data.Select(x => x.AggregatedObjects[0].Count).ToArray();
            Array.Sort(xs, ys);

            WpfPlot plot = (WpfPlot)FindName("PercentageOfIdentifiedSentences");
            if (data.Count == 0)
            {
                plot.Plot.Title("No data available for selected period");
                return;
            }

            plot.Plot.AddFill(xs, ys, color: Constants.TIMELINE_COLOR);
            plot.Plot.AddScatter(xs, ys, color: Color.DodgerBlue, markerSize: 7);

            // Chart Configuration
            plot.Plot.XAxis.DateTimeFormat(true);
            plot.Plot.YAxis.Label("Percentage");
            plot.Plot.Title("Identified Sentences by Session");
            plot.Plot.SetAxisLimits(yMin: 0);
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Refresh();
        }
    }
}
