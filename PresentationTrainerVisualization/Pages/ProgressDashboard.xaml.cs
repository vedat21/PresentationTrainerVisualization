using Newtonsoft.Json;
using PresentationTrainerVisualization.helper;
using PresentationTrainerVisualization.models;
using PresentationTrainerVisualization.models.json;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using static PresentationTrainerVisualization.helper.Constants;
using Color = System.Drawing.Color;

namespace PresentationTrainerVisualization.Pages
{
    public partial class ProgressDashboard : Page
    {
        private ProcessedSessionsData processedSessionsData { get; }
        private ProcessedGoalsData processedGoalsData;
        private ConfigurationRoot configurationRoot;
        private Configuration configuration;


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
                configuration = configurationRoot.Configurations.Find(x => x.Label == Constants.ConfigurationLabel.CONFIGURATION_DATES.ToString());
            }
            else
            {
                configurationRoot = new ConfigurationRoot();
                configurationRoot.Configurations = new List<Configuration>();
            }

            PlotPercentageOfRecongnisedInTimeLine();
            PlotNumberOfSessionsInTimeLine();
            PlotAverageORecongnisedSentences();
        }

        /// <summary>
        /// Plot Line chart that shows the percentage of identified sentences by session in timeline.
        /// </summary>
        private void PlotPercentageOfRecongnisedInTimeLine()
        {
            List<AggregatedSession> data = processedSessionsData.GetPercentageOfRecongnisedSentenceBySession().ToList();
            // Convert DateTime[] to double[] before plotting
            double[] xs = data.Select(x => x.SessionDate.ToOADate()).ToArray();
            double[] ys = data.Select(x => x.AggregatedObjects[0].Count).ToArray();
            Array.Sort(xs, ys);

            WpfPlot plot = (WpfPlot)FindName("PlotPercentageOfRecongnisedSentences");

            if (data.Count == 0)
            {
                plot.Plot.Title("No Data Available for chosen time");
                return;
            }

            plot.Plot.AddFill(xs, ys, color: Color.DodgerBlue);
            plot.Plot.AddScatter(xs, ys, color: Color.DodgerBlue, markerSize: 7);

            // Chart Configuration
            plot.Plot.XAxis.DateTimeFormat(true);
            plot.Plot.Title("Percentage of identified Sentences by Session");
            plot.Plot.SetAxisLimits(yMin: 0);
            plot.Plot.Legend(location: Alignment.UpperRight);
            plot.Refresh();
        }

        /// <summary> PROGRESS
        /// Plot the number of Sessions by dateonly in a timelinechart.
        /// </summary>
        private void PlotNumberOfSessionsInTimeLine()
        {
            var numberOfSessions = processedSessionsData.GetNumberOfSessionsByDate();

            var timeSpentByDate = processedSessionsData.GetDurationByDate();

            // Convert DateTime[] to double[] before plotting
            double[] xs = numberOfSessions.Keys.Select(x => x.ToDateTime(TimeOnly.Parse("00:00 PM")).ToOADate()).ToArray();
            double[] ys = numberOfSessions.Values.Select(x => (double)x).ToArray();
            Array.Sort(xs, ys);

            double[] ys2 = timeSpentByDate.Values.Select(x => (double)x).ToArray();
            Array.Sort(xs, ys2);

            WpfPlot plot = (WpfPlot)FindName("PlotNumberOfSessionsInTimeline");
            plot.Plot.AddFill(xs, ys);
            plot.Plot.AddScatter(xs, ys, color: Color.DodgerBlue, label: "Number of Sessions");
            plot.Plot.XAxis.DateTimeFormat(true);
            plot.Plot.Title("Number of Sessions");
            plot.Plot.SetAxisLimits(yMin: 0);
            plot.Plot.Legend(location: Alignment.UpperRight);
            plot.Refresh();
        }

        private void PlotAverageORecongnisedSentences()
        {
            double percentageOfRecongnisedSentences = processedSessionsData.GetAverageNumberOfRecongnisedSentencesByTime();

            Color prograssColor;
            if (percentageOfRecongnisedSentences < 25)
                prograssColor = Color.Red;
            else if (percentageOfRecongnisedSentences < 50 && percentageOfRecongnisedSentences >= 25)
                prograssColor = Color.Orange;
            else if (percentageOfRecongnisedSentences < 75 && percentageOfRecongnisedSentences >= 50)
                prograssColor = Color.FromArgb(255, 255, 244, 0); //yellow
            else
                prograssColor = Color.FromArgb(255, 44, 186, 0); //green


            WpfPlot plot = (WpfPlot)FindName("PlotTest");
            var pie = plot.Plot.AddPie(new double[] { percentageOfRecongnisedSentences, 100 - percentageOfRecongnisedSentences });

            // Chart Configuration
            plot.Plot.XAxis.Label("Average Recongnised Sentences", size: 16, color: Color.Gray, bold: true);
            pie.DonutSize = .7;
            pie.Size = 0.7;
            pie.DonutLabel = percentageOfRecongnisedSentences.ToString() + "%";
            pie.CenterFont.Size = 24f; // size of text inside donut
            pie.CenterFont.Color = prograssColor;
            pie.OutlineSize = 0.9f;
            pie.SliceFillColors = new Color[] { prograssColor, Color.LightGray };

            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Refresh();

        }

    }
}
