using PresentationTrainerVisualization.models;
using PresentationTrainerVisualization.models.json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace PresentationTrainerVisualization.helper
{
    public static class ProcessedSessionsDataHelper
    {

        /// <summary>
        /// Returns a list with the average of all lists combined.
        /// </summary>
        /// <param name="aggregatedSessions"></param>
        /// <param name="numberOfLastSessions"></param>
        /// <returns></returns>
        public static AggregatedSession GetAverageOfLastSessions(List<AggregatedSession> aggregatedSessions, int numberOfLastSessions)
        {
            if (numberOfLastSessions == 1)
                return aggregatedSessions.Last();
            if (aggregatedSessions.Count == 1)
                return aggregatedSessions[0];

            List<AggregatedObject> result = new List<AggregatedObject>();
            Dictionary<String, double> resultSum = new Dictionary<String, double>();

           
            foreach (var aggregatedSession in aggregatedSessions.TakeLast(numberOfLastSessions))
                foreach (var aggregatedObject in aggregatedSession.AggregatedObjects)
                    if (resultSum.ContainsKey(aggregatedObject.Label))
                        resultSum[aggregatedObject.Label] = (resultSum[aggregatedObject.Label] + aggregatedObject.Count);
                    else
                        resultSum[aggregatedObject.Label] = aggregatedObject.Count;


            foreach (KeyValuePair<String, double> kvp in resultSum)
                result.Add(new AggregatedObject(kvp.Key, kvp.Value / numberOfLastSessions));
       

            return new AggregatedSession(result);
        }

        public static AggregatedSession GetAverageOfLastDays(List<AggregatedSession> aggregatedSessions, int numberOfLastDays)
        {

            DateTime lastIncludedDay = DateTime.Today.AddDays(-numberOfLastDays);
            Trace.WriteLine(lastIncludedDay.ToString());

            List<AggregatedObject> result = new List<AggregatedObject>();
            Dictionary<String, double> resultSum = new Dictionary<String, double>();

            int numberOfSessions = 0;

            foreach (var aggregatedSession in aggregatedSessions)

                if (DateTime.Compare(lastIncludedDay, aggregatedSession.SessionDate) <= 0)
                {
                    Trace.WriteLine(aggregatedSession.SessionDate.ToString());
                    foreach (var aggregatedObject in aggregatedSession.AggregatedObjects)
                        if (resultSum.ContainsKey(aggregatedObject.Label))
                            resultSum[aggregatedObject.Label] = (resultSum[aggregatedObject.Label] + aggregatedObject.Count);
                        else
                            resultSum[aggregatedObject.Label] = aggregatedObject.Count;

                    numberOfSessions++;
                }

            foreach (KeyValuePair<String, double> kvp in resultSum)
                result.Add(new AggregatedObject(kvp.Key, kvp.Value / numberOfSessions));

            return new AggregatedSession(result);
        }


        public static List<Session> GetSessionsOfLastDays(List<Session> sessions, int numberOfLastDays)
        {
            return sessions.Where(x => x.Start.AddDays(numberOfLastDays) > DateTime.Now).ToList();
        }

        public static List<Session> GetLastSessions(List<Session> sessions, int numberOfSessions)
        {
            return sessions.TakeLast(numberOfSessions).ToList();
        }


        /// <summary>
        /// Returns the sessions of last *numberOfLastDays days
        /// </summary>
        /// <param name="aggregatedSessions"></param>
        /// <param name="numberOfLastDays"></param>
        /// <returns></returns>
        public static List<AggregatedSession> GetAggregatedSessionsOfLastDays(List<AggregatedSession> aggregatedSessions, int numberOfLastDays)
        {
            return aggregatedSessions.Where(x => x.SessionDate.AddDays(numberOfLastDays) > DateTime.Now).ToList();
        }

        /// <summary>
        /// Returns the last *numberOfSessions sessions.
        /// </summary>
        /// <param name="aggregatedSessions"></param>
        /// <param name="numberOfSessions"></param>
        /// <returns></returns>
        public static List<AggregatedSession> GetLastAggregatedSessions(List<AggregatedSession> aggregatedSessions, int numberOfSessions)
        {
            return aggregatedSessions.TakeLast(numberOfSessions).ToList();
        }
    }
}
