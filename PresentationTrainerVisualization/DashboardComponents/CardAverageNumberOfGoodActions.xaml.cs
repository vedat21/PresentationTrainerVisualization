using PresentationTrainerVisualization.helper;
using System.Windows.Controls;

namespace PresentationTrainerVisualization.DashboardComponents
{
    /// <summary>
    /// Interaktionslogik für CardAverageNumberOfGoodActions.xaml
    /// </summary>
    public partial class CardAverageNumberOfGoodActions : UserControl
    {

        private ProcessedSessions processedSessionsData;

        public CardAverageNumberOfGoodActions()
        {
            InitializeComponent();
            processedSessionsData = new ProcessedSessions();
            PlotCard();
        }

        private void PlotCard()
        {
            TextBlock text = (TextBlock)FindName("NumberOfGoodActions");
            text.Text = processedSessionsData.GetAverageNumberOfGoodActions().ToString();
        }
    }
}
