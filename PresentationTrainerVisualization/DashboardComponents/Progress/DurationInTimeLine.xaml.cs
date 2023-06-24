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
    public partial class DurationInTimeLine : UserControl
    {

        private ProcessedSessions processedSessions;
        private ProcessedConfigurations processedConfigurations;

        public DurationInTimeLine()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();
            processedConfigurations = new ProcessedConfigurations();

            PlotDurationOfSessionInTimeLine();
        }

        private void PlotDurationOfSessionInTimeLine()
        {
            List<AggregatedSession> result = processedSessions.GetDurationBySession();

            double[] data = new double[result.Count];
            for (int i = 0; i < result.Count; i++)
                data[i] = (result[i].AggregatedObjects[0].Count) / 60;

            // Add datalabels and positions of label in chart
            List<string> DataLabels = new List<string>();
            for (int i = 0; i < result.Count; i++)
                DataLabels.Add(result[i].SessionDate.ToString("MM/dd/yyyy\nhh:mm tt"));

            double[] positions = Array.ConvertAll(Enumerable.Range(0, result.Count).ToArray(), x => (double)x);

            // Create plot

            WpfPlot plot = (WpfPlot)FindName("DurationInTimeLinePlot");
            // if data is empty
            if (data.Length == 0)
            {
                plot.Plot.Title("No data available for selected period");
                return;
            }

            var bar = plot.Plot.AddBar(data, color: Constants.TIMELINE_COLOR);
            plot.Plot.XTicks(positions, DataLabels.ToArray());
            plot.Plot.YAxis.Label("duration (min)");
            plot.Plot.Legend(location: Alignment.UpperRight);
            plot.Plot.SetAxisLimits(yMin: 0);
            plot.Plot.Title("Duration by Session");
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Refresh();
        }
    }
}
