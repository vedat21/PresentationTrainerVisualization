using PresentationTrainerVisualization.Helper;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Color = System.Drawing.Color;


namespace PresentationTrainerVisualization.DashboardComponents.Feedback
{
    public partial class ActionsVideoTimeLine : UserControl
    {
        private ProcessedSessions processedSessions { get; } 

        public ActionsVideoTimeLine()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();

            PlotCandleActions();
        }

        /// <summary>
        /// Plots actions in Candle chart with color depending on mistake or not.
        /// </summary>
        private void PlotCandleActions()
        {
            var actions = processedSessions.GetSelectedSessionActions();
            var durationOfSelectedSession = processedSessions.SelectedSession.Duration;
            WpfPlot plot = (WpfPlot)FindName("ActionsCandlePlot");

            // Display time in equal fractions
            string[] xLabels = new string[5];
            TimeSpan fractionDuration = TimeSpan.FromTicks(durationOfSelectedSession.Ticks / 4);
            for (int i = 0; i <= 4; i++)
            {
                TimeSpan fraction = TimeSpan.FromTicks(fractionDuration.Ticks * i);
                xLabels[i] = fraction.ToString("mm\\:ss");
            }

            List<string> labels = new List<string>();

            int index = 0;
            foreach (var action in actions)
            {

                if (action.Mistake)
                {
                    var lolipop = plot.Plot.AddLollipop(
                                       values: new double[] { (action.End - action.Start).TotalSeconds },
                                       positions: new double[] { index },
                                       color: Constants.BAD_INDICATOR_COLOR);

                }
                else
                {
                    var lolipop = plot.Plot.AddLollipop(
                                       values: new double[] { (action.End - action.Start).TotalSeconds },
                                       positions: new double[] { index },
                                       color: Constants.GOOD_INDICATOR_COLOR);
                }
                labels.Add(action.LogActionDisplay);
                index++;
            }

            //   double[] xPositions = { -0.5, (int)actions.Count * 1 / 4, (int)actions.Count * 1 / 2, (int)actions.Count * 3 / 4, actions.Count }; // postion of x labels
            double[] positions = Array.ConvertAll(Enumerable.Range(0, labels.Count).ToArray(), x => (double)x);
            plot.Plot.XAxis.ManualTickPositions(positions, labels.ToArray());
            plot.Plot.XAxis.TickLabelStyle(rotation: 60);
            //   plot.Plot.YAxis.Ticks(false);
            plot.Plot.YAxis.Label("duration (sec)");
            plot.Plot.Title("Distribution of Actions");
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Plot.Legend();
            plot.Refresh();
        }

        private void ActionVideoButtonClicked(object sender, RoutedEventArgs e)
        {
            VideoPlayerWindow window = new VideoPlayerWindow(true);
            window.Show();
        }
    }
}
