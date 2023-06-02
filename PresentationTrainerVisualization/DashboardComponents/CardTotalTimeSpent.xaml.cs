using PresentationTrainerVisualization.helper;
using System.Windows.Controls;

namespace PresentationTrainerVisualization.DashboardComponents
{
   
    public partial class CardTotalTimeSpent : UserControl
    {
        private ProcessedSessionsData processedSessionsData;

        public CardTotalTimeSpent()
        {
            InitializeComponent();
            processedSessionsData = new ProcessedSessionsData();

            PlotCard();
        }

        private void PlotCard()
        {
            TextBlock text = (TextBlock)FindName("TotalTimeSpent");
            text.Text = processedSessionsData.GetTotalTimeSpent().ToString("hh\\:mm") + " Hours";
        }
    }
}
