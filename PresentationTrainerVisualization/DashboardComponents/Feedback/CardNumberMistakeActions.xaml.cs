using PresentationTrainerVisualization.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace PresentationTrainerVisualization.DashboardComponents.Feedback
{
    public partial class CardNumberMistakeActions : UserControl
    {
        private ProcessedSessions processedSessions;
        private ProcessedGoals processedGoals;

        public CardNumberMistakeActions()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();
            processedGoals = ProcessedGoals.GetInstance();

            PlotNumberOfMistakeActions();
        }

        /// <summary>
        /// Plot Number of Actions that were labeled as mistake.
        /// </summary>
        private void PlotNumberOfMistakeActions()
        {
            List<string> selectedGoalsActions = processedGoals.GetSelectedActionsLog();

            NumberOfBadActions.Text = (from action in processedSessions.SelectedSession.Actions
                                       where action.Mistake == true && selectedGoalsActions.Contains(action.LogAction)
                                       select action).Count().ToString();
        }
    }
}
