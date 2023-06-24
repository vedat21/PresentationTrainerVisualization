using PresentationTrainerVisualization.Helper;
using PresentationTrainerVisualization.models.json;
using System.Collections.Generic;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace PresentationTrainerVisualization.DashboardComponents.Feedback
{
    public partial class GoalDurationOfSession : UserControl
    {
        private ProcessedSessions processedSessions;
        private ProcessedGoals processedGoals;
        private ProcessedConfigurations processedConfigurations;

        public GoalDurationOfSession()
        {
            InitializeComponent();
            processedSessions = ProcessedSessions.GetInstance();
            processedConfigurations = new ProcessedConfigurations();
            processedGoals = new ProcessedGoals();

            PlotGoalDurationOfSession();
        }

        /// <summary>
        /// Plots the goal card that shows the of duration of session for selcted session and for average of selected sessions.
        /// </summary>
        private void PlotGoalDurationOfSession()
        {
            TextBlock timeSelectedSession = (TextBlock)FindName("TimeLastSession");
            TextBlock tipTimeSelectedSession = (TextBlock)FindName("TimeTipLastSession");
            TextBlock timeLastXSession = (TextBlock)FindName("TimeLastXSession");
            TextBlock tipTimeLastXSession = (TextBlock)FindName("TimeTipLastXSession");

            Goal goal = processedGoals.GetGoal(Constants.GoalsLabel.DurationOfSession.ToString());

            if (goal == null)
            {
                TitleDurationOfSession.Text = "Set Goal to display chart";
                return;
            }

            TimeSpan minTime = new TimeSpan(0, 0, int.Parse(goal.Description[Constants.GoalsDescription.session_duration_min_minutes.ToString()]), int.Parse(goal.Description[Constants.GoalsDescription.session_duration_min_seconds.ToString()]));
            TimeSpan maxTime = new TimeSpan(0, 0, int.Parse(goal.Description[Constants.GoalsDescription.session_duration_max_minutes.ToString()]), int.Parse(goal.Description[Constants.GoalsDescription.session_duration_max_seconds.ToString()]));

            List<Session> sessions = processedSessions.GetCopyOfAllSessions();
            TimeSpan result1 = sessions.Find(x => x.Start == processedSessions.SelectedSession.Start).Duration;

            // If data is empty for chosen timespan
            if (result1 == null)
                return;

            // display result for selected session
            if (result1 > maxTime)
            {
                TimeSpan difference = maxTime - result1;
                timeSelectedSession.Foreground = new SolidColorBrush(Colors.Maroon);
                tipTimeSelectedSession.Text = Math.Abs(difference.TotalSeconds).ToString() + "sec overdrawn";
            }
            else if (result1 < minTime)
            {
                TimeSpan difference = result1 - minTime;
                timeSelectedSession.Foreground = new SolidColorBrush(Colors.Maroon);
                tipTimeSelectedSession.Text = Math.Abs(difference.TotalSeconds).ToString() + "sec short";
            }
            else
            {
                timeSelectedSession.Foreground = new SolidColorBrush(Colors.Green);
                tipTimeSelectedSession.Text = "Goal achieved";
            }

            timeSelectedSession.Text = result1.ToString("mm\\:ss") + " min";
            SelectedSessionText.Text = "Selected Session";


            // Decides what is shown
            List<Session> sessionsResult2;
            if (processedConfigurations.ConfigurationLastDays == null)
            {
                sessionsResult2 = ProcessedSessionsHelper.GetLastSessions(sessions, Constants.CONFIG_DEFAULT_NUMBER_OF_SESSIONS);
                LastXSession.Text = "Last 7 Sessions";
            }
            else
            {
                if (processedConfigurations.ConfigurationLastDays.CompareWithLastSessions)
                {
                    sessionsResult2 = ProcessedSessionsHelper.GetLastSessions(sessions, processedConfigurations.ConfigurationLastDays.NumberOfSessions);
                    LastXSession.Text = "Last " + processedConfigurations.ConfigurationLastDays.NumberOfSessions + " Sessions";
                }
                else
                {
                    sessionsResult2 = ProcessedSessionsHelper.GetSessionsOfLastDays(sessions, processedConfigurations.ConfigurationLastDays.NumberOfSessions);
                    LastXSession.Text = "Last " + processedConfigurations.ConfigurationLastDays.NumberOfSessions + " Days";
                }
            }


            if (sessionsResult2.Count == 0)
                return;

            int index = 0;
            TimeSpan tempResult2 = new TimeSpan(0);
            foreach (var session in sessionsResult2)
            {
                tempResult2 += session.Duration;
                index++;
            }

            TimeSpan result2 = new TimeSpan(tempResult2.Ticks / index);
            timeLastXSession.Text = result2.ToString("mm\\:ss") + " min";

            // Decides what is shown
            if (result2 > maxTime)
            {
                TimeSpan difference = maxTime - result2;
                timeLastXSession.Foreground = new SolidColorBrush(Colors.Maroon);
                tipTimeLastXSession.Text = Math.Abs(difference.TotalSeconds).ToString() + "sec overdrawn";
            }
            else if (result2 < minTime)
            {
                TimeSpan difference = result2 - minTime;
                timeLastXSession.Foreground = new SolidColorBrush(Colors.Maroon);
                tipTimeLastXSession.Text = Math.Abs(difference.TotalSeconds).ToString() + "sec short";
            }
            else
            {
                timeLastXSession.Foreground = new SolidColorBrush(Colors.Green);
                tipTimeLastXSession.Text = "Goal achieved";
            }

        }
    }
}
