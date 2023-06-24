using Newtonsoft.Json;
using PresentationTrainerVisualization.models.json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static PresentationTrainerVisualization.Helper.Constants;

namespace PresentationTrainerVisualization.Helper
{
    class ProcessedGoals
    {
        private GoalsRoot goalsRoot;

        public ProcessedGoals()
        {
            if (File.Exists(Constants.PATH_TO_GOALSCONFIG_DATA))
            {
                string json = File.ReadAllText(Constants.PATH_TO_GOALSCONFIG_DATA);
                goalsRoot = JsonConvert.DeserializeObject<GoalsRoot>(json);
            }
            else
            {
                goalsRoot = new GoalsRoot();
                goalsRoot.Goals = new List<Goal>();
            }
        }

        public Goal GetGoal(string label)
        {
            return goalsRoot.Goals.Find(x => x.Label == label);
        }

        public void UpdateGoal(Goal goal)
        {
            // remove goal if it already exists
            goalsRoot.Goals.RemoveAll(x => x.Label == goal.Label);
            // add new goal
            goalsRoot.Goals.Add(goal);
            // save in json file
            File.WriteAllText(Constants.PATH_TO_GOALSCONFIG_DATA, JsonConvert.SerializeObject(goalsRoot));
        }

        /// <summary>
        /// Gets list of all actions selected in GoalSetting page
        /// </summary>
        /// <returns></returns>
        public List<string> GetSelectedActionsLog()
        {
            List<string> selectedActions = new List<string>();

            Goal goalBadActions = GetGoal(GoalsLabel.BadActions.ToString());
            Goal goalGoodActions = GetGoal(GoalsLabel.GoodActions.ToString());


            // No actions selected in goal setting window.
            if (goalBadActions == null)
                selectedActions.Concat(Constants.BAD_ACTION_FROM_VIDEO.Keys.ToList());
            else
            {
                var selectedBadActions = goalBadActions.Description[GoalsDescription.list_of_bad_actions.ToString()];
                foreach(var action in selectedBadActions)
                    selectedActions.Add(action.ToString());
            }
            
            if (goalGoodActions == null)
                selectedActions.Concat(Constants.GOOD_ACTION_FROM_VIDEO.Keys.ToList());
            else
            {
                var selectedGoodActions = goalGoodActions.Description[GoalsDescription.list_of_good_actions.ToString()];
                foreach (var action in selectedGoodActions)
                    selectedActions.Add(action.ToString());
            }

            return selectedActions;
        }
    }
}
