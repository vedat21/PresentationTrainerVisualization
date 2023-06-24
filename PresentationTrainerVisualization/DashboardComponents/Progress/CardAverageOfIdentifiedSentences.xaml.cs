using PresentationTrainerVisualization.Helper;
using ScottPlot;
using System;
using System.Windows.Controls;
using Color = System.Drawing.Color;


namespace PresentationTrainerVisualization.DashboardComponents.Progress
{
    public partial class CardAverageOfIdentifiedSentences : UserControl
    {
        private ProcessedSessions processedSessions;

        public CardAverageOfIdentifiedSentences()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();

            PlotCard();
        }

        private void PlotCard()
        {
            double percentageOfRecongnisedSentences = processedSessions.GetAverageNumberOfIdentifiedentences();

            if (Double.IsNaN(percentageOfRecongnisedSentences))
                return;

            Color prograssColor;
            if (percentageOfRecongnisedSentences < 25)
                prograssColor = Constants.BAD_INDICATOR_COLOR;
            else if (percentageOfRecongnisedSentences < 75 && percentageOfRecongnisedSentences >= 25)
                prograssColor = Color.Orange;
            else
                prograssColor = Constants.GOOD_INDICATOR_COLOR;


            WpfPlot plot = (WpfPlot)FindName("PlotAverageOfIdentifiedSentences");
            var pie = plot.Plot.AddPie(new double[] { percentageOfRecongnisedSentences, 100 - percentageOfRecongnisedSentences });

            // Chart Configuration
            pie.DonutSize = .7;
            pie.Size = 0.8;
            pie.DonutLabel = percentageOfRecongnisedSentences.ToString() + "%";
            pie.CenterFont.Size = 20f; // size of text inside donut
            pie.CenterFont.Color = prograssColor;
            pie.OutlineSize = 0.9f;
            pie.SliceFillColors = new Color[] { prograssColor, Color.LightGray };
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Refresh();
        }
    }
}
