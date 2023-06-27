using Newtonsoft.Json;
using PresentationTrainerVisualization.models.json;
using System;
using System.Collections.Generic;
using System.IO;


namespace PresentationTrainerVisualization.Helper
{
    public class ProcessedConfigurations
    {
        private static ProcessedConfigurations instance;

        public Configuration ConfigurationLastDays { get; set; }
        public Configuration ConfigurationTimeSpan { get; set; }

        private ConfigurationsRoot configurationsRoot;

        public ProcessedConfigurations()
        {
            instance = this;

            if (File.Exists(Constants.PATH_TO_CONFIG_DATA))
            {
                string json = File.ReadAllText(Constants.PATH_TO_CONFIG_DATA);
                configurationsRoot = JsonConvert.DeserializeObject<ConfigurationsRoot>(json);
                ConfigurationLastDays = configurationsRoot.Configurations.Find(x => x.Label == Constants.ConfigurationLabel.CONFIGURATION_LAST_X.ToString());
                ConfigurationTimeSpan = configurationsRoot.Configurations.Find(x => x.Label == Constants.ConfigurationLabel.CONFIGURATION_DATES.ToString());
            }
            else
            {
                configurationsRoot = new ConfigurationsRoot();
                configurationsRoot.Configurations = new List<Configuration>();

                ConfigurationTimeSpan = new Configuration
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-7)),
                    EndDate = DateOnly.FromDateTime(DateTime.Today),
                };
                ConfigurationLastDays = new Configuration
                {
                    CompareWithLastSessions = true,
                    NumberOfSessions = 7,
                };
            }

        }

        public static ProcessedConfigurations GetInstace()
        {
            if (instance == null)
                instance = new ProcessedConfigurations();

            return instance;
        }

        public void UpdateConfiguration(Configuration configuration)
        {
            // remove configuration if it already exists
            configurationsRoot.Configurations.RemoveAll(x => x.Label == configuration.Label);
            // add new configuration
            configurationsRoot.Configurations.Add(configuration);

            File.WriteAllText(Constants.PATH_TO_CONFIG_DATA, JsonConvert.SerializeObject(configurationsRoot));
        }

    }
}
