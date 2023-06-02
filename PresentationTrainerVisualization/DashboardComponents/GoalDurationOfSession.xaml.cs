using PresentationTrainerVisualization.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationTrainerVisualization.DashboardComponents
{

    public partial class GoalDurationOfSession : UserControl
    {
        private ProcessedSessionsData processedSessionsData;
        private ProcessedGoalsData processedGoalsData;

        public GoalDurationOfSession()
        {
            InitializeComponent();
            processedSessionsData = new ProcessedSessionsData();
            processedGoalsData = new ProcessedGoalsData();

            PlotCard();
        }

        private void PlotCard()
        {
            throw new NotImplementedException();
        }
    }
}
