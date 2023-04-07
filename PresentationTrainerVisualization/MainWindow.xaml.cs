using Microsoft.VisualBasic;
using Microsoft.Win32;
using Newtonsoft.Json;
using PresentationTrainerVisualization.helper;
using PresentationTrainerVisualization.models;
using ScottPlot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;
using static PresentationTrainerVisualization.Window1;

namespace PresentationTrainerVisualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProcessData processData;

        public MainWindow()
        {
            InitializeComponent();
            processData = new ProcessData();

            //     PlotDoughnutChart(processData.GetNumberOfRightAndWrongSentences());
        //    PlotBarChartWithLabels(processData.GetNumberOfRightAndWrongSentences());
            PlotTimeLineChart();
            PlotStackedBarChart();
            PlotGroupedBarChart();
            PlotDonutChartWithText();
            PlotGroupedBarChartByLevel();
            PlotRadarChart();
        }



        public void PlotTimeLineChart()
        {
            var numberOfSessions = processData.GetNumberOfSessionsByDate();

            // Convert DateTime[] to double[] before plotting
            double[] xs = numberOfSessions.Keys.Select(x => x.ToDateTime(TimeOnly.Parse("00:00 PM")).ToOADate()).ToArray();
            double[] ys = numberOfSessions.Values.Select(x => (double)x).ToArray();
            Array.Sort(xs, ys);

            TestPlot2.Plot.AddScatter(xs, ys);
            TestPlot2.Plot.XAxis.DateTimeFormat(true);
            TestPlot2.Plot.Title("Number of Sessions");
            TestPlot2.Plot.SetAxisLimits(yMin: 0);


            TestPlot2.Refresh();

        }


        public void PlotBarChartWithLabels(List<double> Data)
        {
            List<string> DataLabels = new List<string>();
            DataLabels.Add("Right Sentences");
            DataLabels.Add("Wrong Sentences");

            // Barchart needs array of double
            double[] positions = Array.ConvertAll(Enumerable.Range(0, Data.Count).ToArray(), x => (double)x);

            var bar = TestPlot1.Plot.AddBar(Data.ToArray(), positions);
            TestPlot1.Plot.XTicks(positions, DataLabels.ToArray());
            TestPlot1.Plot.SetAxisLimits(yMin: 0);

            TestPlot1.Plot.Title("Number of Sentences that were identified");
            TestPlot1.Plot.Legend();
            TestPlot1.Refresh();
        }

        public void PlotDoughnutChart(List<double> Data)
        {
            List<string> DataLabels = new List<string>();
            DataLabels.Add("Right Sentences");
            DataLabels.Add("Wrong Sentences");
            Color[] sliceColors =
                {
                   ColorTranslator.FromHtml("#e01b1b") ,
                   ColorTranslator.FromHtml("#1be04c"),

                };

            var pie = TestPlot.Plot.AddPie(Data.ToArray());
            pie.ShowValues = true;
            pie.SliceLabels = DataLabels.ToArray();
            pie.SliceFillColors = sliceColors;

            //     TestPlot.Plot.Title("Number of Sentences that were identified");
            TestPlot.Plot.Legend();
            TestPlot.Refresh();
        }



        public void PlotStackedBarChart()
        {

            var result = processData.GetNumberOfRightAndWrongSentencesBySession();

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
                var bar = TestPlot3.Plot.AddBar(data[i]);
                bar.Label = "Session " + (i + 1);
            }

            TestPlot3.Plot.XTicks(positions, DataLabels.ToArray());
            TestPlot3.Plot.Legend(location: Alignment.UpperRight);
            TestPlot3.Plot.SetAxisLimits(yMin: 0);
            TestPlot3.Plot.Title("Sentence by Session");

            TestPlot3.Refresh();
        }


        public void PlotGroupedBarChart()
        {
            string[] seriesNames = { "Right", "Wrong" };
            string[] groupNames = { "Session 1", "Session 2", "Session 3", "Session 4", "Session 5", "Session 6", "Session 7", "Session 8", "Session 9", "Session 10", "Session 11" };

            var result = processData.GetNumberOfRightAndWrongSentencesBySession();

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
            {
                fakeErrorData[i] = new double[transpondedData[i].Length];
            }

            var bar = TestPlot4.Plot.AddBarGroups(groupNames, seriesNames, transpondedData, fakeErrorData);
            bar[0].ShowValuesAboveBars = true; bar[1].ShowValuesAboveBars = true;
            TestPlot4.Plot.Legend(location: Alignment.UpperRight);
            TestPlot4.Plot.Title("Sentence by Session");
            TestPlot4.Refresh();

        }

        public void PlotGroupedBarChartByLevel()
        {

            var result = processData.GetNumberOfRightAndWrongSentencesByLevel();

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



            var bar = TestPlot5.Plot.AddBarGroups(groupNames.ToArray(), seriesNames, transpondedData, fakeErrorData);
            TestPlot5.Plot.Legend(location: Alignment.UpperRight);
            TestPlot5.Plot.Title("Sentence by Level");
            bar[0].ShowValuesAboveBars = true; bar[1].ShowValuesAboveBars = true;
            bar[0].Color = Color.DeepSkyBlue; bar[1].Color = Color.IndianRed;
            TestPlot5.Refresh();
        }


        public void PlotDonutChartWithText()
        {

            var data = processData.GetNumberOfRightAndWrongSentences();

            double[] values = data.ToArray();
            string centerText = $"{values[0] / values.Sum() * 100:00.0}%";
            Color color1 = Color.FromArgb(255, 0, 150, 200);
            Color color2 = Color.FromArgb(100, 0, 150, 200);

            var pie = TestPlot.Plot.AddPie(values);
            pie.DonutSize = .65;
            pie.DonutLabel = centerText;
            pie.CenterFont.Color = color1;
            pie.OutlineSize = 1;
            pie.SliceFillColors = new Color[] { color1, color2 };

            TestPlot.ToolTip = "Percentage of Sentences that were recognized.";
            TestPlot.Plot.Title("Recongized Sentence", size: 13f);

            TestPlot.Refresh();

        }

        public void PlotRadarChart()
        {
            var result = processData.GetNumberOfRightAndWrongSentencesByLevel();

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

            var radar = TestPlot1.Plot.AddRadar(ImperativeConvert(transpondedData));
            radar.CategoryLabels = groupNames.ToArray();
            radar.GroupLabels = seriesNames;
            radar.ShowAxisValues = false;

            // customize the plot
            TestPlot1.Plot.Title("Recongnized Sentences by Level");
            TestPlot1.Plot.Legend();

            TestPlot1.Refresh();
        }

        public void PlotRadarChartWithIndependentAxes()
        {
            double[,] values = {
                { 5, 3, 10, 15, 3, 2 },
                { 5, 2, 10, 10, 1, 4 },
            };



            var radar = TestPlot2.Plot.AddRadar(values);
            radar.CategoryLabels = new string[] { "Wins", "Poles", "Podiums", "Points Finishes", "DNFs", "Fastest Laps" };
            radar.GroupLabels = new[] { "Sebastian Vettel", "Fernando Alonso" };
            //     radar.ShowAxisValues = false;

            // customize the plot
            TestPlot2.Plot.Title("2010 Formula One World Championship");
            TestPlot2.Plot.Legend();

            TestPlot2.Refresh();
        }

        /**
         * Author: https://highfieldtales.wordpress.com/2013/08/17/convert-a-jagged-array-into-a-2d-array/
         */
        static double[,] ImperativeConvert(double[][] source)
        {
            double[,] result = new double[source.Length, source[0].Length];

            for (int i = 0; i < source.Length; i++)            
                for (int k = 0; k < source[0].Length; k++)
                    result[i, k] = source[i][k];

            return result;
        }


    }
}
