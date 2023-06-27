using PresentationTrainerVisualization.Helper;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Color = System.Drawing.Color;


namespace PresentationTrainerVisualization.Pages
{
    public partial class FeedbackDashboard : Page
    {
        private ProcessedSessions processedSessions { get; }

        public FeedbackDashboard()
        {
            // In each dashboard component this instance is used. Has to be created here.
            processedSessions = new ProcessedSessions();

            // Has to be last
            InitializeComponent(); 
        }



        // Not used currently.

        private void PlotStackedBarChart()
        {
            WpfPlot plot = (WpfPlot)FindName("TestPlot3");


            var result = processedSessions.GetNumberOfRightAndWrongSentencesBySession();

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

            var result = processedSessions.GetNumberOfRightAndWrongSentencesBySession();

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

            var result = processedSessions.GetNumberOfRightAndWrongSentencesByLevel();

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

            var resultData = processedSessions.GetNumberOfRightAndWrongSentencesByLevel();

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
