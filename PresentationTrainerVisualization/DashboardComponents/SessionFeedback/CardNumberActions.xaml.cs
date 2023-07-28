using PresentationTrainerVisualization.DashboardComponents.Progress;
using PresentationTrainerVisualization.Helper;
using PresentationTrainerVisualization.models;
using PresentationTrainerVisualization.models.json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace PresentationTrainerVisualization.DashboardComponents.SessionFeedback
{
    public partial class CardNumberActions : UserControl
    {
        private ProcessedSessions processedSessions;
        private ProcessedGoals processedGoals;
        private ProcessedConfigurations processedConfigurations;


        public CardNumberActions()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();
            processedGoals = ProcessedGoals.GetInstance();
            processedConfigurations = ProcessedConfigurations.GetInstace();


            PlotNumberOfActions();
        }

        /// <summary>
        /// Plot Number of Actions that were not labeled as mistake.
        /// </summary>
        private void PlotNumberOfActions()
        {
            List<string> selectedGoalsActions = processedGoals.GetSelectedActionsLog();
            int numberOfGoodActions = (from action in processedSessions.SelectedSession.Actions
                                      where action.Mistake == false && selectedGoalsActions.Contains(action.LogAction)
                                      select action).Count();

            NumberOfGoodActions.Text = numberOfGoodActions.ToString();


            Configuration configuration = processedConfigurations.ConfigurationLastDays;
            List<AggregatedSession> actions = processedSessions.GetActionsBySession(false);
            double percentageResult;

            if (configuration.CompareWithLastSessions)
            {
                AggregatedSession averageSession = ProcessedSessionsHelper.GetAverageOfLastSessions(actions, configuration.NumberOfSessions);
                if (averageSession.AggregatedObjects.Count == 0)
                    return;

                double averageActions = averageSession.AggregatedObjects.First().Count;
                percentageResult = Math.Round((numberOfGoodActions - averageActions) / numberOfGoodActions * 100, 1);               
            }
            else
            {
                AggregatedSession averageSession = ProcessedSessionsHelper.GetAverageOfLastDays(actions, configuration.NumberOfSessions);

                if (averageSession.AggregatedObjects.Count == 0)
                    return;

                double averageActions = averageSession.AggregatedObjects.First().Count;
                percentageResult = Math.Round((numberOfGoodActions - averageActions) / numberOfGoodActions * 100, 1);
            }


            if (percentageResult >= 0)
            {
                Percentage.Foreground = new SolidColorBrush(Constants.GOOD_INDICATOR_COLOR_MEDIA);
                Percentage.Text = "▲ +" + percentageResult.ToString() + "%";
            }
            else
            {
                Percentage.Foreground = new SolidColorBrush(Constants.BAD_INDICATOR_COLOR_MEDIA);
                Percentage.Text = "▼ " + percentageResult.ToString() + "%";
            }

        }
    }
}
