using PresentationTrainerVisualization.DashboardComponents.Progress;
using PresentationTrainerVisualization.Helper;
using PresentationTrainerVisualization.models;
using PresentationTrainerVisualization.models.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace PresentationTrainerVisualization.DashboardComponents.Feedback
{
    public partial class CardNumberMistakeActions : UserControl
    {
        private ProcessedSessions processedSessions;
        private ProcessedGoals processedGoals;
        private ProcessedConfigurations processedConfigurations;

        public CardNumberMistakeActions()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();
            processedGoals = ProcessedGoals.GetInstance();
            processedConfigurations = ProcessedConfigurations.GetInstace();

            PlotNumberOfMistakeActions();
        }

        /// <summary>
        /// Plot Number of Actions that were labeled as mistake.
        /// </summary>
        private void PlotNumberOfMistakeActions()
        {
            List<string> selectedGoalsActions = processedGoals.GetSelectedActionsLog();
            int numberOfBadActions = (from action in processedSessions.SelectedSession.Actions
                                      where action.Mistake == true && selectedGoalsActions.Contains(action.LogAction)
                                      select action).Count();

            NumberOfBadActions.Text = numberOfBadActions.ToString();


            Configuration configuration = processedConfigurations.ConfigurationLastDays;
            List<AggregatedSession> actions = processedSessions.GetActionsBySession(true);
            double percentageResult;

            if (configuration.CompareWithLastSessions)
            {
                AggregatedSession averageSession = ProcessedSessionsHelper.GetAverageOfLastSessions(actions, configuration.NumberOfSessions);
                if (averageSession.AggregatedObjects.Count == 0)
                    return;

                double averageActions = averageSession.AggregatedObjects.First().Count;
                percentageResult = Math.Round((numberOfBadActions - averageActions) / numberOfBadActions * 100, 1);
            }
            else
            {
                AggregatedSession averageSession = ProcessedSessionsHelper.GetAverageOfLastDays(actions, configuration.NumberOfSessions);

                if (averageSession.AggregatedObjects.Count == 0)
                    return;

                double averageActions = averageSession.AggregatedObjects.First().Count;
                percentageResult = Math.Round((numberOfBadActions - averageActions) / numberOfBadActions * 100, 1);
            }


            if (percentageResult <= 0)
            {
                Percentage.Foreground = new SolidColorBrush(Constants.GOOD_INDICATOR_COLOR_MEDIA);
                Percentage.Text = "▲ " + percentageResult.ToString() + "%";
            }
            else
            {
                Percentage.Foreground = new SolidColorBrush(Constants.BAD_INDICATOR_COLOR_MEDIA);
                Percentage.Text = "▼ +" + percentageResult.ToString() + "%";
            }


        }
    }
}
