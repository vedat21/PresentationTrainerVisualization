﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PresentationTrainerVisualization.helper;
using PresentationTrainerVisualization.models.json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static PresentationTrainerVisualization.helper.Constants;
using Action = PresentationTrainerVisualization.models.json.Action;

namespace PresentationTrainerVisualization.Pages
{
    /// <summary>
    /// Interaktionslogik für GoalSettingPage.xaml
    /// </summary>
    public partial class GoalSettingPage : Page
    {
        private GoalsRoot goalsRoot;
        private ProcessedSessionsData processedSessionsData;

        private List<Action> badActions;
        private List<Action> goodActions;
        private ListBox badActionsListBox;
        private ListBox goodActionsListBox;

        public GoalSettingPage()
        {
            InitializeComponent();
            processedSessionsData = new ProcessedSessionsData();
            badActions = new List<Action>();
            goodActions = new List<Action>();
            badActionsListBox = (ListBox)FindName("ListBoxBadActions");
            goodActionsListBox = (ListBox)FindName("ListBoxGoodActions");

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
            InitListBoxesAction();
        }

        /// <summary>
        /// Loads the text that is shown on every button to set or update a goal.
        /// </summary>
        public void InitButtonsText()
        {
            foreach (var goalLabel in Enum.GetValues(typeof(GoalsLabel)))
            {
                Button button = (Button)this.FindName("Button" + goalLabel.ToString());

                if (button == null)
                {
                    break;
                }

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

            Dictionary<string, dynamic> description = new Dictionary<string, dynamic>() { { GOAL_DESC1, input1.Text }, { GOAL_DESC2, input2.Text } };

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

            Dictionary<string, dynamic> description = new Dictionary<string, dynamic>() { { GOAL_MIN_MINUTES, inputMinMinutes.Text }, { GOAL_MIN_SECONDS, inputMinSeconds.Text }, { GOAL_MAX_MINUTES, inputMaxMinutes.Text }, { GOAL_MAX_SECONDS, inputMaxSeconds.Text } };

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

            Dictionary<string, dynamic> description = new Dictionary<string, dynamic>() { { GOAL_DESC, input.Value.ToString() } };

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

        /// <summary>
        /// Inits the values of both listboxes
        /// </summary>
        private void InitListBoxesAction()
        {
            foreach (var entry in Constants.BAD_ACTION_FROM_VIDEO)
                badActions.Add(new Action(entry.Key, entry.Value, true));
            badActionsListBox.ItemsSource = badActions;

            foreach (var entry in Constants.GOOD_ACTION_FROM_VIDEO)
                goodActions.Add(new Action(entry.Key, entry.Value, true));
            goodActionsListBox.ItemsSource = goodActions;

        }

        /// <summary>
        /// Default selects items that were selected previously
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxGoodActions_Loaded(object sender, RoutedEventArgs e)
        {
            string GOAL_LABEL = GoalsLabel.GoodActions.ToString();
            string GOAL_DESC = GoalsDescription.list_of_good_actions.ToString();
            Goal currentGoal = goalsRoot.Goals.Find(x => x.Label == GOAL_LABEL);

            if(currentGoal ==  null)
                return;

            var goalDesc = currentGoal.Description[GOAL_DESC];
            List<string> selectedActions = new List<string>();
            foreach (var description in goalDesc)
                selectedActions.Add(description.ToString());

            foreach (var action in goodActions)
            {
                if (selectedActions.Contains(action.LogAction))
                {
                    int index = goodActions.IndexOf(action);
                    ListBoxItem item = (ListBoxItem)goodActionsListBox.ItemContainerGenerator.ContainerFromIndex(index);
                    if (item != null)
                        item.IsSelected = true;
                    else
                    {
                        goodActionsListBox.UpdateLayout();
                        goodActionsListBox.ScrollIntoView(goodActionsListBox.Items[index]);
                        item = (ListBoxItem)goodActionsListBox.ItemContainerGenerator.ContainerFromIndex(index);
                        item.IsSelected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Default selects items that were selected previously
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxBadActions_Loaded(object sender, RoutedEventArgs e)
        {
            string GOAL_LABEL = GoalsLabel.BadActions.ToString();
            string GOAL_DESC = GoalsDescription.list_of_bad_actions.ToString();
            Goal currentGoal = goalsRoot.Goals.Find(x => x.Label == GOAL_LABEL);

            if (currentGoal == null)
                return;

            var goalDesc = currentGoal.Description[GOAL_DESC];
            List<string> selectedActions = new List<string>();
            foreach (var description in goalDesc)
                selectedActions.Add(description.ToString());

            foreach (var action in badActions)
            {
                if (selectedActions.Contains(action.LogAction))
                {
                    int index = badActions.IndexOf(action);
                    ListBoxItem item = (ListBoxItem)badActionsListBox.ItemContainerGenerator.ContainerFromIndex(index);
                    if (item != null)
                        item.IsSelected = true;
                    else
                    {
                        badActionsListBox.UpdateLayout();
                        badActionsListBox.ScrollIntoView(badActionsListBox.Items[index]);
                        item = (ListBoxItem)badActionsListBox.ItemContainerGenerator.ContainerFromIndex(index);
                        item.IsSelected = true;
                    }
                }
            }
        }

        public async void HandleListBoxGoodActions(object sender, RoutedEventArgs e)
        {
            string GOAL_LABEL = GoalsLabel.GoodActions.ToString();
            string GOAL_DESC = GoalsDescription.list_of_good_actions.ToString();

            IList tempSelectedActions = (IList)goodActionsListBox.SelectedItems;
            var selectedActions = tempSelectedActions.Cast<Action>();
            List<string> selectedActionsLabel = selectedActions.Select(x => x.LogAction).ToList();

            Dictionary<string, dynamic> description = new Dictionary<string, dynamic> { { GOAL_DESC, selectedActionsLabel } };

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

        public async void HandleListBoxBadActions(object sender, RoutedEventArgs e)
        {
            string GOAL_LABEL = GoalsLabel.BadActions.ToString();
            string GOAL_DESC = GoalsDescription.list_of_bad_actions.ToString();

            IList tempSelectedActions = (IList)badActionsListBox.SelectedItems;
            var selectedActions = tempSelectedActions.Cast<Action>();
            List<string> selectedActionsLabel = selectedActions.Select(x => x.LogAction).ToList();

            Dictionary<string, dynamic> description = new Dictionary<string, dynamic> { { GOAL_DESC, selectedActionsLabel } };

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

            Dictionary<string, dynamic> description = new Dictionary<string, dynamic> { { GOAL_DESC, input.Value.ToString() } };

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

    }
}
