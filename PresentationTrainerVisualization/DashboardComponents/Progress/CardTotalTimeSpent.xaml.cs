using PresentationTrainerVisualization.Helper;
using System.Windows.Controls;


namespace PresentationTrainerVisualization.DashboardComponents.Progress
{
    public partial class CardTotalTimeSpent : UserControl
    {
        private ProcessedSessions processedSessionsData;

        public CardTotalTimeSpent()
        {
            InitializeComponent();
            processedSessionsData = ProcessedSessions.GetInstance();

            PlotCard();
        }

        private void PlotCard()
        {
            TextBlock text = (TextBlock)FindName("TotalTimeSpent");
            text.Text = processedSessionsData.GetTotalTimeSpent().ToString("hh\\:mm") + " Hours";
        }
    }
}
