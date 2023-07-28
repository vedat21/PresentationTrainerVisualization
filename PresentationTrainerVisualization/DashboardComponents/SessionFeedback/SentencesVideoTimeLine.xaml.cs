using PresentationTrainerVisualization.Helper;
using ScottPlot;
using System;
using System.Windows;
using System.Windows.Controls;
using Color = System.Drawing.Color;

namespace PresentationTrainerVisualization.DashboardComponents.SessionFeedback
{
    public partial class SentencesVideoTimeLine : UserControl
    {
        private ProcessedSessions processedSessions { get; }

        public SentencesVideoTimeLine()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();

            PlotCandleSentences();
        }

        /// <summary>
        /// Plots sentences in Candle chart with color depending on identified or not.
        /// </summary>
        private void PlotCandleSentences()
        {
            var sentences = processedSessions.SelectedSession.Sentences;
            var durationOfSelectedSession = processedSessions.SelectedSession.Duration;
            WpfPlot plot = (WpfPlot)FindName("CandleSentences");

            string[] xLabels = new string[5];
            TimeSpan fractionDuration = TimeSpan.FromTicks(durationOfSelectedSession.Ticks / 4);
            // Display time in equal fractions
            for (int i = 0; i <= 4; i++)
            {
                TimeSpan fraction = TimeSpan.FromTicks(fractionDuration.Ticks * i);
                xLabels[i] = fraction.ToString("mm\\:ss");
            }

            int index = 0;
            foreach (var sentence in sentences)
            {

                if (sentence.WasIdentified)
                {
                    var lolipop = plot.Plot.AddLollipop(
                                       values: new double[] { (sentence.End - sentence.Start).TotalSeconds },
                                       positions: new double[] { index },
                                       color: Constants.GOOD_INDICATOR_COLOR);

                }
                else
                {
                    var lolipop = plot.Plot.AddLollipop(
                                       values: new double[] { (sentence.End - sentence.Start).TotalSeconds },
                                       positions: new double[] { index },
                                       color: Constants.BAD_INDICATOR_COLOR);
                }
                index++;
            }
            // var lolipop = plot.Plot.AddLollipop(values);
            double[] xPositions = { -0.5, (int)sentences.Count * 1 / 4, (int)sentences.Count * 1 / 2, (int)sentences.Count * 3 / 4, sentences.Count - 0.5 };
            plot.Plot.XAxis.ManualTickPositions(xPositions, xLabels);
            //    plot.Plot.YAxis.Ticks(false);
            plot.Plot.YAxis.Label("duration (sec)");
            plot.Plot.Title("Distribution of Sentences");
            plot.Plot.Style(figureBackground: Color.GhostWhite, dataBackground: Color.GhostWhite);
            plot.Plot.Legend();
            plot.Refresh();
        }

        private void SentenceVideoButtonClicked(object sender, RoutedEventArgs e)
        {
            VideoPlayerWindow window = new VideoPlayerWindow(false);
            window.Show();
        }

    }
}
