using Newtonsoft.Json;
using PresentationTrainerVisualization.helper;
using PresentationTrainerVisualization.models;
using PresentationTrainerVisualization.models.json;
using ScottPlot;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Controls;
using static PresentationTrainerVisualization.helper.Constants;
using Color = System.Drawing.Color;


namespace PresentationTrainerVisualization.Pages
{
    public partial class FeedbackDashboard : Page
    {
        private ProcessedSessionsData processedSessionsData { get; }
        private ProcessedGoalsData processedGoalsData;
        private ConfigurationRoot configurationRoot;
        private Session selectedSession;

        private int DEFAULT_NUMBER_OF_SESSIONS = 7;


        public FeedbackDashboard()
        {
            InitializeComponent();
            processedSessionsData = new ProcessedSessionsData();
            processedGoalsData = new ProcessedGoalsData();

            // json file only exists if user has set goals in the once.
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

            // Get value of selected Session in combobox
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    selectedSession = (Session)(window as MainWindow).ComboSessions.SelectedItem;
                }
            }

            PlotRadarWithBadLabelsFromVideo();
            PlotRadarWithGoodLabelsFromVideo();
            PlotGoalDurationOfSession();
            PlotPercentageOfIdentifiedSentencesInSelectedSession();
          //  PlotTest();
        }

        private void PlotTest()
        {
            var actions = processedSessionsData.GetAllActionsFromLastSession();
            WpfPlot plot = (WpfPlot)FindName("RadarBadActions");

            int index = 0;
            foreach (var value in actions)
            {

                if (value.Mistake)
                {
                    var lolipop = plot.Plot.AddLollipop(
                                       values: new double[] { 10 },
                                       positions: new double[] { index },
                                       color: Color.Red);

                }
                else
                {
                    var lolipop = plot.Plot.AddLollipop(
                                       values: new double[] { 10 },
                                       positions: new double[] { index },
                                       color: Color.Gray);
                }
                index++;
            }
            // var lolipop = plot.Plot.AddLollipop(values);
            double[] xPositions = { 0, (int)actions.Count * 1/4, (int)actions.Count * 1/2, (int)actions.Count * 3/4, actions.Count };
            string[] xLabels = { "0:00", "0:30", "1:00", "1:30", "2:00" };
            plot.Plot.XAxis.ManualTickPositions(xPositions, xLabels);
            plot.Refresh();
        }

        private void PlotGoalDurationOfSession()
        {
            TextBlock timeLastSession = (TextBlock)FindName("TimeLastSession");
            TextBlock tipTimeLastSession = (TextBlock)FindName("TimeTipLastSession");
            TextBlock timeLastXSession = (TextBlock)FindName("TimeLastXSession");
            TextBlock tipTimeLastXSession = (TextBlock)FindName("TimeTipLastXSession");

            Configuration configuration = configurationRoot.Configurations.Find(x => x.Label == Constants.ConfigurationLabel.CONFIGURATION_LAST_X.ToString());

            List<Session> sessions = processedSessionsData.GetCopyOfAllSessions();
            TimeSpan result1 = sessions.Find(x => x.Start == selectedSession.Start).Duration;
            List<Session> sessionsResult2;

            // Decides what is shown
            if (configuration == null)
                sessionsResult2 = ProcessedSessionsDataHelper.GetLastSessions(sessions, DEFAULT_NUMBER_OF_SESSIONS);
            else
                if (configuration.IsLastSessions)
                sessionsResult2 = ProcessedSessionsDataHelper.GetLastSessions(sessions, configuration.LastDaysOrSessions);
            else
                sessionsResult2 = ProcessedSessionsDataHelper.GetSessionsOfLastDays(sessions, configuration.LastDaysOrSessions);

            // If data is emoty for chosen timespan
            if(result1 == null || sessionsResult2.Count == 0 )
                return;

            int index = 0;
            TimeSpan tempResult2 = new TimeSpan(0);
            foreach (var session in sessionsResult2)
            {
                tempResult2 += session.Duration;
                index++;
            }

            TimeSpan result2 = new TimeSpan(tempResult2.Ticks / index);

            Goal goal = processedGoalsData.GetGoalWithLabel(GoalsLabel.DurationOfSession.ToString());
            TimeSpan minTime = new TimeSpan(0, 0, int.Parse(goal.Description[GoalsDescription.session_duration_min_minutes.ToString()]), int.Parse(goal.Description[GoalsDescription.session_duration_min_seconds.ToString()]));
            TimeSpan maxTime = new TimeSpan(0, 0, int.Parse(goal.Description[GoalsDescription.session_duration_max_minutes.ToString()]), int.Parse(goal.Description[GoalsDescription.session_duration_max_seconds.ToString()]));

            timeLastSession.Text = result1.ToString("mm\\:ss") + " min";
            timeLastXSession.Text = result2.ToString("mm\\:ss") + " min";

            if (result1 > maxTime)
            {
                TimeSpan difference = maxTime - result1;
                timeLastSession.Foreground = new SolidColorBrush(Colors.Red);
                tipTimeLastSession.Text = Math.Abs(difference.TotalSeconds).ToString() + "sec overdrawn";
            }
            else if (result1 < minTime)
            {
                TimeSpan difference = result1 - minTime;
                timeLastSession.Foreground = new SolidColorBrush(Colors.Red);
                tipTimeLastSession.Text = Math.Abs(difference.TotalSeconds).ToString() + "sec short";
            }
            else
            {
                timeLastSession.Foreground = new SolidColorBrush(Colors.LimeGreen);
                tipTimeLastSession.Text = "Goal achieved";
            }

            // same with result2
            if (result2 > maxTime)
            {
                TimeSpan difference = maxTime - result2;
                timeLastXSession.Foreground = new SolidColorBrush(Colors.Red);
                tipTimeLastXSession.Text = Math.Abs(difference.TotalSeconds).ToString() + "sec overdrawn";
            }
            else if (result2 < minTime)
            {
                TimeSpan difference = result2 - minTime;
                timeLastXSession.Foreground = new SolidColorBrush(Colors.Red);
                tipTimeLastXSession.Text = Math.Abs(difference.TotalSeconds).ToString() + "sec short";
            }
            else
            {
                timeLastXSession.Foreground = new SolidColorBrush(Colors.LimeGreen);
                tipTimeLastXSession.Text = "Goal achieved";
            }


        }


        /// <summary> 
        /// Plots radarchart that shows mistake actions that were detected by video.
        /// Shows data of selected sessions and average of selected sessions.
        /// </summary>
        private void PlotRadarWithBadLabelsFromVideo()
        {
            Configuration configuration = configurationRoot.Configurations.Find(x => x.Label == Constants.ConfigurationLabel.CONFIGURATION_LAST_X.ToString());

            List<AggregatedSession> resultData = processedSessionsData.GetAggregatedActionsBySession(true);
            AggregatedSession result1 = resultData.Find(x => x.SessionDate == selectedSession.Start);
            AggregatedSession result2;
            string[] groupLabels;

            // Decides what is shown
            if (configuration == null)
            {
                result2 = ProcessedSessionsDataHelper.GetAverageOfLastSessions(resultData, DEFAULT_NUMBER_OF_SESSIONS);
                groupLabels = new[] { "Selected Session", "Average last " + DEFAULT_NUMBER_OF_SESSIONS + " Session" };
            }
            else
            {
                int lastDaysOrSessions = configuration.LastDaysOrSessions;
                if (configuration.IsLastSessions)
                {
                    result2 = ProcessedSessionsDataHelper.GetAverageOfLastSessions(resultData, lastDaysOrSessions);
                    groupLabels = new[] { "Selected Session", "Average last " + lastDaysOrSessions + " Session" };
                }
                else
                {
                    result2 = ProcessedSessionsDataHelper.GetAverageOfLastDays(resultData, lastDaysOrSessions);
                    groupLabels = new[] { "Selected Session", "Average last " + lastDaysOrSessions + " Days" };
                }
            }

            // Display Chart only when data is not empty
            if (result1.AggregatedObjects.Count == 0 || result2.AggregatedObjects.Count == 0)
                return;

            // entferne Einträge kleiner gleich 1
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
                categoryLabels.Add(Constants.ACTION_FROM_VIDEO[result1.AggregatedObjects[i].Label]);
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
            radar.CategoryLabels = categoryLabelsArr;
            radar.GroupLabels = groupLabels;
            radar.ShowAxisValues = true;
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
            plot.Refresh();
        }

        /// <summary> 
        /// Plots radarchart that shows actions (no mistake) that were detected by video.
        /// Shows data of selected sessions and average of selected sessions.
        /// </summary>
        private void PlotRadarWithGoodLabelsFromVideo()
        {
            Configuration configuration = configurationRoot.Configurations.Find(x => x.Label == Constants.ConfigurationLabel.CONFIGURATION_LAST_X.ToString());

            List<AggregatedSession> resultData = processedSessionsData.GetAggregatedActionsBySession(false);
            AggregatedSession result1 = resultData.Find(x => x.SessionDate == selectedSession.Start);
            AggregatedSession result2;
            string[] groupLabels;

            // Decides what is shown
            if (configuration == null)
            {
                result2 = ProcessedSessionsDataHelper.GetAverageOfLastSessions(resultData, DEFAULT_NUMBER_OF_SESSIONS);
                groupLabels = new[] { "Selected Session", "Average last " + DEFAULT_NUMBER_OF_SESSIONS + " Session" };
            }
            else
            {
                int lastDaysOrSessions = configuration.LastDaysOrSessions;
                if (configuration.IsLastSessions)
                {
                    result2 = ProcessedSessionsDataHelper.GetAverageOfLastSessions(resultData, lastDaysOrSessions);
                    groupLabels = new[] { "Selected Session", "Average last " + lastDaysOrSessions + " Session" };
                }
                else
                {
                    result2 = ProcessedSessionsDataHelper.GetAverageOfLastDays(resultData, lastDaysOrSessions);
                    groupLabels = new[] { "Selected Session", "Average last " + lastDaysOrSessions + " Days" };
                }
            }

            // Display Chart only when data is not empty
            if (result1.AggregatedObjects.Count == 0 || result2.AggregatedObjects.Count == 0)
                return;

            // entferne Einträge kleiner gleich 1
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
                categoryLabels.Add(Constants.ACTION_FROM_VIDEO[result2.AggregatedObjects[i].Label]);
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
            WpfPlot plot = (WpfPlot)FindName("RadarActions");
            var radar = plot.Plot.AddRadar(UtilityHelper.ConvertJaggedTo2D(resultArr));
            radar.CategoryLabels = categoryLabels.ToArray();
            radar.GroupLabels = new[] { "Last Session", "Average last 7 Session" };
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
            plot.Refresh();
        }

        /// <summary>
        /// Plots a donutchart that shows the percantage of identified sentences in the last session.
        /// </summary>
        private void PlotPercentageOfIdentifiedSentencesInSelectedSession()
        {

            List<AggregatedSession> dataAllSessions = processedSessionsData.GetPercentageOfRecongnisedSentenceBySession();
            AggregatedSession result = dataAllSessions.Find(x => x.SessionDate == selectedSession.Start);
            double percentageOfRecongnisedSentences = result.AggregatedObjects[0].Count;

            Color prograssColor;
            if (percentageOfRecongnisedSentences < 25)
                prograssColor = Color.Red;
            else if (percentageOfRecongnisedSentences < 50 && percentageOfRecongnisedSentences >= 25)
                prograssColor = Color.Orange;
            else if (percentageOfRecongnisedSentences < 75 && percentageOfRecongnisedSentences >= 50)
                prograssColor = Color.FromArgb(255, 255, 244, 0); //yellow
            else
                prograssColor = Color.FromArgb(255, 44, 186, 0); //green


            WpfPlot plot = (WpfPlot)FindName("DonutForCard");
            var pie = plot.Plot.AddPie(new double[] { percentageOfRecongnisedSentences, 100 - percentageOfRecongnisedSentences });

            // Chart Configuration
            plot.Plot.XAxis.Label("Recongnised Sentences", size: 16, color: Color.Gray, bold: true);
            pie.DonutSize = .7;
            pie.Size = 0.8;
            pie.DonutLabel = percentageOfRecongnisedSentences.ToString() + "%";
            pie.CenterFont.Color = prograssColor;
            pie.CenterFont.Size = 24f;
            pie.OutlineSize = 0.7f;
            pie.SliceFillColors = new Color[] { prograssColor, Color.LightGray };
            plot.Refresh();
        }

       

        private void PlotStackedBarChart()
        {
            WpfPlot plot = (WpfPlot)FindName("TestPlot3");


            var result = processedSessionsData.GetNumberOfRightAndWrongSentencesBySession();

            List<string> DataLabels = new List<string>();
            DataLabels.Add("Right Sentences");
            DataLabels.Add("Wrong Sentences");


            double[][] data = new double[result.Count][];
            for (int i = 0; i < data.Length; i++)
            {
                var sessionResult = result.ElementAt(i);
                data[i] = new double[sessionResult.Count];
                data[i][0] = sessionResult["right"];
                data[i][1] = sessionResult["wrong"];
            }


            double[] totalSummation = new double[data[0].Length];
            foreach (var ítem in data)
            {
                totalSummation[0] = totalSummation[0] + ítem[0];
                totalSummation[1] = totalSummation[1] + ítem[1];
            }

            // Update bar values for stackedbar
            var dataCopy = data;
            for (var i = data.Length - 1; i >= 0; i--)
            {
                if (data.Length - 1 == i)
                {
                    data[i] = totalSummation;
                }
                else
                {
                    for (int j = 0; j < data[i].Length; j++)
                    {
                        data[i][j] = data[i + 1][j] - dataCopy[i][j];
                    }
                }
            }

            // Plot each bar on top
            double[] positions = Array.ConvertAll(Enumerable.Range(0, data[0].Length).ToArray(), x => (double)x);
            for (var i = data.Length - 1; i >= 0; i--)
            {
                var bar = plot.Plot.AddBar(data[i]);
                bar.Label = "Session " + (i + 1);
            }



            plot.Plot.XTicks(positions, DataLabels.ToArray());
            plot.Plot.Legend(location: Alignment.UpperRight);
            plot.Plot.SetAxisLimits(yMin: 0);
            plot.Plot.Title("Sentence by Session");

            plot.Refresh();
        }


        private void PlotGroupedBarChart()
        {
            WpfPlot plot = (WpfPlot)FindName("TestPlot5");

            string[] seriesNames = { "Right", "Wrong" };
            string[] groupNames = {
                "Session 1", "Session 2", "Session 3"
            };

            var result = processedSessionsData.GetNumberOfRightAndWrongSentencesBySession();

            List<string> DataLabels = new List<string>();
            DataLabels.Add("Right Sentences");
            DataLabels.Add("Wrong Sentences");


            double[][] data = new double[result.Count][];
            for (int i = 0; i < data.Length; i++)
            {
                var sessionResult = result.ElementAt(i);
                data[i] = new double[sessionResult.Count];
                data[i][0] = sessionResult["right"];
                data[i][1] = sessionResult["wrong"];
            }


            // 2x2 array is transponded to be adjusted for function AddBarGroups. 
            var transpondedData = new double[data[0].Length][];
            for (int i = 0; i < data[0].Length; i++)
            {
                transpondedData[i] = new double[data.Length];
                for (int j = 0; j < data.Length; j++)
                    transpondedData[i][j] = data[j][i];

            }

            // 2d array with same shape as transpondedData but every value is zero. Needed for function AddBarGroups.
            var fakeErrorData = new double[transpondedData.Length][];
            for (int i = 0; i < transpondedData.Length; i++)
                fakeErrorData[i] = new double[transpondedData[i].Length];


            var bar = plot.Plot.AddBarGroups(groupNames, seriesNames, transpondedData, fakeErrorData);
            bar[0].ShowValuesAboveBars = true; bar[1].ShowValuesAboveBars = true;
            plot.Plot.Legend(location: Alignment.UpperRight);
            plot.Plot.Title("Sentence by Session");
            plot.Refresh();

        }

        private void PlotGroupedBarChartByLevel()
        {
            WpfPlot plot = (WpfPlot)FindName("TestPlot4");

            var result = processedSessionsData.GetNumberOfRightAndWrongSentencesByLevel();

            List<string> DataLabels = new List<string>();
            DataLabels.Add("Right Sentences");
            DataLabels.Add("Wrong Sentences");

            int index = 0;
            double[][] data = new double[result.Count][];
            List<string> groupNames = new List<string>();
            List<string> seriesNamesS = new List<string>();
            foreach (KeyValuePair<String, Dictionary<String, double>> kvp in result)
            {
                groupNames.Add("Level " + kvp.Key);
                data[index] = new double[kvp.Value.Count];
                data[index][0] = kvp.Value["right"];
                data[index][1] = kvp.Value["wrong"];

                index++;
            }
            string[] seriesNames = { "Right", "Wrong" };

            // 2x2 array is transponded to be adjusted as AddBarGroups parameter 
            var transpondedData = new double[data[0].Length][];
            for (int i = 0; i < data[0].Length; i++)
            {
                transpondedData[i] = new double[data.Length];
                for (int j = 0; j < data.Length; j++)
                    transpondedData[i][j] = data[j][i];
            }
            // 2d array with same shape as transpondedData but every value is zero. Needed for function AddBarGroups.
            var fakeErrorData = new double[transpondedData.Length][];
            for (int i = 0; i < transpondedData.Length; i++)
                fakeErrorData[i] = new double[transpondedData[i].Length];


            var bar = plot.Plot.AddBarGroups(groupNames.ToArray(), seriesNames, transpondedData, fakeErrorData);
            plot.Plot.Legend(location: Alignment.UpperRight);
            plot.Plot.Title("Sentence by Level");
            bar[0].ShowValuesAboveBars = true; bar[1].ShowValuesAboveBars = true;
            bar[0].Color = Color.DeepSkyBlue; bar[1].Color = Color.IndianRed;
            plot.Refresh();
        }

        private void PlotRadarChart()
        {
            WpfPlot plot = (WpfPlot)FindName("TestPlot1");

            var resultData = processedSessionsData.GetNumberOfRightAndWrongSentencesByLevel();

            int index = 0;
            double[][] data = new double[resultData.Count][];
            List<string> groupNames = new List<string>();
            List<string> seriesNamesS = new List<string>();
            foreach (KeyValuePair<String, Dictionary<String, double>> kvp in resultData)
            {
                groupNames.Add("Level " + kvp.Key);
                data[index] = new double[kvp.Value.Count];
                data[index][0] = kvp.Value["right"];
                data[index][1] = kvp.Value["wrong"];

                index++;
            }
            string[] seriesNames = { "Right", "Wrong" };


            // 2x2 array is transponded to be adjusted as AddBarGroups parameter 
            var transpondedData = new double[data[0].Length][];
            for (int i = 0; i < data[0].Length; i++)
            {
                transpondedData[i] = new double[data.Length];
                for (int j = 0; j < data.Length; j++)
                    transpondedData[i][j] = data[j][i];
            }

            var radar = plot.Plot.AddRadar(UtilityHelper.ConvertJaggedTo2D(transpondedData));
            radar.CategoryLabels = groupNames.ToArray();
            radar.GroupLabels = seriesNames;
            radar.ShowAxisValues = false;

            // customize the plot
            plot.Plot.Title("Recongnized Sentences by Level");
            plot.Plot.Legend();

            plot.Refresh();
        }
    }
}
