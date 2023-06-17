using Newtonsoft.Json;
using PresentationTrainerVisualization.helper;
using PresentationTrainerVisualization.models.json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;

namespace PresentationTrainerVisualization.Helper
{
    public class ProcessedConfigurationData
    {
        public Configuration ConfigurationLastDays { get; set; }
        public Configuration ConfigurationTimeSpan { get; set; }

        private ConfigurationsRoot configurationRoot;

        public ProcessedConfigurationData()
        {
            if (File.Exists(Constants.PATH_TO_CONFIG_DATA))
            {
                string json = File.ReadAllText(Constants.PATH_TO_CONFIG_DATA);
                configurationRoot = JsonConvert.DeserializeObject<ConfigurationsRoot>(json);
                ConfigurationLastDays = configurationRoot.Configurations.Find(x => x.Label == Constants.ConfigurationLabel.CONFIGURATION_LAST_X.ToString());
                ConfigurationTimeSpan = configurationRoot.Configurations.Find(x => x.Label == Constants.ConfigurationLabel.CONFIGURATION_DATES.ToString());
            }
            else
            {
                configurationRoot = new ConfigurationsRoot();
                configurationRoot.Configurations = new List<Configuration>();

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

        public void UpdateConfiguration(Configuration configuration)
        {
            // remove configuration if it already exists
            configurationRoot.Configurations.RemoveAll(x => x.Label == configuration.Label);
            // add new configuration
            configurationRoot.Configurations.Add(configuration);

            File.WriteAllText(Constants.PATH_TO_CONFIG_DATA, JsonConvert.SerializeObject(configurationRoot));
        }

    }
}
