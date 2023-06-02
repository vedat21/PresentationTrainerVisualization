using Newtonsoft.Json;
using PresentationTrainerVisualization.models.json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static PresentationTrainerVisualization.helper.Constants;

namespace PresentationTrainerVisualization.Pages
{
    /// <summary>
    /// Interaktionslogik für GoalSettingPage.xaml
    /// </summary>
    public partial class GoalSettingPage : Page
    {
        private GoalsRoot goalsRoot;

        public GoalSettingPage()
        {
            InitializeComponent();

            // json file only exists if user has set goals in the past.
            if (File.Exists(PATH_TO_GOALSCONFIG_DATA))
            {
                string json = File.ReadAllText(PATH_TO_GOALSCONFIG_DATA);
                goalsRoot = JsonConvert.DeserializeObject<GoalsRoot>(json);
            }
            else
            {
                goalsRoot = new GoalsRoot();
                goalsRoot.Goals = new List<Goal>();
            }

            InitButtonsText();
        }

        /// <summary>
        /// Loads the text that is shown on every button to set or update a goal.
        /// </summary>
        public void InitButtonsText()
        {
            foreach (var goalLabel in Enum.GetValues(typeof(GoalsLabel)))
            {
                Button button = (Button)this.FindName("Button" + goalLabel.ToString());

                if (goalsRoot.Goals.Find(x => x.Label == goalLabel.ToString()) != null)
                    button.Content = "Update Goal";
                else
                    button.Content = "Set Goal";
            }
        }

        /// <summary>
        /// Saves the user input into json file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonNumberOfSessionsClicked(object sender, RoutedEventArgs e)
        {
            string GOAL_LABEL = GoalsLabel.NumberOfSessionsForDays.ToString();
            string GOAL_DESC1 = GoalsDescription.number_of_days.ToString();
            string GOAL_DESC2 = GoalsDescription.number_of_sessions.ToString();

            TextBox input1 = (TextBox)this.FindName(GOAL_DESC1);
            TextBox input2 = (TextBox)this.FindName(GOAL_DESC2);

            Dictionary<string, string> description = new Dictionary<string, string>();
            description.Add(GOAL_DESC1, input1.Text);
            description.Add(GOAL_DESC2, input2.Text);

            bool isValid = ValidateInputEmptyAndShowMessage(new string[] { input1.Text, input2.Text });

            if (!isValid)
                return;

            Goal currentGoal = goalsRoot.Goals.Find(x => x.Label == GOAL_LABEL);

            if (currentGoal != null)
                goalsRoot.Goals.Remove(currentGoal);

            Goal goal = new Goal
            {
                StartDate = DateTime.Today,
                Label = GOAL_LABEL,
                Description = description
            };
            goalsRoot.Goals.Add(goal);


            File.WriteAllText(PATH_TO_GOALSCONFIG_DATA, JsonConvert.SerializeObject(goalsRoot));
        }

        public async void ButtonDurationOfSessionClicked(object sender, RoutedEventArgs e)
        {
            string GOAL_LABEL = GoalsLabel.DurationOfSession.ToString();
            string GOAL_MIN_MINUTES = GoalsDescription.session_duration_min_minutes.ToString();
            string GOAL_MIN_SECONDS = GoalsDescription.session_duration_min_seconds.ToString();
            string GOAL_MAX_MINUTES = GoalsDescription.session_duration_max_minutes.ToString();
            string GOAL_MAX_SECONDS = GoalsDescription.session_duration_max_seconds.ToString();

            TextBox inputMinMinutes = (TextBox)this.FindName(GOAL_MIN_MINUTES);
            TextBox inputMinSeconds = (TextBox)this.FindName(GOAL_MIN_SECONDS);
            TextBox inputMaxMinutes = (TextBox)this.FindName(GOAL_MAX_MINUTES);
            TextBox inputMaxSeconds = (TextBox)this.FindName(GOAL_MAX_SECONDS);

            Dictionary<string, string> description = new Dictionary<string, string>();
            description.Add(GOAL_MIN_MINUTES, inputMinMinutes.Text);
            description.Add(GOAL_MIN_SECONDS, inputMinSeconds.Text);
            description.Add(GOAL_MAX_MINUTES, inputMaxMinutes.Text);
            description.Add(GOAL_MAX_SECONDS, inputMaxSeconds.Text);

            bool isValid1 = ValidateInputEmptyAndShowMessage(new string[] { inputMinMinutes.Text, inputMinSeconds.Text, inputMaxMinutes.Text, inputMaxSeconds.Text }, showSucessMessage: false);
            bool isValid2 = ValidateInputMinMaxAndShowMessage(Int32.Parse(inputMinMinutes.Text), Int32.Parse(inputMinSeconds.Text), Int32.Parse(inputMaxMinutes.Text), Int32.Parse(inputMaxSeconds.Text));

            if (!isValid1 || !isValid2)
                return;

            Goal currentGoal = goalsRoot.Goals.Find(x => x.Label == GOAL_LABEL);

            if (currentGoal != null)
                goalsRoot.Goals.Remove(currentGoal);

            Goal goal = new Goal
            {
                StartDate = DateTime.Today,
                Label = GOAL_LABEL,
                Description = description
            };
            goalsRoot.Goals.Add(goal);


            File.WriteAllText(PATH_TO_GOALSCONFIG_DATA, JsonConvert.SerializeObject(goalsRoot));
        }

        public async void ButtonBadActionsClicked(object sender, RoutedEventArgs e)
        {
            string GOAL_LABEL = GoalsLabel.MaxNumberOfBadActions.ToString();
            string GOAL_DESC = GoalsDescription.max_number_of_bad_actions.ToString();

            Slider input = (Slider)this.FindName(GOAL_DESC);

            Dictionary<string, string> description = new Dictionary<string, string>();
            description.Add(GOAL_DESC, input.Value.ToString());

            bool isValid = ValidateInputEmptyAndShowMessage(new string[] { input.Value.ToString() });
            if (!isValid)
                return;

            Goal currentGoal = goalsRoot.Goals.Find(x => x.Label == GOAL_LABEL);

            if (currentGoal != null)
                goalsRoot.Goals.Remove(currentGoal);

            Goal goal = new Goal
            {
                StartDate = DateTime.Today,
                Label = GOAL_LABEL,
                Description = description
            };
            goalsRoot.Goals.Add(goal);


            File.WriteAllText(PATH_TO_GOALSCONFIG_DATA, JsonConvert.SerializeObject(goalsRoot));
        }

        public async void ButtonGoodActionsClicked(object sender, RoutedEventArgs e)
        {
            string GOAL_LABEL = GoalsLabel.MinNumberOfGoodActions.ToString();
            string GOAL_DESC = GoalsDescription.min_number_of_good_actions.ToString();

            Slider input = (Slider)this.FindName(GOAL_DESC);

            Dictionary<string, string> description = new Dictionary<string, string>();
            description.Add(GOAL_DESC, input.Value.ToString());

            bool isValid = ValidateInputEmptyAndShowMessage(new string[] { input.Value.ToString() });
            if (!isValid)
                return;

            Goal currentGoal = goalsRoot.Goals.Find(x => x.Label == GOAL_LABEL);

            if (currentGoal != null)
                goalsRoot.Goals.Remove(currentGoal);

            Goal goal = new Goal
            {
                StartDate = DateTime.Today,
                Label = GOAL_LABEL,
                Description = description
            };
            goalsRoot.Goals.Add(goal);


            File.WriteAllText(PATH_TO_GOALSCONFIG_DATA, JsonConvert.SerializeObject(goalsRoot));
        }

        private bool ValidateInputEmptyAndShowMessage(string[] inputs, bool showSucessMessage = true)
        {
            foreach (var input in inputs)
                if (input.Length <= 0)
                {
                    MessageBox.Show("Goal could not be saved.", "Failure");
                    return false;
                }

            if (showSucessMessage)
                MessageBox.Show("Goal was saved succesfully.", "Success");

            return true;
        }

        private bool ValidateInputMinMaxAndShowMessage(int inputMinMinutes, int inputMinSeconds, int inputMaxMinutes, int inputMaxSeconds)
        {
            if (inputMaxMinutes < inputMinMinutes)
            {
                MessageBox.Show("Goal could not be saved.\n Minimum time duration must be bigger than maximum time duration", "Failure");
                return false;
            }
            else if (inputMinMinutes == inputMaxMinutes && inputMaxSeconds <= inputMinSeconds)
            {
                MessageBox.Show("Goal could not be saved.\n Minimum time duration must be bigger than maximum time duration", "Failure");
                return false;
            }

            MessageBox.Show("Goal was saved succesfully.", "Success");
            return true;
        }

        /// <summary>
        /// Validation for textboxes. Only allow numbers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBoxNumberRange(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }

        public static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 0 && i <= 100;
        }
              
    }
}
