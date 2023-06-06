﻿using PresentationTrainerVisualization.helper;
using PresentationTrainerVisualization.models;
using PresentationTrainerVisualization.models.json;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static PresentationTrainerVisualization.helper.Constants;
using Color = System.Drawing.Color;


namespace PresentationTrainerVisualization.DashboardComponents
{
    public partial class GoalMaxNumberOfBadActions : UserControl
    {

        private ProcessedSessionsData processedSessionsData;
        private ProcessedGoalsData processedGoalsData;

        public GoalMaxNumberOfBadActions()
        {
            InitializeComponent();
            processedSessionsData = new ProcessedSessionsData();
            processedGoalsData = new ProcessedGoalsData();

            PlotGoalChart();
        }

        private void PlotGoalChart()
        {
            Goal goal = processedGoalsData.GetGoalWithLabel(GoalsLabel.MaxNumberOfBadActions.ToString());
            int max = int.Parse(goal.Description[GoalsDescription.max_number_of_bad_actions.ToString()]);

            List<AggregatedSession> result = processedSessionsData.GetBadActionsBySession();
            List<double> result1 = new List<double>();
            List<double> result2 = new List<double>();

            foreach (var aggregatedSession in result)
            {
                double numberOfActions = aggregatedSession.AggregatedObjects[0].Count;

                if (numberOfActions > max)
                {
                    result1.Add(max);
                    result2.Add(numberOfActions - max);
                }
                else
                {
                    result1.Add(numberOfActions);
                    result2.Add(0);
                }
            }

            double[] stack2New = new double[result2.Count];
            for (int i = 0; i < result2.Count; i++)
                stack2New[i] = result1[i] + result2[i];

            WpfPlot plot = (WpfPlot)FindName("GoalPlot");
            var bar1 = plot.Plot.AddBar(stack2New);
            bar1.Color = Color.Red;
            var bar2 = plot.Plot.AddBar(result1.ToArray());
            bar2.Color = Color.SkyBlue;

            plot.Plot.SetAxisLimits(yMin: 0);
            plot.Plot.Frameless();
            plot.Refresh();
        }
    }
}
