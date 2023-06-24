using PresentationTrainerVisualization.Helper;
using System.Windows.Controls;

namespace PresentationTrainerVisualization.DashboardComponents.Progress
{
    public partial class CardNumberOfSessions : UserControl
    {
        private ProcessedSessions processedSessions;

        public CardNumberOfSessions()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();

            PlotCard();
        }

        private void PlotCard()
        {
            TextBlock text = (TextBlock)FindName("NumberOfSessions");
            text.Text = processedSessions.GetNumberOfSessions().ToString();
        }
    }
}
