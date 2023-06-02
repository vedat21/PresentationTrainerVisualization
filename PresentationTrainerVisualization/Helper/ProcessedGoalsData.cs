using Newtonsoft.Json;
using PresentationTrainerVisualization.models.json;
using System.IO;

namespace PresentationTrainerVisualization.helper
{
    class ProcessedGoalsData
    {
        private GoalsRoot goalsRoot;

        public ProcessedGoalsData()
        {
            goalsRoot = JsonConvert.DeserializeObject<GoalsRoot>(File.ReadAllText(Constants.PATH_TO_GOALSCONFIG_DATA));
        }

        public Goal GetGoalWithLabel(string label)
        {
            return goalsRoot.Goals.Find(x => x.Label == label);
        }
    }
}
