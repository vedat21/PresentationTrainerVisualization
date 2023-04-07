using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using static PresentationTrainerVisualization.MainWindow;

namespace PresentationTrainerVisualization
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        private JsonRoot jsonRoot;
        public Window1()
        {
            string json = File.ReadAllText("C:\\Users\\vedat\\Downloads\\Memory.json");
            jsonRoot = JsonConvert.DeserializeObject<JsonRoot>(json);
        }


        public void PlotTimeLineChart()
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


            // Convert DateTime[] to double[] before plotting
            double[] xs = numberOfSesions.Keys.Select(x => x.ToDateTime(TimeOnly.Parse("00:00 PM")).ToOADate()).ToArray();
            double[] ys = numberOfSesions.Values.Select(x => (double)x).ToArray();
            Array.Sort(xs, ys);

            TestPlot2.Plot.AddScatter(xs, ys);
            TestPlot2.Plot.XAxis.DateTimeFormat(true);
            TestPlot2.Plot.Title("Number of Sessions");

            TestPlot2.Refresh();

        }

        // JSON Modelle
        public class JsonRoot
        {
            [JsonProperty("sessions")]
            public List<Session> Sessions { get; set; }
        }

        public class Sentence
        {
            [JsonProperty("wasIdentified")]
            public bool WasIdentified { get; set; }

            [JsonProperty("sentence")]
            public string SentenceS { get; set; }
        }

        public class Session
        {
            [JsonProperty("start")]
            public DateTime Start { get; set; }

            [JsonProperty("level")]
            public int Level { get; set; }

            [JsonProperty("sentences")]
            public List<Sentence> Sentences { get; set; }
        }
    }

}

