using PresentationTrainerVisualization.Helper;
using PresentationTrainerVisualization.models;
using ScottPlot;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Color = System.Drawing.Color;


namespace PresentationTrainerVisualization.DashboardComponents.Feedback
{
    public partial class ActionsDistribution : UserControl
    {
        private ProcessedSessions processedSessions { get; }
        private ProcessedConfigurations processedConfiguration;

        public ActionsDistribution()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();
            processedConfiguration = ProcessedConfigurations.GetInstace();

            PlotRadarWithActionsFromVideo();
        }

        /// <summary> 
        /// Plots radarchart that shows actions that were detected by video.
        /// Shows data of selected sessions and average of selected sessions.
        /// </summary>
        private void PlotRadarWithActionsFromVideo()
        {
            List<AggregatedSession> resultData = processedSessions.GetAggregatedActionsBySession();
            AggregatedSession result1 = resultData.Find(x => x.SessionDate == processedSessions.SelectedSession.Start);
            AggregatedSession result2;
            string[] groupLabels;

            // Decides what is shown
            if (processedConfiguration.ConfigurationLastDays == null)
            {
                result2 = ProcessedSessionsHelper.GetAverageOfLastSessions(resultData, Constants.CONFIG_DEFAULT_NUMBER_OF_SESSIONS);
                groupLabels = new[] { "Selected Session", "Average last " + Constants.CONFIG_DEFAULT_NUMBER_OF_SESSIONS + " Session" };
            }
            else
            {
                int lastDaysOrSessions = processedConfiguration.ConfigurationLastDays.NumberOfSessions;
                if (processedConfiguration.ConfigurationLastDays.CompareWithLastSessions)
                {
                    result2 = ProcessedSessionsHelper.GetAverageOfLastSessions(resultData, lastDaysOrSessions);
                    groupLabels = new[] { "Selected Session", "Average last " + lastDaysOrSessions + " Session" };
                }
                else
                {
                    result2 = ProcessedSessionsHelper.GetAverageOfLastDays(resultData, lastDaysOrSessions);
                    groupLabels = new[] { "Selected Session", "Average last " + lastDaysOrSessions + " Days" };
                }
            }

            // Display Chart only when data is not empty
            if (result1.AggregatedObjects.Count == 0 || result2.AggregatedObjects.Count == 0)
                return;

            // remove elements where count is 0
            for (int i = 0; i < result1.AggregatedObjects.Count; i++)
            {
                if (result1.AggregatedObjects[i].Count == 0 && result2.AggregatedObjects[i].Count == 0)
                {
                    result1.AggregatedObjects.RemoveAt(i);
                    result2.AggregatedObjects.RemoveAt(i);
                }
            }


            List<List<double>> allValues = new List<List<double>>();
            List<double> values1 = new List<double>();
            List<double> values2 = new List<double>();
            List<string> categoryLabels = new List<string>();

            for (int i = 0; i < result1.AggregatedObjects.Count; ++i)
            {
                values1.Add(result1.AggregatedObjects[i].Count);
                values2.Add(result2.AggregatedObjects[i].Count);
                categoryLabels.Add(Constants.ACTIONS_FROM_VIDEO[result1.AggregatedObjects[i].Label]);
            }


            allValues.Add(values1);
            allValues.Add(values2);
            double[][] resultArr = allValues.Select(a => a.ToArray()).ToArray();
            string[] categoryLabelsArr = categoryLabels.ToArray();

            Array.Sort(categoryLabelsArr, resultArr[0]);
            Array.Sort(resultArr[1], resultArr[0]);
            Array.Sort(resultArr[0]);
            Array.Reverse(resultArr[0]);
            Array.Reverse(resultArr[1]);
            Array.Reverse(categoryLabelsArr);


            // Create Chart
            WpfPlot plot = (WpfPlot)FindName("RadarActions");
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);

            // Radarchart need atleast two data points.
            if (categoryLabels.Count <= 1)
            {
                plot.Plot.Title("No data available");
                return;
            }

            var radar = plot.Plot.AddRadar(UtilityHelper.ConvertJaggedTo2D(resultArr));
            radar.ShowAxisValues = false;
            radar.CategoryLabels = categoryLabelsArr;
            radar.GroupLabels = groupLabels;
            radar.LineColors = new Color[2]
            {
                Color.FromArgb(255, Constants.TIMELINE_COLOR) ,Color.Gray
            };
            radar.FillColors = new Color[2] {
               Color.FromArgb(50, Constants.TIMELINE_COLOR), Color.FromArgb(50, Color.Gray),
            };

            plot.Plot.Title("Detected Actions");
            plot.Plot.Legend();
            plot.Refresh();
        }


        /// <summary> 
        /// Plots radarchart that shows mistake actions that were detected by video.
        /// Shows data of selected sessions and average of selected sessions.
        /// </summary>
        private void PlotRadarWithBadLabelsFromVideo()
        {
            List<AggregatedSession> resultData = processedSessions.GetAggregatedActionsBySession(false, true);
            AggregatedSession result1 = resultData.Find(x => x.SessionDate == processedSessions.SelectedSession.Start);
            AggregatedSession result2;
            string[] groupLabels;

            // Decides what is shown
            if (processedConfiguration.ConfigurationLastDays == null)
            {
                result2 = ProcessedSessionsHelper.GetAverageOfLastSessions(resultData, Constants.CONFIG_DEFAULT_NUMBER_OF_SESSIONS);
                groupLabels = new[] { "Selected Session", "Average last " + Constants.CONFIG_DEFAULT_NUMBER_OF_SESSIONS + " Session" };
            }
            else
            {
                int lastDaysOrSessions = processedConfiguration.ConfigurationLastDays.NumberOfSessions;
                if (processedConfiguration.ConfigurationLastDays.CompareWithLastSessions)
                {
                    result2 = ProcessedSessionsHelper.GetAverageOfLastSessions(resultData, lastDaysOrSessions);
                    groupLabels = new[] { "Selected Session", "Average last " + lastDaysOrSessions + " Session" };
                }
                else
                {
                    result2 = ProcessedSessionsHelper.GetAverageOfLastDays(resultData, lastDaysOrSessions);
                    groupLabels = new[] { "Selected Session", "Average last " + lastDaysOrSessions + " Days" };
                }
            }

            // Display Chart only when data is not empty
            if (result1.AggregatedObjects.Count == 0 || result2.AggregatedObjects.Count == 0)
                return;

            // remove elements where count is 0
            for (int i = 0; i < result1.AggregatedObjects.Count; i++)
            {
                if (result1.AggregatedObjects[i].Count == 0 && result2.AggregatedObjects[i].Count == 0)
                {
                    result1.AggregatedObjects.RemoveAt(i);
                    result2.AggregatedObjects.RemoveAt(i);
                }
            }


            List<List<double>> allValues = new List<List<double>>();
            List<double> values1 = new List<double>();
            List<double> values2 = new List<double>();
            List<string> categoryLabels = new List<string>();

            for (int i = 0; i < result1.AggregatedObjects.Count; ++i)
            {
                values1.Add(result1.AggregatedObjects[i].Count);
                values2.Add(result2.AggregatedObjects[i].Count);
                categoryLabels.Add(Constants.ACTIONS_FROM_VIDEO[result1.AggregatedObjects[i].Label]);
            }

            allValues.Add(values1);
            allValues.Add(values2);
            double[][] resultArr = allValues.Select(a => a.ToArray()).ToArray();
            string[] categoryLabelsArr = categoryLabels.ToArray();

            Array.Sort(categoryLabelsArr, resultArr[0]);
            Array.Sort(resultArr[1], resultArr[0]);
            Array.Sort(resultArr[0]);
            Array.Reverse(resultArr[0]);
            Array.Reverse(resultArr[1]);
            Array.Reverse(categoryLabelsArr);

            // Chart Configuration
            WpfPlot plot = (WpfPlot)FindName("RadarBadActions");
            var radar = plot.Plot.AddRadar(UtilityHelper.ConvertJaggedTo2D(resultArr));
            radar.ShowAxisValues = false;
            radar.CategoryLabels = categoryLabelsArr;
            radar.GroupLabels = groupLabels;
            radar.LineColors = new Color[2]
            {
                Color.FromArgb(255,171, 50, 50) ,Color.Gray
            };
            radar.FillColors = new Color[2] {
               Color.FromArgb(50, 171, 50, 50), Color.FromArgb(50, Color.Gray),
            };
            radar.HatchOptions = new HatchOptions[]
            {
                new() { Pattern = HatchStyle.StripedUpwardDiagonal, Color = Color.FromArgb(100, Color.Gray) },
                new() { Pattern = HatchStyle.StripedDownwardDiagonal, Color = Color.FromArgb(100, Color.Gray) },
            };

            plot.Plot.Title("Detected mistakes");
            plot.Plot.Legend();
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Refresh();
        }

        /// <summary> 
        /// Plots radarchart that shows actions (no mistake) that were detected by video.
        /// Shows data of selected sessions and average of selected sessions.
        /// </summary>
        private void PlotRadarWithGoodLabelsFromVideo()
        {
            List<AggregatedSession> resultData = processedSessions.GetAggregatedActionsBySession(false, false);
            AggregatedSession result1 = resultData.Find(x => x.SessionDate == processedSessions.SelectedSession.Start);
            AggregatedSession result2;
            string[] groupLabels;

            // Decides what is shown
            if (processedConfiguration.ConfigurationLastDays == null)
            {
                result2 = ProcessedSessionsHelper.GetAverageOfLastSessions(resultData, Constants.CONFIG_DEFAULT_NUMBER_OF_SESSIONS);
                groupLabels = new[] { "Selected Session", "Average last " + Constants.CONFIG_DEFAULT_NUMBER_OF_SESSIONS + " Session" };
            }
            else
            {
                int lastDaysOrSessions = processedConfiguration.ConfigurationLastDays.NumberOfSessions;
                if (processedConfiguration.ConfigurationLastDays.CompareWithLastSessions)
                {
                    result2 = ProcessedSessionsHelper.GetAverageOfLastSessions(resultData, lastDaysOrSessions);
                    groupLabels = new[] { "Selected Session", "Average last " + lastDaysOrSessions + " Session" };
                }
                else
                {
                    result2 = ProcessedSessionsHelper.GetAverageOfLastDays(resultData, lastDaysOrSessions);
                    groupLabels = new[] { "Selected Session", "Average last " + lastDaysOrSessions + " Days" };
                }
            }

            // Display Chart only when data is not empty
            if (result1.AggregatedObjects.Count == 0 || result2.AggregatedObjects.Count == 0)
                return;

            // remove elements where count is 0
            for (int i = 0; i < result1.AggregatedObjects.Count; i++)
            {
                if (result1.AggregatedObjects[i].Count == 0 && result2.AggregatedObjects[i].Count == 0)
                {
                    result1.AggregatedObjects.RemoveAt(i);
                    result2.AggregatedObjects.RemoveAt(i);
                }
            }

            List<List<double>> allValues = new List<List<double>>();
            List<double> values1 = new List<double>();
            List<double> values2 = new List<double>();
            List<string> categoryLabels = new List<string>();

            for (int i = 0; i < result1.AggregatedObjects.Count; ++i)
            {
                values1.Add(result1.AggregatedObjects[i].Count);
                values2.Add(result2.AggregatedObjects[i].Count);
                categoryLabels.Add(Constants.ACTIONS_FROM_VIDEO[result2.AggregatedObjects[i].Label]);
            }

            allValues.Add(values1);
            allValues.Add(values2);
            double[][] resultArr = allValues.Select(a => a.ToArray()).ToArray();
            string[] categoryLabelsArr = categoryLabels.ToArray();

            Array.Sort(resultArr[0], categoryLabelsArr);
            Array.Sort(resultArr[0], resultArr[1]);
            Array.Sort(resultArr[0]);
            Array.Reverse(resultArr[0]);
            Array.Reverse(resultArr[1]);
            Array.Reverse(categoryLabelsArr);

            // Chart Configuration
            WpfPlot plot = (WpfPlot)FindName("RadarGoodActions");
            var radar = plot.Plot.AddRadar(UtilityHelper.ConvertJaggedTo2D(resultArr));
            radar.CategoryLabels = categoryLabels.ToArray();
            radar.GroupLabels = new[] { "Selected Session", "Average last 7 Session" };
            radar.ShowAxisValues = false;
            radar.LineColors = new Color[2]
            {
                Color.FromArgb(255,24, 89, 24) ,Color.Gray
            };
            radar.FillColors = new Color[2] {
               Color.FromArgb(50, 24, 89, 24), Color.FromArgb(50, Color.Gray),
            };
            radar.HatchOptions = new HatchOptions[]
            {
                new() { Pattern = HatchStyle.StripedUpwardDiagonal, Color = Color.FromArgb(100, Color.Gray) },
                new() { Pattern = HatchStyle.StripedDownwardDiagonal, Color = Color.FromArgb(100, Color.Gray) },
            };

            plot.Plot.Title("Detected actions");
            plot.Plot.Legend();
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Refresh();
        }

    }
}
