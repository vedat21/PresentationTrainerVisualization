﻿using Newtonsoft.Json;
using PresentationTrainerVisualization.Helper;
using PresentationTrainerVisualization.models;
using PresentationTrainerVisualization.models.json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using static PresentationTrainerVisualization.helper.Constants;
using Action = PresentationTrainerVisualization.models.json.Action;


namespace PresentationTrainerVisualization.helper
{
    public class ProcessedSessionsData
    {
        private static ProcessedSessionsData instance;

        private SessionsRoot sessionsRoot;
        private Session selectedSession; // selected session by user in dashboard 

        private ProcessedGoalsData processedGoals;
        private List<string> selectedGoalsActions;

        private ProcessedConfigurationData processedConfiguration;


        public ProcessedSessionsData()
        {
            sessionsRoot = JsonConvert.DeserializeObject<SessionsRoot>(File.ReadAllText(Constants.PATH_TO_SESSION_DATA));
            processedGoals = new ProcessedGoalsData();
            processedConfiguration = new ProcessedConfigurationData();
            selectedGoalsActions = processedGoals.GetSelectedActionsLog();

            // Get selected session from combobox
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    selectedSession = (Session)(window as MainWindow).ComboSessions.SelectedItem;
                }
            }

        }

        public static ProcessedSessionsData GetInstance()
        {
            if (instance == null)
                instance = new ProcessedSessionsData();

            return instance;
        }

        /// <summary>
        /// To not change the original object. 
        /// </summary>
        /// <returns></returns>
        public List<Session> GetCopyOfAllSessions()
        {
            var sessions = sessionsRoot.Sessions;

            // Want to return copy not reference
            List<Session> copySessions = new List<Session>();
            foreach (var session in sessions)
            {
                copySessions.Add(new Session
                {
                    Actions = session.Actions,
                    End = session.End,
                    Level = session.Level,
                    Sentences = session.Sentences,
                    Start = session.Start,
                    ScriptVisible = session.ScriptVisible,
                    VideoId = session.VideoId,
                    Duration = session.Duration,
                    TextForComboBox = session.TextForComboBox,
                    StartForComboBox = session.StartForComboBox,
                });
            }

            return copySessions;
        }

        /// <summary>
        /// Returns the total number of sessions.
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfTotalSessions()
        {
            return sessionsRoot.Sessions.Count;
        }

        /// <summary>
        /// Get selected session. User can select sessions in combobox
        /// </summary>
        /// <returns></returns>
        public Session GetSelectedSession()
        {
            return selectedSession;
        }

        /// <summary>
        /// Get actions from selected session. Only includes actions which were selected in GoalSetting page.
        /// </summary>
        /// <returns></returns>
        public List<Action> GetSelectedSessionActions()
        {
            List<Action> actions = (from action in selectedSession.Actions
                                    where selectedGoalsActions.Contains(action.LogAction)
                                    select action).ToList();

            return actions;
        }

        /// <summary>
        /// Get the number of sessions for selected datespan.
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfSessions()
        {
            int numberOfSessions = (from session in sessionsRoot.Sessions
                                    where processedConfiguration.ConfigurationTimeSpan.StartDate <= DateOnly.FromDateTime(session.Start) && DateOnly.FromDateTime(session.Start) <= processedConfiguration.ConfigurationTimeSpan.EndDate
                                    select session).Count();

            return numberOfSessions;
        }

        public int NumberOfDaysBetweenFirstSessionAndToday()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            DateOnly firstSession = DateOnly.FromDateTime(sessionsRoot.Sessions.First().Start);

            return today.DayNumber - firstSession.DayNumber;
        }

        /// <summary>
        /// Returns the percentage of identified sentences from selected session.
        /// </summary>
        /// <returns></returns>
        public double GetPercentageOfIdentifiedFromSelectedSession()
        {
            int numberOfIdentified = 0;
            int numberOfNotIdentified = 0;

            foreach (var sentence in selectedSession.Sentences)
                if (sentence.WasIdentified)
                    numberOfIdentified++;
                else
                    numberOfNotIdentified++;

            double percentageIdentified = Math.Round((double)numberOfIdentified / (numberOfIdentified + numberOfNotIdentified) * 100, 2);

            return percentageIdentified;
        }

        /// <summary>
        /// Determines the number of actions by each session.Every AggregatedSession represents a session.
        /// </summary>
        /// <param name="mistake"></param>
        /// <returns></returns>
        public List<AggregatedSession> GetNumberOfActionsBySession(bool mistake = true)
        {
            List<AggregatedSession> result = new List<AggregatedSession>();

            foreach (var session in sessionsRoot.Sessions)
            {
                DateOnly sessionDate = DateOnly.FromDateTime(session.Start);

                // get only the data that is in timespan of user selection
                if (processedConfiguration.ConfigurationTimeSpan.StartDate <= sessionDate && processedConfiguration.ConfigurationTimeSpan.EndDate >= sessionDate)
                {
                    List<AggregatedObject> aggregatedObjects = new List<AggregatedObject>();
                    int numberOfAction = 0;

                    foreach (var action in session.Actions)
                        if (mistake == action.Mistake)
                            numberOfAction++;

                    aggregatedObjects.Add(new AggregatedObject("count", numberOfAction));
                    result.Add(new AggregatedSession(aggregatedObjects, session.Start));
                }
            }

            return result;
        }

        /// <summary>
        /// Determines the percentage of identified sentences by each session. Every AggregatedSession represents a session.
        /// </summary>
        /// <returns></returns>
        public List<AggregatedSession> GetPercentageOfRecongnisedSentenceBySession()
        {
            List<AggregatedSession> result = new List<AggregatedSession>();

            foreach (var session in sessionsRoot.Sessions)
            {
                DateOnly sessionDate = DateOnly.FromDateTime(session.Start);

                // get only the data that is in timespan of user selection
                if (processedConfiguration.ConfigurationTimeSpan.StartDate <= sessionDate && processedConfiguration.ConfigurationTimeSpan.EndDate >= sessionDate)
                {
                    List<AggregatedObject> aggregatedObjects = new List<AggregatedObject>();
                    int numberOfIdentified = 0;
                    int numberOfNotIdentified = 0;

                    foreach (var sentence in session.Sentences)
                        if (sentence.WasIdentified)
                            numberOfIdentified++;
                        else
                            numberOfNotIdentified++;

                    double percentageIdentified = Math.Round((double)numberOfIdentified / (numberOfIdentified + numberOfNotIdentified) * 100, 2);
                    aggregatedObjects.Add(new AggregatedObject("percentageOfIdentified", percentageIdentified));
                    result.Add(new AggregatedSession(aggregatedObjects, session.Start));
                }
            }

            return result;
        }

        /// <summary>
        /// Determines the duration of each session.
        /// </summary>
        /// <returns></returns>
        public List<AggregatedSession> GetDurationBySession()
        {
            List<AggregatedSession> result = new List<AggregatedSession>();

            foreach (var session in sessionsRoot.Sessions)
            {
                DateOnly sessionDate = DateOnly.FromDateTime(session.Start);

                // get only the data that is in timespan of user selection
                if (processedConfiguration.ConfigurationTimeSpan.StartDate <= sessionDate && processedConfiguration.ConfigurationTimeSpan.EndDate >= sessionDate)
                {
                    List<AggregatedObject> aggregatedObjects = new List<AggregatedObject>();
                    aggregatedObjects.Add(new AggregatedObject("duration", session.Duration.TotalSeconds));
                    result.Add(new AggregatedSession(aggregatedObjects, session.Start));
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the average number of idetified sentences in selected datespan.
        /// </summary>
        /// <returns></returns>
        public double GetAverageNumberOfIdentifiedentences()
        {
            double numberOfRecongnised = 0;
            double numberOfNotRecongnised = 0;

            foreach (var session in sessionsRoot.Sessions)
            {
                DateOnly sessionDate = DateOnly.FromDateTime(session.Start);

                // get only the data that is in timespan of user selection
                if (processedConfiguration.ConfigurationTimeSpan.StartDate <= sessionDate && processedConfiguration.ConfigurationTimeSpan.EndDate >= sessionDate)
                    foreach (var sentence in session.Sentences)
                        if (sentence.WasIdentified)
                            numberOfRecongnised++;
                        else
                            numberOfNotRecongnised++;
            }

            double result = (numberOfRecongnised / (numberOfRecongnised + numberOfNotRecongnised)) * 100;

            return Math.Round(result, 2);
        }

        /// <summary>
        /// Returns the average number of mistake actions in selected datespan.
        /// </summary>
        /// <returns></returns>
        public double GetAverageNumberOfBadActions()
        {
            List<string> selectedActions = processedGoals.GetSelectedActionsLog();
        
            double numberOfBadActions = 0;
            double numberOfGoodActions = 0;

            foreach (var session in sessionsRoot.Sessions)
            {
                DateOnly sessionDate = DateOnly.FromDateTime(session.Start);

                if (processedConfiguration.ConfigurationTimeSpan.StartDate <= sessionDate && processedConfiguration.ConfigurationTimeSpan.EndDate >= sessionDate)
                    foreach (var action in session.Actions)
                        if (selectedActions.Contains(action.LogAction))
                            if (action.Mistake)
                                numberOfBadActions++;
                            else
                                numberOfGoodActions++;
            }

            double result = (numberOfBadActions / (numberOfBadActions + numberOfGoodActions)) * 100;

            return Math.Round(result, 1);
        }

        /// <summary>
        /// Returns the average number of actions that are no mistake in selected datespan.
        /// </summary>
        /// <returns></returns>
        public double GetAverageNumberOfGoodActions()
        {
            List<string> selectedActions = processedGoals.GetSelectedActionsLog();

            double numberOfBadActions = 0;
            double numberOfGoodActions = 0;

            foreach (var session in sessionsRoot.Sessions)
            {
                DateOnly sessionDate = DateOnly.FromDateTime(session.Start);

                if (processedConfiguration.ConfigurationTimeSpan.StartDate <= sessionDate && processedConfiguration.ConfigurationTimeSpan.EndDate >= sessionDate)
                    foreach (var action in session.Actions)
                        if (selectedActions.Contains(action.LogAction))
                            if (action.Mistake)
                                numberOfBadActions++;
                            else
                                numberOfGoodActions++;
            }

            double result = (numberOfGoodActions / (numberOfBadActions + numberOfGoodActions)) * 100;

            return Math.Round(result, 1);
        }

        /// <summary>
        /// Returns the total time of all sessions in selected datespan.
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTotalTimeSpent()
        {
            TimeSpan totalTime = new TimeSpan();
            foreach (var session in sessionsRoot.Sessions)
            {
                var sessionDate = session.Start;

                // get only the data that is in timespan of user selection
                if (processedConfiguration.ConfigurationTimeSpan.StartDate <= DateOnly.FromDateTime(sessionDate) && processedConfiguration.ConfigurationTimeSpan.EndDate >= DateOnly.FromDateTime(sessionDate))
                {
                    TimeSpan timeDifference = session.End - session.Start;
                    totalTime += timeDifference;
                }
            }

            return totalTime;
        }

        /// <summary>
        /// Returns the number of sessions by each datetime.
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<DateTime, int> GetNumberOfSessionsByDateTime()
        {
            SortedDictionary<DateTime, int> numberOfSesions = new SortedDictionary<DateTime, int>();

            foreach (var session in sessionsRoot.Sessions)
            {
                var sessionDate = session.Start;

                // get only the data that is in timespan of user selection
                if (processedConfiguration.ConfigurationTimeSpan.StartDate <= DateOnly.FromDateTime(sessionDate) && processedConfiguration.ConfigurationTimeSpan.EndDate >= DateOnly.FromDateTime(sessionDate))
                    if (numberOfSesions.ContainsKey(sessionDate))
                        numberOfSesions[sessionDate] = numberOfSesions[sessionDate] + 1;
                    else
                        numberOfSesions[sessionDate] = 1;
            }

            return numberOfSesions;
        }

        /// <summary>
        /// Returns the number of sessions by dateonly.
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<DateOnly, int> GetNumberOfSessionsByDateOnly()
        {
            // Count number of sessions by dateonly
            SortedDictionary<DateOnly, int> numberOfSesions = new SortedDictionary<DateOnly, int>();

            foreach (var session in sessionsRoot.Sessions)
            {
                var sessionDate = DateOnly.FromDateTime(session.Start);

                // get only the data that is in timespan of user selection
                if (processedConfiguration.ConfigurationTimeSpan.StartDate <= sessionDate && processedConfiguration.ConfigurationTimeSpan.EndDate >= sessionDate)
                    if (numberOfSesions.ContainsKey(sessionDate))
                        numberOfSesions[sessionDate] = numberOfSesions[sessionDate] + 1;
                    else
                        numberOfSesions[sessionDate] = 1;
            }

            return numberOfSesions;
        }

        /// <summary>
        /// Determines the number of actions recognized with video by each session. Every list of AggregatedObjects represents a session.
        /// Parameter all determines if all actions should be included.
        /// Parameter mistake determines if actions that are a mistake or actions that are not a mistake should be included.
        /// </summary>
        /// <param name="mistake"></param>
        /// <returns></returns>
        public List<AggregatedSession> GetAggregatedActionsBySession(bool all = true, bool mistake = true)
        {
            List<string> selectedActions = processedGoals.GetSelectedActionsLog();

            List<AggregatedSession> result = new List<AggregatedSession>();
            List<string> allPossibleLabels = new List<string>();

            foreach (var session in sessionsRoot.Sessions)
            {
                List<AggregatedObject> aggregatedObjects = new List<AggregatedObject>();
                Dictionary<String, int> countOfLabels = new Dictionary<String, int>();

                foreach (var action in session.Actions)
                {
                    string logAction = action.LogAction.ToUpper();

                    // include all actions
                    if (all && selectedActions.Contains(logAction))
                    {
                        if (countOfLabels.ContainsKey(logAction))
                            countOfLabels[logAction] = countOfLabels[logAction] + 1;
                        else
                            countOfLabels[logAction] = 1;

                        // every list of aggregated objects should have the same number of labels.
                        if (!allPossibleLabels.Contains(logAction))
                            allPossibleLabels.Add(logAction);
                    }
                    // include only mistake or no mistake
                    else
                    {
                        if (mistake == true && selectedActions.Contains(logAction))
                        {
                            if (countOfLabels.ContainsKey(logAction))
                                countOfLabels[logAction] = countOfLabels[logAction] + 1;
                            else
                                countOfLabels[logAction] = 1;

                            // every list of aggregated objects should have the same number of labels.
                            if (!allPossibleLabels.Contains(logAction))
                                allPossibleLabels.Add(logAction);
                        }
                        else if (mistake == false && selectedActions.Contains(logAction))
                        {
                            if (countOfLabels.ContainsKey(logAction))
                                countOfLabels[logAction] = countOfLabels[logAction] + 1;
                            else
                                countOfLabels[logAction] = 1;

                            // every list of aggregated objects should have the same number of labels.
                            if (!allPossibleLabels.Contains(logAction))
                                allPossibleLabels.Add(logAction);
                        }
                    }
                }

                foreach (KeyValuePair<String, int> kvp in countOfLabels)
                    aggregatedObjects.Add(new AggregatedObject(kvp.Key, kvp.Value));

                result.Add(new AggregatedSession(aggregatedObjects, session.Start));
            }

            foreach (var sessionsResult in result)
            {
                // every list of aggregated objects should have the same number of labels.
                List<string> labelsInSessionResult = sessionsResult.AggregatedObjects.Select(s => s.Label).ToList();
                var missingLabels = allPossibleLabels.Except(labelsInSessionResult);

                foreach (var missingLabel in missingLabels)
                    sessionsResult.AggregatedObjects.Add(new AggregatedObject(missingLabel, 0));

                // sort every list in same order
                sessionsResult.AggregatedObjects.Sort((x, y) => x.Label.CompareTo(y.Label));
            }

            return result;
        }



        // Not used currently.

        public Dictionary<DateOnly, int> GetDurationByDate()
        {

            // Count number of sessions by dateonly
            Dictionary<DateOnly, int> averageTimeSpent = new Dictionary<DateOnly, int>();
            foreach (var session in sessionsRoot.Sessions)
            {
                var date = DateOnly.FromDateTime(session.Start);
                var start = session.Start;
                var end = session.End;

                if (end == null)
                    continue;

                var durationMinutes = (end - start).Minutes;

                if (averageTimeSpent.ContainsKey(date))
                    averageTimeSpent[date] = averageTimeSpent[date] + durationMinutes;
                else
                    averageTimeSpent[date] = durationMinutes;
            }

            return averageTimeSpent;
        }

        public List<AggregatedSession> GetActionsBySession(bool mistake = true)
        {
            List<AggregatedSession> result = new List<AggregatedSession>();

            foreach (var session in sessionsRoot.Sessions)
            {
                List<AggregatedObject> aggregatedObjects = new List<AggregatedObject>();
                int numberOfTargetActions = 0;

                foreach (var action in session.Actions)
                    if (mistake == action.Mistake)
                        numberOfTargetActions++;

                aggregatedObjects.Add(new AggregatedObject("count", numberOfTargetActions));
                result.Add(new AggregatedSession(aggregatedObjects, session.Start));
            }

            return result;
        }

        public List<Dictionary<String, double>> GetNumberOfRightAndWrongSentencesBySession()
        {

            List<Dictionary<String, double>> resultData = new List<Dictionary<String, double>>();

            foreach (var session in sessionsRoot.Sessions)
            {
                Dictionary<String, double> dataBySession = new Dictionary<string, double>();
                int rightSentences = 0;
                int wrongSentences = 0;
                foreach (var sentence in session.Sentences)
                {
                    if (sentence.WasIdentified)
                    {
                        rightSentences++;
                    }
                    else
                    {
                        wrongSentences++;
                    }
                }
                dataBySession.Add("right", rightSentences);
                dataBySession.Add("wrong", wrongSentences);
                resultData.Add(dataBySession);

            }

            return resultData;
        }

        public Dictionary<String, Dictionary<String, double>> GetNumberOfRightAndWrongSentencesByLevel()
        {

            var resultData = new Dictionary<String, Dictionary<String, double>>();

            foreach (var session in sessionsRoot.Sessions)
            {
                string level = session.Level.ToString();
                int rightSentences = 0;
                int wrongSentences = 0;
                foreach (var sentence in session.Sentences)
                {
                    if (sentence.WasIdentified)
                    {
                        rightSentences++;
                    }
                    else
                    {
                        wrongSentences++;
                    }
                }

                if (resultData.ContainsKey(level))
                {
                    resultData[level]["wrong"] = resultData[level]["wrong"] + wrongSentences;
                    resultData[level]["right"] = resultData[level]["right"] + rightSentences;
                }
                else
                {
                    resultData[level] = new Dictionary<String, double>();
                    resultData[level]["wrong"] = wrongSentences;
                    resultData[level]["right"] = rightSentences;
                }


            }
            return resultData;
        }

        /// <summary>
        /// Determines the number of identified and not identified sentences by each session. Every AggregatedSession represents a session.
        /// </summary>
        /// <returns></returns>
        public List<AggregatedSession> GetIdentifiedAndNotIdentifiedSentenceBySession()
        {
            List<AggregatedSession> result = new List<AggregatedSession>();

            foreach (var session in sessionsRoot.Sessions)
            {
                List<AggregatedObject> aggregatedObjects = new List<AggregatedObject>();
                int numberOfIdentified = 0;
                int numberOfNotIdentified = 0;

                foreach (var sentence in session.Sentences)
                    if (sentence.WasIdentified)
                        numberOfIdentified++;
                    else
                        numberOfNotIdentified++;


                aggregatedObjects.Add(new AggregatedObject("identified", numberOfIdentified));
                aggregatedObjects.Add(new AggregatedObject("notIdentified", numberOfNotIdentified));
                result.Add(new AggregatedSession(aggregatedObjects, session.Start));
            }

            return result;
        }
    }
}
