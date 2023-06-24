using PresentationTrainerVisualization.Helper;
using System.Windows.Controls;


namespace PresentationTrainerVisualization.DashboardComponents.Progress
{
    public partial class CardAverageNumberOfBadActions : UserControl
    {
        private ProcessedSessions processedSessions;

        public CardAverageNumberOfBadActions()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();

            PlotCard();
        }

        private void PlotCard()
        {
            TextBlock text = (TextBlock)FindName("NumberOfBadActions");
            text.Text = processedSessions.GetAverageNumberOfBadActions().ToString();
        }
    }
}
