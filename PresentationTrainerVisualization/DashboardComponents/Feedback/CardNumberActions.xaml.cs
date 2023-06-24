using PresentationTrainerVisualization.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace PresentationTrainerVisualization.DashboardComponents.Feedback
{
    public partial class CardNumberActions : UserControl
    {
        private ProcessedSessions processedSessions;
        private ProcessedGoals processedGoals;

        public CardNumberActions()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();
            processedGoals = new ProcessedGoals();

            PlotNumberOfActions();
        }

        /// <summary>
        /// Plot Number of Actions that were not labeled as mistake.
        /// </summary>
        private void PlotNumberOfActions()
        {
            List<string> selectedGoalsActions = processedGoals.GetSelectedActionsLog();

            NumberOfGoodActions.Text = (from action in processedSessions.SelectedSession.Actions
                                        where action.Mistake == false == true && selectedGoalsActions.Contains(action.LogAction)
                                        select action).Count().ToString();
        }
    }
}
