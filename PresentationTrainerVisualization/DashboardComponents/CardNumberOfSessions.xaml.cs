using PresentationTrainerVisualization.helper;
using System.Windows.Controls;

namespace PresentationTrainerVisualization.DashboardComponents
{
    /// <summary>
    /// Interaktionslogik für CardNumberOfSessions.xaml
    /// </summary>
    public partial class CardNumberOfSessions : UserControl
    {
        private ProcessedSessionsData processedSessionsData;

        public CardNumberOfSessions()
        {
            InitializeComponent();
            processedSessionsData = new ProcessedSessionsData();

            PlotCard();
        }

        private void PlotCard()
        {
            TextBlock text = (TextBlock)FindName("NumberOfSessions");
            text.Text = processedSessionsData.GetNumberOfSessionsByTime().ToString();
        }
    }
}
