using PresentationTrainerVisualization.Helper;
using PresentationTrainerVisualization.Helper;
using PresentationTrainerVisualization.models;
using ScottPlot;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows.Controls;
using Color = System.Drawing.Color;


namespace PresentationTrainerVisualization.DashboardComponents.Progress
{
 
    public partial class ActionsInTimeLine : UserControl
    {
        private ProcessedSessions processedSessions;

        public ActionsInTimeLine()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();

            PlotActionsInTimeLines();
        }

        /// <summary>
        /// Plots the number of actions by session in stacked bar chart.
        /// </summary>
        private void PlotActionsInTimeLines()
        {
            List<AggregatedSession> resultGoodActions = processedSessions.GetNumberOfActionsBySession(false);
            List<AggregatedSession> resultBadActions = processedSessions.GetNumberOfActionsBySession(true);

            List<double> dataGood = new List<double>();
            List<double> dataBad = new List<double>();

            for (int i = 0; i < resultGoodActions.Count; i++)
            {
                dataGood.Add(resultGoodActions[i].AggregatedObjects[0].Count);
                dataBad.Add(resultBadActions[i].AggregatedObjects[0].Count);
            }

            // Adjust data for stacked bar chart. 
            double[] dataBadStacked = new double[dataBad.Count];
            for (int i = 0; i < dataBad.Count; i++)
                dataBadStacked[i] = dataGood[i] + dataBad[i];

            // Add datalabels and positions of label in chart
            List<string> DataLabels = new List<string>();
            for (int i = 0; i < resultGoodActions.Count; i++)
                DataLabels.Add(resultGoodActions[i].SessionDate.ToString("MM/dd/yyyy\nhh:mm tt"));

            double[] positions = Array.ConvertAll(Enumerable.Range(0, dataGood.Count).ToArray(), x => (double)x);


            // Create plot
            WpfPlot plot = (WpfPlot)FindName("ActionsInTimeLinePlot");
            // if data is empty
            if (dataBad.Count == 0 && dataGood.Count == 0)
            {
                plot.Plot.Title("No data available for selected period");
                return;
            }

            var barBadActions = plot.Plot.AddBar(dataBadStacked.ToArray());
            var barGoodActions = plot.Plot.AddBar(dataGood.ToArray());
            barGoodActions.Label = "No Mistake";
            barBadActions.Label = "Mistake";
            barGoodActions.Color = Constants.GOOD_INDICATOR_COLOR;
            barBadActions.Color = Constants.BAD_INDICATOR_COLOR;

            plot.Plot.XTicks(positions, DataLabels.ToArray());
            plot.Plot.Legend(location: Alignment.UpperRight);
            plot.Plot.SetAxisLimits(yMin: 0);
            plot.Plot.Title("Actions by Session");
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Refresh();
        }
    }
}
