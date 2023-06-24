using PresentationTrainerVisualization.Helper;
using System.Windows.Controls;


namespace PresentationTrainerVisualization.DashboardComponents.Progress
{
    public partial class CardAverageNumberOfGoodActions : UserControl
    {
        private ProcessedSessions processedSessions;

        public CardAverageNumberOfGoodActions()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();

            PlotCard();
        }

        private void PlotCard()
        {
            TextBlock text = (TextBlock)FindName("NumberOfGoodActions");
            text.Text = processedSessions.GetAverageNumberOfGoodActions().ToString();
        }
    }
}
