using PresentationTrainerVisualization.Helper;
using System.Windows.Controls;

namespace PresentationTrainerVisualization.Pages
{
    public partial class ProgressDashboard : Page
    {
        private ProcessedSessions processedSessions { get; }

        public ProgressDashboard()
        {
            // In each dashboard component this instance is used. Has to be created here.
            processedSessions = new ProcessedSessions();

            // Has to be last
            InitializeComponent();
        }
    }
}
