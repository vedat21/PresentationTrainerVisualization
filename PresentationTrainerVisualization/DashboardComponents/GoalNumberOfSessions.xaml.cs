using PresentationTrainerVisualization.helper;
using PresentationTrainerVisualization.models.json;
using ScottPlot;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Color = System.Drawing.Color;


namespace PresentationTrainerVisualization.DashboardComponents
{
    public partial class GoalNumberOfSessions : UserControl
    {

        private ProcessedSessions processedSessionsData;
        private ProcessedGoals processedGoalsData;

        public GoalNumberOfSessions()
        {
            InitializeComponent();
            processedSessionsData = new ProcessedSessions();
            processedGoalsData = new ProcessedGoals();

            PlotGoalNumberOfSessionsInDays();
        }

        public void PlotGoalNumberOfSessionsInDays()
        {
            Goal goal = processedGoalsData.GetGoal(Constants.GoalsLabel.NumberOfSessionsForDays.ToString());

            if (goal == null)
            {
                GoalNumber.Text = "Set Goal to display";
                return;
            }


            double numberOfDaysLeft = (goal.StartDate.AddDays(int.Parse(goal.Description[Constants.GoalsDescription.number_of_days.ToString()])) - DateTime.Today).TotalDays;
            int numberOfSessionsGoals = int.Parse(goal.Description[Constants.GoalsDescription.number_of_sessions.ToString()]);
            var numberOfSessionsToday = 3; //    var numberOfSessionsToday = processedSessionsData.GetNumberOfSessionsToday();
            int numberOfSessionsMissingToday = numberOfSessionsGoals - numberOfSessionsToday;

            // Decide text and style based on data.
            if (numberOfSessionsMissingToday > 0)
            {
                GoalNumber.Text = numberOfSessionsMissingToday + (numberOfSessionsMissingToday == 1 ? " Session" : " Sessions");
                GoalNumber.Foreground = new SolidColorBrush(Colors.Red);
                GoalText.Text = "Missing for today";
            }
            else  //if (numberOfSessionsMissingToday == 0)
            {
                GoalNumber.Text = "Completed";
                GoalNumber.Foreground = new SolidColorBrush(Colors.Green);
                GoalText.Text = "Goal Achieved";
            }
            /*
            else
            {
                GoalNumber.Text = Math.Abs(numberOfSessionsMissingToday) + (numberOfSessionsMissingToday == -1 ? " Session" : " Sessions");
                GoalNumber.Foreground = new SolidColorBrush(Colors.Green);
                GoalText.Text = "More than Target";
            }
            */

            if (numberOfDaysLeft > 0)
                GoalDays.Text = numberOfDaysLeft + " days left";
            else if (numberOfDaysLeft == 0)
                GoalDays.Text = "Final day";
            else
                GoalDays.Text = "Goal finished";

            double result = Math.Min((double)numberOfSessionsToday / (double)numberOfSessionsGoals * 100, 100);
            result = Math.Round(result, 1);
            Color prograssColor;

            if (result < 25)
                prograssColor = Constants.BAD_INDICATOR_COLOR;
            else if (result < 70 && result >= 25)
                prograssColor = Color.Orange;
            else
                prograssColor = Constants.GOOD_INDICATOR_COLOR;

            WpfPlot plot = (WpfPlot)FindName("GoalPlot");
            var pie = plot.Plot.AddPie(new double[] { result, 100 - result });
            pie.DonutSize = .7;
            pie.DonutLabel = result + "%";
            pie.CenterFont.Color = prograssColor;
            pie.OutlineSize = 0.6f;
            pie.SliceFillColors = new Color[] { prograssColor, Color.LightGray };
            pie.CenterFont.Size = 15f;
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Refresh();


            /*
            // Plot Barchart
            var numberOfSessionsByDate = processedData.GetNumberOfSessionsByDate();
            double[] values = numberOfSessionsByDate.Values.Select(x => (double)x).ToArray();

            Func<double, string> customFormatter = y => y == values.Last() ? y.ToString() : ""; // to only show value of last bar
            WpfPlot plot = (WpfPlot)FindName("BarForCard");
            var bar = plot.Plot.AddBar(values);
            bar.BarWidth = 0.4;
            bar.ShowValuesAboveBars = true;
            bar.ValueFormatter = customFormatter;
            plot.Plot.SetAxisLimits(yMin: 0);
            plot.Plot.Frameless();
            plot.Refresh();
            */
        }
    }
}
