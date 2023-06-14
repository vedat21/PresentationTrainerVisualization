using Newtonsoft.Json;
using PresentationTrainerVisualization.helper;
using PresentationTrainerVisualization.models.json;
using PresentationTrainerVisualization.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace PresentationTrainerVisualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProcessedSessionsData processedSessions;
        private ConfigurationRoot configurationRoot;
        private ComboBox comboBoxSelectedSession;

        private bool isLastSession;

        private int DEFAULT_NUMBER_OF_SESSIONS = 7;

        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            processedSessions = new ProcessedSessionsData();
            comboBoxSelectedSession = (ComboBox)FindName("ComboSessions");


            // json file only exists if user has set goals in the past.
            if (File.Exists(Constants.PATH_TO_CONFIG_DATA))
            {
                string json = File.ReadAllText(Constants.PATH_TO_CONFIG_DATA);
                configurationRoot = JsonConvert.DeserializeObject<ConfigurationRoot>(json);
            }
            else
            {
                configurationRoot = new ConfigurationRoot();
                configurationRoot.Configurations = new List<Configuration>();
            }

            InitComboBox();
            InitUserSelection();
        }


        private void InitUserSelection()
        {
            Configuration configurationTimeSpan = configurationRoot.Configurations.Find(x => x.Label == Constants.ConfigurationLabel.CONFIGURATION_DATES.ToString());
            Configuration configurationLastDays = configurationRoot.Configurations.Find(x => x.Label == Constants.ConfigurationLabel.CONFIGURATION_LAST_X.ToString());

            DatePicker startDate = (DatePicker)FindName("StartDate");
            DatePicker endDate = (DatePicker)FindName("EndDate");
            Slider slider = (Slider)FindName("NumberOfX");
            ToggleButton toggleButton = (ToggleButton)FindName("DaysOrSessions");


            if (configurationTimeSpan != null)
            {
                startDate.Text = configurationTimeSpan.StartDate.ToString();
                endDate.Text = configurationTimeSpan.EndDate.ToString();

                startDate.DisplayDateEnd = endDate.SelectedDate;
                endDate.DisplayDateStart = startDate.SelectedDate;
            }
            else
            {
                startDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                endDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }

            if (configurationLastDays != null)
            {
                slider.Value = configurationLastDays.LastDaysOrSessions;
                toggleButton.IsChecked = configurationLastDays.IsLastSessions;

                if (configurationLastDays.IsLastSessions)
                {
                    slider.Maximum = processedSessions.GetNumberOfTotalSessions();
                }
                else
                {
                    slider.Maximum = processedSessions.NumberOfDaysBetweenFirstSessionAndToday();
                }
            }
            else
            {
                slider.Value = DEFAULT_NUMBER_OF_SESSIONS;
                toggleButton.IsChecked = true;
            }
        }

        private void InitComboBox()
        {
            List<Session> sessions = processedSessions.GetCopyOfAllSessions();
            Session avgSession = new Session();
            avgSession.TextForComboBox = "Progress";
            avgSession.StartForComboBox = null;
            sessions.Insert(0, avgSession);

            // Pre select ProgressDashboard
            ComboSessions.ItemsSource = sessions;
            ComboSessions.SelectedIndex = 0;
        }

        private void HandleComboBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDashboard();
        }

        private void HandleDatePickerChanged(object sender, SelectionChangedEventArgs e)
        {
            Configuration configurationTimeSpan = configurationRoot.Configurations.Find(x => x.Label == Constants.ConfigurationLabel.CONFIGURATION_DATES.ToString());

            DatePicker startDate = (DatePicker)StartDate;
            DatePicker endDate = (DatePicker)EndDate;

            if (startDate.SelectedDate == null || endDate.SelectedDate == null)
                return;

            DateTime selectedStartDate = (DateTime)startDate.SelectedDate;
            DateTime selectedEndDate = (DateTime)endDate.SelectedDate;
            // set date restriction
            startDate.DisplayDateEnd = endDate.SelectedDate;
            endDate.DisplayDateStart = startDate.SelectedDate;


            if (configurationTimeSpan != null)
                configurationRoot.Configurations.Remove(configurationTimeSpan);

            Configuration newConfiguration = new Configuration
            {
                StartDate = DateOnly.FromDateTime(selectedStartDate),
                EndDate = DateOnly.FromDateTime(selectedEndDate),
                Label = Constants.ConfigurationLabel.CONFIGURATION_DATES.ToString(),
            };
            configurationRoot.Configurations.Add(newConfiguration);

            File.WriteAllText(Constants.PATH_TO_CONFIG_DATA, JsonConvert.SerializeObject(configurationRoot));
            LoadDashboard();
        }

        private void HandleLastSessionsToggle(object sender, RoutedEventArgs e)
        {
            Slider slider = (Slider)FindName("NumberOfX");
            ToggleButton toggleButton = e.Source as ToggleButton;
            toggleButton.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            Trace.WriteLine(toggleButton.IsChecked);

            if(toggleButton.IsChecked == true)
            {
                slider.Maximum = processedSessions.GetNumberOfTotalSessions();
            }
            else
            {
                slider.Maximum = processedSessions.NumberOfDaysBetweenFirstSessionAndToday();
            }

            UpdateLastDaysConfiguration();
        }

        private void HandleSliderDragCompleted(object sender, DragCompletedEventArgs e)
        {
            UpdateLastDaysConfiguration();
        }

        private void UpdateLastDaysConfiguration()
        {
            Configuration configurationLastDays = configurationRoot.Configurations.Find(x => x.Label == Constants.ConfigurationLabel.CONFIGURATION_LAST_X.ToString());

            Slider slider = (Slider)FindName("NumberOfX");
            ToggleButton toggleButton = (ToggleButton)FindName("DaysOrSessions");

            if (configurationLastDays != null)
                configurationRoot.Configurations.Remove(configurationLastDays);


            Configuration newConfiguration = new Configuration
            {
                LastDaysOrSessions = (int)slider.Value,
                IsLastSessions = (bool)toggleButton.IsChecked,
                Label = Constants.ConfigurationLabel.CONFIGURATION_LAST_X.ToString(),
            };
            configurationRoot.Configurations.Add(newConfiguration);

            File.WriteAllText(Constants.PATH_TO_CONFIG_DATA, JsonConvert.SerializeObject(configurationRoot));
            LoadDashboard();
        }

        /// <summary>
        /// Makes texts next to icon visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleSidebarChecked(object sender, RoutedEventArgs e)
        {
            ((TextBlock)FindName("TextDashboard1")).Visibility = Visibility.Visible;
            ((TextBlock)FindName("TextGoalsSetting")).Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Makes texts next to icon hidden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleSidebarUnchecked(object sender, RoutedEventArgs e)
        {
            ((TextBlock)FindName("TextDashboard1")).Visibility = Visibility.Hidden;
            ((TextBlock)FindName("TextGoalsSetting")).Visibility = Visibility.Hidden;
        }

        private void HandleDashboardClicked(object sender, RoutedEventArgs e)
        {
            ((Grid)FindName("DashboardConfiguration")).Visibility = Visibility.Visible;
            LoadDashboard();
        }

        private void HandleGoalClicked(object sender, RoutedEventArgs e)
        {
            ((Grid)FindName("DashboardConfiguration")).Visibility = Visibility.Hidden;
            ((Frame)FindName("MainContainer")).Content = new GoalSettingPage();
        }

        private void LoadProgressDashboard()
        {
            ((Frame)FindName("MainContainer")).Content = new ProgressDashboard();
            ((Grid)FindName("SelectDateSpan")).Visibility = Visibility.Visible;
            ((Grid)FindName("SelectLastSessions")).Visibility = Visibility.Collapsed;
        }

        private void LoadFeedbackDashboard()
        {
            ((Frame)FindName("MainContainer")).Content = new FeedbackDashboard();
            ((Grid)FindName("SelectDateSpan")).Visibility = Visibility.Collapsed;
            ((Grid)FindName("SelectLastSessions")).Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Based on the selected value in combobox loads the progress or feedback dashboard
        /// </summary>
        private void LoadDashboard()
        {
            var selectedString = ((Session)comboBoxSelectedSession.SelectedItem).TextForComboBox;

            // Average Dashboard is shown
            if (selectedString != null)
                LoadProgressDashboard();
            else
                LoadFeedbackDashboard();
        }
    }
}
