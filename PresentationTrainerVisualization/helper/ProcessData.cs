using Newtonsoft.Json;
using PresentationTrainerVisualization.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainerVisualization.helper
{
    internal class ProcessData
    {

        private JsonRoot jsonRoot;


        public ProcessData()
        {
            // read json file
            string json = File.ReadAllText("C:\\Users\\vedat\\OneDrive\\Desktop\\BachelorNeu\\testdata\\Memory.json");
            jsonRoot = JsonConvert.DeserializeObject<JsonRoot>(json);
        }

        public List<double> GetNumberOfRightAndWrongSentences()
        {
            int rightSentences = 0;
            int wrongSentences = 0;
            foreach (var session in jsonRoot.Sessions)
            {
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
            }

            List<double> Data = new List<double>();
            Data.Add(rightSentences);
            Data.Add(wrongSentences);

            return Data;
        }

        public List<Dictionary<String, double>> GetNumberOfRightAndWrongSentencesBySession()
        {

            List<Dictionary<String, double>> resultData = new List<Dictionary<String, double>>();

            foreach (var session in jsonRoot.Sessions)
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

            foreach (var session in jsonRoot.Sessions)
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

            Trace.WriteLine(string.Join("\n", resultData["1"]));

            return resultData;
        }

        public Dictionary<DateOnly, int> GetNumberOfSessionsByDate()
        {
            // Count number of sessions by dateonly
            Dictionary<DateOnly, int> numberOfSesions = new Dictionary<DateOnly, int>();
            foreach (var session in jsonRoot.Sessions)
            {
                if (numberOfSesions.ContainsKey(DateOnly.FromDateTime(session.Start)))
                {
                    numberOfSesions[DateOnly.FromDateTime(session.Start)] = numberOfSesions[DateOnly.FromDateTime(session.Start)] + 1;
                }
                else
                {
                    numberOfSesions[DateOnly.FromDateTime(session.Start)] = 1;
                }
            }

            return numberOfSesions;
        }


    }
}
