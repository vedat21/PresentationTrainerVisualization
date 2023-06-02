using PresentationTrainerVisualization.helper;
using PresentationTrainerVisualization.models.json;
using PresentationTrainerVisualization.models;
using ScottPlot;
using System.Collections.Generic;
using System.Windows.Controls;
using static PresentationTrainerVisualization.helper.Constants;
using Color = System.Drawing.Color;


namespace PresentationTrainerVisualization.DashboardComponents
{

    public partial class GoalMinNumberOfActions : UserControl
    {
        private ProcessedSessionsData processedSessionsData;
        private ProcessedGoalsData processedGoalsData;

        public GoalMinNumberOfActions()
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

            List<AggregatedSession> result = processedSessionsData.GetBadActionsBySession(false);
            List<double> stack1 = new List<double>();
            List<double> stack2 = new List<double>();

            foreach (var aggregatedSession in result)
            {
                double numberOfActions = aggregatedSession.AggregatedObjects[0].Count;

                if (numberOfActions > max)
                {
                    stack1.Add(max);
                    stack2.Add(numberOfActions - max);
                }
                else
                {
                    stack1.Add(numberOfActions);
                    stack2.Add(0);
                }
            }

            double[] stack2New = new double[stack2.Count];
            for (int i = 0; i < stack2.Count; i++)
                stack2New[i] = stack1[i] + stack2[i];

            WpfPlot plot = (WpfPlot)FindName("GoalPlot");
            var bar1 = plot.Plot.AddBar(stack2New);
            bar1.Color = Color.Red;
            var bar2 = plot.Plot.AddBar(stack1.ToArray());
            bar2.Color = Color.SkyBlue;

            plot.Plot.SetAxisLimits(yMin: 0);
            plot.Plot.Frameless();
            plot.Refresh();
        }
    }
}
