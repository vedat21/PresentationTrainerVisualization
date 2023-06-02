using Newtonsoft.Json;
using PresentationTrainerVisualization.helper;
using PresentationTrainerVisualization.models.json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;


namespace PresentationTrainerVisualization.Pages
{
    /// <summary>
    /// Interaktionslogik für ChartsConfigurationPage.xaml
    /// </summary>
    public partial class ChartsConfigurationPage : Page
    {
        private ConfigurationRoot configurationRoot;
        private Configuration configurationLastDays;
        private Configuration configurationTimeSpan;

        private int DEFAULT_NUMBER_OF_SESSIONS = 7;
        private bool DEFAULT_IS_LAST_SESSIONS = true;


        public ChartsConfigurationPage()
        {
            InitializeComponent();

            // json file only exists if user has set goals in the past.
            if (File.Exists(Constants.PATH_TO_CONFIG_DATA))
            {
                string json = File.ReadAllText(Constants.PATH_TO_CONFIG_DATA);
                configurationRoot = JsonConvert.DeserializeObject<ConfigurationRoot>(json);
                configurationLastDays = configurationRoot.Configurations.Find(x => x.Label == Constants.ConfigurationLabel.CONFIGURATION_LAST_X.ToString());
                configurationTimeSpan = configurationRoot.Configurations.Find(x => x.Label == Constants.ConfigurationLabel.CONFIGURATION_DATES.ToString());
            }
            else
            {
                configurationRoot = new ConfigurationRoot();
                configurationRoot.Configurations = new List<Configuration>();
            }

            InitValues();
        }

        private void InitValues()
        {
            Slider slider = (Slider)FindName("NumberOfX");
            ToggleButton toggleButton = (ToggleButton)FindName("DaysOrSessions");

            DatePicker startDate = (DatePicker)FindName("StartDate");
            DatePicker endDate = (DatePicker)FindName("EndDate");


            if (configurationLastDays != null)
            {
                slider.Value = configurationLastDays.LastDaysOrSessions;
                toggleButton.IsChecked = configurationLastDays.IsLastSessions;
            }
            else
            {
                slider.Value = DEFAULT_NUMBER_OF_SESSIONS;
                toggleButton.IsChecked = DEFAULT_IS_LAST_SESSIONS;
            }

            if (configurationTimeSpan != null)
            {
                startDate.Text = configurationTimeSpan.StartDate.ToString();
                endDate.Text = configurationTimeSpan.EndDate.ToString();
            }
            else
            {
                startDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                endDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }

        private void HandleLastSessionsToggle(object sender, RoutedEventArgs e)
        {
            ToggleButton srcButton = e.Source as ToggleButton;
            srcButton.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));

        }

        private void SaveButton1Clicked(object sender, RoutedEventArgs e)
        {
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

            MessageBox.Show("Changes are saved.", "Success");
            File.WriteAllText(Constants.PATH_TO_CONFIG_DATA, JsonConvert.SerializeObject(configurationRoot));
        }

        private void SaveButton2Clicked(object sender, RoutedEventArgs e)
        {
            DateTime startDate = (DateTime)StartDate.SelectedDate;
            DateTime endDate = (DateTime)EndDate.SelectedDate;

            if(startDate == null || endDate == null)
            {
                MessageBox.Show("Dates should not be emtpy.", "Failure");
                return;
            }

            if (configurationTimeSpan != null)
                configurationRoot.Configurations.Remove(configurationTimeSpan);

            Configuration newConfiguration = new Configuration
            {
                StartDate = DateOnly.FromDateTime(startDate),
                EndDate = DateOnly.FromDateTime(endDate),
                Label = Constants.ConfigurationLabel.CONFIGURATION_DATES.ToString(),
            };
            configurationRoot.Configurations.Add(newConfiguration);

            MessageBox.Show("Changes are saved.", "Success");
            File.WriteAllText(Constants.PATH_TO_CONFIG_DATA, JsonConvert.SerializeObject(configurationRoot));

        }
    }
}
