using Newtonsoft.Json;
using PresentationTrainerVisualization.helper;
using PresentationTrainerVisualization.models;
using PresentationTrainerVisualization.models.json;
using PresentationTrainerVisualization.windows;
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

            PlotGoalDurationOfSession();
            PlotPercentageOfIdentifiedSentencesInSelectedSession();
            PlotRadarWithBadLabelsFromVideo();
            PlotRadarWithGoodLabelsFromVideo();
            PlotCandleSentences();
            PlotCandleActions();
        }

        private void PlotGoalDurationOfSession()
        {
            TextBlock timeSelectedSession = (TextBlock)FindName("TimeLastSession");
            TextBlock tipTimeSelectedSession = (TextBlock)FindName("TimeTipLastSession");
            TextBlock timeLastXSession = (TextBlock)FindName("TimeLastXSession");
            TextBlock tipTimeLastXSession = (TextBlock)FindName("TimeTipLastXSession");

            Goal goal = processedGoalsData.GetGoalWithLabel(GoalsLabel.DurationOfSession.ToString());
            TimeSpan minTime = new TimeSpan(0, 0, int.Parse(goal.Description[GoalsDescription.session_duration_min_minutes.ToString()]), int.Parse(goal.Description[GoalsDescription.session_duration_min_seconds.ToString()]));
            TimeSpan maxTime = new TimeSpan(0, 0, int.Parse(goal.Description[GoalsDescription.session_duration_max_minutes.ToString()]), int.Parse(goal.Description[GoalsDescription.session_duration_max_seconds.ToString()]));

            Configuration configuration = configurationRoot.Configurations.Find(x => x.Label == Constants.ConfigurationLabel.CONFIGURATION_LAST_X.ToString());

            List<Session> sessions = processedSessionsData.GetCopyOfAllSessions();
            TimeSpan result1 = sessions.Find(x => x.Start == selectedSession.Start).Duration;

            // If data is empty for chosen timespan
            if (result1 == null)
                return;

            // display result for selected session
            if (result1 > maxTime)
            {
                TimeSpan difference = maxTime - result1;
                timeSelectedSession.Foreground = new SolidColorBrush(Colors.Red);
                tipTimeSelectedSession.Text = Math.Abs(difference.TotalSeconds).ToString() + "sec overdrawn";
            }
            else if (result1 < minTime)
            {
                TimeSpan difference = result1 - minTime;
                timeSelectedSession.Foreground = new SolidColorBrush(Colors.Red);
                tipTimeSelectedSession.Text = Math.Abs(difference.TotalSeconds).ToString() + "sec short";
            }
            else
            {
                timeSelectedSession.Foreground = new SolidColorBrush(Colors.LimeGreen);
                tipTimeSelectedSession.Text = "Goal achieved";
            }
            timeSelectedSession.Text = result1.ToString("mm\\:ss") + " min";


            // Decides what is shown
            List<Session> sessionsResult2;
            if (configuration == null)
            {
                sessionsResult2 = ProcessedSessionsDataHelper.GetLastSessions(sessions, DEFAULT_NUMBER_OF_SESSIONS);
                LastXSession.Text = "Last 7 Sessions";
            }
            else
                if (configuration.IsLastSessions)
            {
                sessionsResult2 = ProcessedSessionsDataHelper.GetLastSessions(sessions, configuration.LastDaysOrSessions);
                LastXSession.Text = "Last " + configuration.LastDaysOrSessions + " Sessions";
            }
            else
            {
                sessionsResult2 = ProcessedSessionsDataHelper.GetSessionsOfLastDays(sessions, configuration.LastDaysOrSessions);
                LastXSession.Text = "Last " + configuration.LastDaysOrSessions + " Days";
            }


            if (sessionsResult2.Count == 0)
                return;

            int index = 0;
            TimeSpan tempResult2 = new TimeSpan(0);
            foreach (var session in sessionsResult2)
            {
                tempResult2 += session.Duration;
                index++;
            }

            TimeSpan result2 = new TimeSpan(tempResult2.Ticks / index);
            timeLastXSession.Text = result2.ToString("mm\\:ss") + " min";

            // Decides what is shown
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

        private void PlotCandleActions()
        {
            var actions = processedSessionsData.GetSelectedSession().Actions;
            var durationOfSelectedSession = processedSessionsData.GetSelectedSession().Duration;
            WpfPlot plot = (WpfPlot)FindName("CandleActions");

            string[] xLabels = new string[5];
            TimeSpan fractionDuration = TimeSpan.FromTicks(durationOfSelectedSession.Ticks / 4);
            // Display time in equal fractions
            for (int i = 0; i <= 4; i++)
            {
                TimeSpan fraction = TimeSpan.FromTicks(fractionDuration.Ticks * i);
                xLabels[i] = fraction.ToString("mm\\:ss");
            }

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
            double[] xPositions = { -0.5 ,(int)actions.Count * 1 / 4, (int)actions.Count * 1 / 2, (int)actions.Count * 3 / 4, actions.Count }; // postion of x labels
            plot.Plot.XAxis.ManualTickPositions(xPositions, xLabels);
            plot.Plot.YAxis.Ticks(false);
            plot.Plot.Title("Distribution of detected Actions");
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Plot.Legend();
            plot.Refresh();
        }

        private void PlotCandleSentences()
        {
            var sentences = processedSessionsData.GetSelectedSession().Sentences;
            var durationOfSelectedSession = processedSessionsData.GetSelectedSession().Duration;
            WpfPlot plot = (WpfPlot)FindName("CandleSentences");

            string[] xLabels = new string[5];
            TimeSpan fractionDuration = TimeSpan.FromTicks(durationOfSelectedSession.Ticks / 4);
            // Display time in equal fractions
            for (int i = 0; i <= 4; i++)
            {
                TimeSpan fraction = TimeSpan.FromTicks(fractionDuration.Ticks * i);
                xLabels[i] = fraction.ToString("mm\\:ss");
            }

            int index = 0;
            foreach (var value in sentences)
            {

                if (value.WasIdentified)
                {
                    var lolipop = plot.Plot.AddLollipop(
                                       values: new double[] { 10 },
                                       positions: new double[] { index },
                                       color: Color.Green);

                }
                else
                {
                    var lolipop = plot.Plot.AddLollipop(
                                       values: new double[] { 10 },
                                       positions: new double[] { index },
                                       color: Color.Red);
                }
                index++;
            }
            // var lolipop = plot.Plot.AddLollipop(values);
            double[] xPositions = { -0.5, (int)sentences.Count * 1 / 4, (int)sentences.Count * 1 / 2, (int)sentences.Count * 3 / 4, sentences.Count-0.5 };
            plot.Plot.XAxis.ManualTickPositions(xPositions, xLabels);
            plot.Plot.YAxis.Ticks(false);
            plot.Plot.Title("Distribution of Sentences");
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Plot.Legend();
            plot.Refresh();
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

            // remove elements with zero count
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

        /// <summary>
        /// Plots a donutchart that shows the percantage of identified sentences in the last session.
        /// </summary>
        private void PlotPercentageOfIdentifiedSentencesInSelectedSession()
        {
            double percentageOfRecongnisedSentences = processedSessionsData.GetPercentageOfIdentifiedFromSelectedSession();

            if (double.IsNaN(percentageOfRecongnisedSentences))
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


            WpfPlot plot = (WpfPlot)FindName("DonutForCard");
            var pie = plot.Plot.AddPie(new double[] { percentageOfRecongnisedSentences, 100 - percentageOfRecongnisedSentences });

            // Chart Configuration
            pie.DonutSize = .7;
            pie.Size = 0.8;
            pie.DonutLabel = percentageOfRecongnisedSentences.ToString() + "%";
            pie.CenterFont.Color = prograssColor;
            pie.CenterFont.Size = 24f;
            pie.OutlineSize = 0.7f;
            pie.SliceFillColors = new Color[] { prograssColor, Color.LightGray };
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
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
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
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

        private void ActionVideoButtonClicked(object sender, RoutedEventArgs e)
        {
            VideoPlayerWindow window = new VideoPlayerWindow(true);
            window.Show();
        }

        private void SentenceVideoButtonClicked(object sender, RoutedEventArgs e)
        {
            VideoPlayerWindow window = new VideoPlayerWindow(false);
            window.Show();
        }
    }
}
