using PresentationTrainerVisualization.Helper;
using System.Windows.Controls;

namespace PresentationTrainerVisualization.Pages
{
    public partial class ProgressPage : Page
    {
        private ProcessedSessions processedSessions { get; }

        public ProgressPage()
        {
            // In each dashboard component this instance is used. Has to be created here.
            processedSessions = new ProcessedSessions();

            // Has to be last
            InitializeComponent();
        }
    }
}
