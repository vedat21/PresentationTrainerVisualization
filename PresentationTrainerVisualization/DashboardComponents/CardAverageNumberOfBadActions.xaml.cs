using PresentationTrainerVisualization.helper;
using System.Windows.Controls;

namespace PresentationTrainerVisualization.DashboardComponents
{
    public partial class CardAverageNumberOfBadActions : UserControl
    {
        private ProcessedSessions processedSessionsData;

        public CardAverageNumberOfBadActions()
        {
            InitializeComponent();
            processedSessionsData = new ProcessedSessions();

            PlotCard();
        }

        private void PlotCard()
        {
            TextBlock text = (TextBlock)FindName("NumberOfBadActions");
            text.Text = processedSessionsData.GetAverageNumberOfBadActions().ToString();
        }
    }
}
