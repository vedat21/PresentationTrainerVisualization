using Newtonsoft.Json;
using PresentationTrainerVisualization.models;
using PresentationTrainerVisualization.models.json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace PresentationTrainerVisualization.helper
{
    public class ProcessedSessionsData
    {
        private SessionsRoot sessionsRoot;

        private ConfigurationRoot configurationRoot;
        private Configuration configurationLastDays;
        private Configuration configurationTimeSpan;

        public ProcessedSessionsData()
        {
            sessionsRoot = JsonConvert.DeserializeObject<SessionsRoot>(File.ReadAllText(Constants.PATH_TO_SESSION_DATA));

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

                configurationTimeSpan = new Configuration
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-7)),
                    EndDate = DateOnly.FromDateTime(DateTime.Today),
                };
                configurationLastDays = new Configuration
                {
                    IsLastSessions = true,
                    LastDaysOrSessions = 7,
                };
            }

            GetAverageNumberOfRecongnisedSentencesByTime();
            GetAllActionsFromLastSession();
        }

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

        public int GetNumberOfSessions()
        {
            return sessionsRoot.Sessions.Count;
        }

        public TimeSpan GetDurationOfLastSession()
        {
            return sessionsRoot.Sessions.Last().Duration;

        }

        public List<Sentence> GetAllSentencesFromLastSession()
        {
            return sessionsRoot.Sessions.Last().Sentences;
        }

        public List<models.json.Action> GetAllActionsFromLastSession()
        {
            var actions = sessionsRoot.Sessions.Last().Actions;
            foreach (var action in actions)
            {
                if (action.Start.StartsWith("-"))
                {
                    action.Start = action.Start.Substring(1, action.Start.Length-1);
                }
            }
            actions = actions.OrderBy(x => x.Start).ToList();

            return sessionsRoot.Sessions.Last().Actions;
        }

        public int NumberOfDaysBetweenFirstSessionAndToday()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            DateOnly firstSession = DateOnly.FromDateTime( sessionsRoot.Sessions.First().Start);
            DateTime x = sessionsRoot.Sessions.First().Start;

            foreach(var session in sessionsRoot.Sessions)
            {
                Trace.WriteLine("Sesion " + session.VideoId  + " " + session.Start);
            }

            Trace.WriteLine( firstSession);

            return today.DayNumber - firstSession.DayNumber;
        }

        public List<AggregatedSession> GetBadActionsBySession(bool mistake = true)
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

        public double GetAverageNumberOfRecongnisedSentencesByTime()
        {
            double numberOfRecongnised = 0;
            double numberOfNotRecongnised = 0;

            foreach (var session in sessionsRoot.Sessions)
            {
                DateOnly sessionDate = DateOnly.FromDateTime(session.Start);

                if (configurationTimeSpan.StartDate <= sessionDate && configurationTimeSpan.EndDate >= sessionDate)
                {

                    foreach (var sentence in session.Sentences)
                        if (sentence.WasIdentified)
                            numberOfRecongnised++;
                        else
                            numberOfNotRecongnised++;
                }
            }

            double result = (numberOfRecongnised / (numberOfRecongnised + numberOfNotRecongnised)) * 100;

            return Math.Round(result, 2);
        }

        public double GetAverageNumberOfBadActionsByTime()
        {
            double numberOfBadActions = 0;
            double numberOfGoodActions = 0;

            foreach (var session in sessionsRoot.Sessions)
            {
                DateOnly sessionDate = DateOnly.FromDateTime(session.Start);

                if (configurationTimeSpan.StartDate <= sessionDate && configurationTimeSpan.EndDate >= sessionDate)
                {

                    foreach (var action in session.Actions)
                        if (action.Mistake)
                            numberOfBadActions++;
                        else
                            numberOfGoodActions++;
                }
            }

            double result = (numberOfBadActions / (numberOfBadActions + numberOfGoodActions)) * 100;

            return Math.Round(result, 1);
        }

        public double GetAverageNumberOfGoodActionsByTime()
        {
            double numberOfBadActions = 0;
            double numberOfGoodActions = 0;

            foreach (var session in sessionsRoot.Sessions)
            {
                DateOnly sessionDate = DateOnly.FromDateTime(session.Start);

                if (configurationTimeSpan.StartDate <= sessionDate && configurationTimeSpan.EndDate >= sessionDate)
                {

                    foreach (var action in session.Actions)
                        if (action.Mistake)
                            numberOfBadActions++;
                        else
                            numberOfGoodActions++;
                }
            }

            double result = (numberOfGoodActions / (numberOfBadActions + numberOfGoodActions)) * 100;

            return Math.Round(result, 1);
        }

        public int GetNumberOfSessionsByTime()
        {
            int numberOfSessions = 0;

            foreach (var session in sessionsRoot.Sessions)
            {
                DateOnly sessionDate = DateOnly.FromDateTime(session.Start);
                if (configurationTimeSpan.StartDate <= sessionDate && configurationTimeSpan.EndDate >= sessionDate)
                    numberOfSessions++;
            }

            return numberOfSessions;
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

                if (configurationTimeSpan.StartDate <= sessionDate && configurationTimeSpan.EndDate >= sessionDate)
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

        public int GetNumberOfSessionsToday()
        {
            int numberOfSessions = 0;

            foreach (var session in sessionsRoot.Sessions)
            {
                if (session.Start.Date == DateTime.Today)
                    numberOfSessions++;
            }

            return numberOfSessions;
        }

        public TimeSpan GetTotalTimeSpent()
        {
            TimeSpan totalTime = new TimeSpan();
            foreach (var session in sessionsRoot.Sessions)
            {
                TimeSpan timeDifference = session.End - session.Start;
                totalTime += timeDifference;
            }

            return totalTime;
        }

        /// <summary>
        /// Returns the number of sessions by each date only.
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<DateOnly, int> GetNumberOfSessionsByDate()
        {
            // Count number of sessions by dateonly
            SortedDictionary<DateOnly, int> numberOfSesions = new SortedDictionary<DateOnly, int>();

            foreach (var session in sessionsRoot.Sessions)
            {
                var date = DateOnly.FromDateTime(session.Start);

                if (numberOfSesions.ContainsKey(date))
                    numberOfSesions[date] = numberOfSesions[date] + 1;
                else
                    numberOfSesions[date] = 1;
            }

            return numberOfSesions;
        }

        /// <summary>
        /// Determines the number of identified actions recognized with video by each session. Every list of AggregatedObjects represents a session.
        /// Parameter mistake determines if actions that are a mistake or actions that are not a mistake should be included.
        /// </summary>
        /// <param name="mistake"></param>
        /// <returns></returns>
        public List<AggregatedSession> GetAggregatedActionsBySession(bool mistake = true)
        {
            List<AggregatedSession> result = new List<AggregatedSession>();
            List<string> allPossibleLabels = new List<string>();

            foreach (var session in sessionsRoot.Sessions)
            {
                List<AggregatedObject> aggregatedObjects = new List<AggregatedObject>();
                Dictionary<String, int> countOfLabels = new Dictionary<String, int>();

                foreach (var action in session.Actions)
                {
                    if (mistake == action.Mistake)
                    {
                        string logAction = action.LogAction.ToLower();
                        if (countOfLabels.ContainsKey(logAction))
                            countOfLabels[logAction] = countOfLabels[logAction] + 1;
                        else
                            countOfLabels[logAction] = 1;

                        // every list of aggregated objects should have the same number of labels.
                        if (!allPossibleLabels.Contains(logAction))
                            allPossibleLabels.Add(logAction);
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
    }
}
