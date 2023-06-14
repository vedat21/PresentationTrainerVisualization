using Newtonsoft.Json;
using PresentationTrainerVisualization.helper;
using PresentationTrainerVisualization.models;
using PresentationTrainerVisualization.models.json;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Vortice;
using static PresentationTrainerVisualization.helper.Constants;
using Color = System.Drawing.Color;

namespace PresentationTrainerVisualization.Pages
{
    public partial class ProgressDashboard : Page
    {
        private ProcessedSessionsData processedSessionsData { get; }
        private ProcessedGoalsData processedGoalsData;
        private ConfigurationRoot configurationRoot;

        public ProgressDashboard()
        {
            InitializeComponent();
            processedSessionsData = new ProcessedSessionsData();
            processedGoalsData = new ProcessedGoalsData();

            // json file only exists if user has set goals in the past.
            if (File.Exists(Constants.PATH_TO_CONFIG_DATA))
            {
                string json = File.ReadAllText(Constants.PATH_TO_CONFIG_DATA);
                configurationRoot = JsonConvert.DeserializeObject<ConfigurationRoot>(json);
            }
            else
            {
                configurationRoot = new ConfigurationRoot();
                configurationRoot.Configurations = new List<Configuration>();
            }

            PlotAverageOfIdentifiedSentences();
            PlotPercentageOfIfdentifiedSentencesInTimeLine();
            PlotNumberOfSessionsInTimeLine();
            PlotActionsInTimeLine();
            PlotDurationOfSessionInTimeLine();
        }

        private void PlotAverageOfIdentifiedSentences()
        {
            double percentageOfRecongnisedSentences = processedSessionsData.GetAverageNumberOfIdentifiedentences();

            if (Double.IsNaN(percentageOfRecongnisedSentences))
                return;

            Color prograssColor;
            if (percentageOfRecongnisedSentences < 25)
                prograssColor = Color.Red;
            else if (percentageOfRecongnisedSentences < 50 && percentageOfRecongnisedSentences >= 25)
                prograssColor = Color.Orange;
            else if (percentageOfRecongnisedSentences < 75 && percentageOfRecongnisedSentences >= 50)
                prograssColor = Color.FromArgb(255, 255, 244, 0); //yellow
            else
                prograssColor = Color.FromArgb(255, 44, 186, 0); //green



            WpfPlot plot = (WpfPlot)FindName("AverageOfIdentifiedSentences");
            var pie = plot.Plot.AddPie(new double[] { percentageOfRecongnisedSentences, 100 - percentageOfRecongnisedSentences });

            // Chart Configuration
            pie.DonutSize = .7;
            pie.Size = 0.8;
            pie.DonutLabel = percentageOfRecongnisedSentences.ToString() + "%";
            pie.CenterFont.Size = 20f; // size of text inside donut
            pie.CenterFont.Color = prograssColor;
            pie.OutlineSize = 0.9f;
            pie.SliceFillColors = new Color[] { prograssColor, Color.LightGray };
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Refresh();

        }

        /// <summary>
        /// Plot Line chart that shows the percentage of identified sentences by session in timeline.
        /// </summary>
        private void PlotPercentageOfIfdentifiedSentencesInTimeLine()
        {
            List<AggregatedSession> data = processedSessionsData.GetPercentageOfRecongnisedSentenceBySession();
            // Convert DateTime[] to double[] before plotting
            double[] xs = data.Select(x => x.SessionDate.ToOADate()).ToArray();
            double[] ys = data.Select(x => x.AggregatedObjects[0].Count).ToArray();
            Array.Sort(xs, ys);

            WpfPlot plot = (WpfPlot)FindName("PercentageOfRecongnisedSentences");

            if (data.Count == 0)
            {
                plot.Plot.Title("No Data Available for chosen time");
                return;
            }

            plot.Plot.AddFill(xs, ys, color: Color.LightSkyBlue);
            plot.Plot.AddScatter(xs, ys, color: Color.DodgerBlue, markerSize: 7);

            // Chart Configuration
            plot.Plot.XAxis.DateTimeFormat(true);
            plot.Plot.Title("Percentage of identified Sentences by Session");
            plot.Plot.SetAxisLimits(yMin: 0);
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Refresh();
        }

        /// <summary> 
        /// Plot the number of Sessions by dateonly in a timelinechart.
        /// </summary>
        private void PlotNumberOfSessionsInTimeLine()
        {
            Configuration configurationTimeSpan = configurationRoot.Configurations.Find(x => x.Label == Constants.ConfigurationLabel.CONFIGURATION_DATES.ToString());

            double[] xs;
            double[] ys;

            // When user selection of timespan includes only one day than show number of sessions for datetime. Else show number of sessions for dateonly.
            if (configurationTimeSpan.StartDate == configurationTimeSpan.EndDate)
            {
                var numberOfSessions = processedSessionsData.GetNumberOfSessionsByDateTime();
                xs = numberOfSessions.Keys.Select(x => x.ToOADate()).ToArray();
                ys = numberOfSessions.Values.Select(x => (double)x).ToArray();
            }
            else
            {
                var numberOfSessions = processedSessionsData.GetNumberOfSessionsByDateOnly();
                xs = numberOfSessions.Keys.Select(x => x.ToDateTime(TimeOnly.Parse("00:00 PM")).ToOADate()).ToArray();
                ys = numberOfSessions.Values.Select(x => (double)x).ToArray();
            }
            Array.Sort(xs, ys);


            WpfPlot plot = (WpfPlot)FindName("NumberOfSessionsInTimeline");
            if (xs.Length == 0 || ys.Length == 0)
            {
                plot.Plot.Title("No Data Available for chosen time");
                return;
            }

            plot.Plot.AddFill(xs, ys, color: Color.LightSkyBlue);
            plot.Plot.AddScatter(xs, ys, color: Color.DodgerBlue, label: "Number of Sessions");
            plot.Plot.XAxis.DateTimeFormat(true);
            plot.Plot.Title("Number of Sessions");
            plot.Plot.SetAxisLimits(yMin: 0);
            plot.Plot.YAxis.ManualTickSpacing(1);
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Refresh();
        }

        private void PlotDurationOfSessionInTimeLine()
        {
            List<AggregatedSession> result = processedSessionsData.GetDurationBySession();

            double[] data = new double[result.Count];
            for (int i = 0; i < result.Count; i++)
                data[i] = (result[i].AggregatedObjects[0].Count);

            // Add datalabels and positions of label in chart
            List<string> DataLabels = new List<string>();
            for (int i = 0; i < result.Count; i++)
                DataLabels.Add(result[i].SessionDate.ToString("MM/dd/yyyy\nhh:mm tt"));

            double[] positions = Array.ConvertAll(Enumerable.Range(0, result.Count).ToArray(), x => (double)x);

            // Create plot
            WpfPlot plot = (WpfPlot)FindName("DurationInTimeLine");
            var bar = plot.Plot.AddBar(data);

            plot.Plot.XTicks(positions, DataLabels.ToArray());
            plot.Plot.Legend(location: Alignment.UpperRight);
            plot.Plot.SetAxisLimits(yMin: 0);
            plot.Plot.Title("Duration by Session");
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Refresh();
        }

        /// <summary>
        /// Plots the number of actions by session in stacked bar chart.
        /// </summary>
        private void PlotActionsInTimeLine()
        {
            List<AggregatedSession> resultGoodActions = processedSessionsData.GetNumberOfActionsBySession(false);
            List<AggregatedSession> resultBadActions = processedSessionsData.GetNumberOfActionsBySession(true);

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
            WpfPlot plot = (WpfPlot)FindName("ActionsInTimeLine");
            var barBadActions = plot.Plot.AddBar(dataBadStacked.ToArray());
            var barGoodActions = plot.Plot.AddBar(dataGood.ToArray());
            barGoodActions.Label = "No Mistake";
            barBadActions.Label = "Mistake";
            barGoodActions.Color = Color.Green;
            barBadActions.Color = Color.Red;

            plot.Plot.XTicks(positions, DataLabels.ToArray());
            plot.Plot.Legend(location: Alignment.UpperRight);
            plot.Plot.SetAxisLimits(yMin: 0);
            plot.Plot.Title("Actions by Session");
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Refresh();
        }

    }
}
