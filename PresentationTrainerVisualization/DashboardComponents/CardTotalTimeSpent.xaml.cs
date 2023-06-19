using PresentationTrainerVisualization.helper;
using System.Windows.Controls;

namespace PresentationTrainerVisualization.DashboardComponents
{
   
    public partial class CardTotalTimeSpent : UserControl
    {
        private ProcessedSessions processedSessionsData;

        public CardTotalTimeSpent()
        {
            InitializeComponent();
            processedSessionsData = new ProcessedSessions();

            PlotCard();
        }

        private void PlotCard()
        {
            TextBlock text = (TextBlock)FindName("TotalTimeSpent");
            text.Text = processedSessionsData.GetTotalTimeSpent().ToString("hh\\:mm") + " Hours";
        }
    }
}
