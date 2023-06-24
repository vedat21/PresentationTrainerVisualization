using PresentationTrainerVisualization.Helper;
using ScottPlot;
using System.Windows.Controls;
using Color = System.Drawing.Color;

namespace PresentationTrainerVisualization.DashboardComponents.Feedback
{
    public partial class CardIdentifiedSentences : UserControl
    {
        private ProcessedSessions processedSessions;

        public CardIdentifiedSentences()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();

            PlotPercentageOfIdentifiedSentences();
        }

        /// <summary>
        /// Plots a donutchart that shows the percantage of identified sentences in the last session.
        /// </summary>
        private void PlotPercentageOfIdentifiedSentences()
        {
            double percentageOfRecongnisedSentences = processedSessions.GetPercentageOfIdentifiedFromSelectedSession();

            if (double.IsNaN(percentageOfRecongnisedSentences))
                return;

            Color prograssColor;
            if (percentageOfRecongnisedSentences < 25)
                prograssColor = Constants.BAD_INDICATOR_COLOR;
            else if (percentageOfRecongnisedSentences < 75 && percentageOfRecongnisedSentences >= 25)
                prograssColor = Color.Orange;
            else
                prograssColor = Constants.GOOD_INDICATOR_COLOR;


            WpfPlot plot = (WpfPlot)FindName("DonutForCard");
            var pie = plot.Plot.AddPie(new double[] { percentageOfRecongnisedSentences, 100 - percentageOfRecongnisedSentences });

            // Chart Configuration
            pie.DonutSize = .7;
            pie.Size = 0.8;
            pie.DonutLabel = percentageOfRecongnisedSentences.ToString() + "%";
            pie.CenterFont.Color = prograssColor;
            pie.CenterFont.Size = 24f;
            pie.OutlineSize = 0.7f;
            pie.SliceFillColors = new Color[] { prograssColor, Color.LightGray };
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Refresh();
        }

    }
}
