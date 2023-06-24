using PresentationTrainerVisualization.Helper;
using ScottPlot;
using System;
using System.Linq;
using System.Windows.Controls;
using Color = System.Drawing.Color;

namespace PresentationTrainerVisualization.DashboardComponents.Progress
{
    public partial class NumberOfSessionsInTimeLine : UserControl
    {
        private ProcessedSessions processedSessions;
        private ProcessedConfigurations processedConfigurations;

        public NumberOfSessionsInTimeLine()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();
            processedConfigurations = new ProcessedConfigurations();

            PlotNumberOfSessionsInTimeLine();
        }

        /// <summary> 
        /// Plot the number of Sessions by dateonly in a timelinechart.
        /// </summary>
        private void PlotNumberOfSessionsInTimeLine()
        {
            double[] xs;
            double[] ys;

            // When user selection of timespan includes only one day than show number of sessions for datetime. Else show number of sessions for dateonly.
            if (processedConfigurations.ConfigurationTimeSpan.StartDate == processedConfigurations.ConfigurationTimeSpan.EndDate)
            {
                var numberOfSessions = processedSessions.GetNumberOfSessionsByDateTime();
                xs = numberOfSessions.Keys.Select(x => x.ToOADate()).ToArray();
                ys = numberOfSessions.Values.Select(x => (double)x).ToArray();
            }
            else
            {
                var numberOfSessions = processedSessions.GetNumberOfSessionsByDateOnly();
                xs = numberOfSessions.Keys.Select(x => x.ToDateTime(TimeOnly.Parse("00:00 PM")).ToOADate()).ToArray();
                ys = numberOfSessions.Values.Select(x => (double)x).ToArray();
            }
            Array.Sort(xs, ys);


            WpfPlot plot = (WpfPlot)FindName("NumberOfSessionsInTimeline");
            if (xs.Length == 0 || ys.Length == 0)
            {
                plot.Plot.Title("No data available for selected period");
                return;
            }

            plot.Plot.AddFill(xs, ys, color: Constants.TIMELINE_COLOR);
            plot.Plot.AddScatter(xs, ys, color: Color.DodgerBlue, label: "Number of Sessions");
            plot.Plot.XAxis.DateTimeFormat(true);
            plot.Plot.Title("Number of Sessions");
            plot.Plot.SetAxisLimits(yMin: 0);
            plot.Plot.YAxis.ManualTickSpacing(1);
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Refresh();
        }
    }
}
