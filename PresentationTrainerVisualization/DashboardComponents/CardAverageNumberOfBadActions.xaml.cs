using PresentationTrainerVisualization.helper;
using System.Windows.Controls;

namespace PresentationTrainerVisualization.DashboardComponents
{
    public partial class CardAverageNumberOfBadActions : UserControl
    {
        private ProcessedSessionsData processedSessionsData;

        public CardAverageNumberOfBadActions()
        {
            InitializeComponent();
            processedSessionsData = new ProcessedSessionsData();

            PlotCard();
        }

        private void PlotCard()
        {
            TextBlock text = (TextBlock)FindName("NumberOfBadActions");
            text.Text = processedSessionsData.GetAverageNumberOfBadActions().ToString();
        }
    }
}
