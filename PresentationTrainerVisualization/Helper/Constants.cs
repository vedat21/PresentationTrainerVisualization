using System.Collections.Generic;

namespace PresentationTrainerVisualization.helper
{
    internal static class Constants
    {

        public static string PATH_TO_SESSION_DATA = "C:\\Users\\vedat\\OneDrive\\Desktop\\BachelorNeu\\testdata\\neueDaten\\Downloads\\PracticeSession.json";
        public static string PATH_TO_CONFIG_DATA = "C:\\Users\\vedat\\source\\repos\\PresentationTrainerVisualization\\PresentationTrainerVisualization\\UserConfig\\configurations.json";
        public static string PATH_TO_GOALSCONFIG_DATA = "C:\\Users\\vedat\\source\\repos\\PresentationTrainerVisualization\\PresentationTrainerVisualization\\UserConfig\\goals.json";


        // Die jeweiligen Button GoalSettingWindow müssen selben namen haben.
        public enum GoalsLabel
        {
            NumberOfSessionsForDays,
            DurationOfSession,
            MaxNumberOfBadActions,
            MinNumberOfGoodActions
        }

        public enum GoalsDescription
        {
            number_of_sessions,
            number_of_days,
            session_duration_min_minutes,
            session_duration_min_seconds,
            session_duration_max_minutes,
            session_duration_max_seconds,
            max_number_of_bad_actions,
            min_number_of_good_actions,
        }

        public enum ConfigurationLabel
        {
            CONFIGURATION_LAST_X,
            CONFIGURATION_DATES,
        }

        public static Dictionary<string, string> ACTION_FROM_VIDEO = new Dictionary<string, string>()
        {
            {"long_pause", "Long Pause"},
            {"lefthandnotvisible", "Left Hand unvisable"},
            {"low_volume", "Low Volume"},
            {"dancing", "Dancing"},
            {"big_gesture", "Big Gesture"},
            {"gesture", "Gesture"},
            {"good_pause", "Good Pause"},
            {"hands_not_moving", "Hands not moving" },
            {"long_talk", "Long Talk" },
            {"armscrossed", "Arms crossed" },
            {"legscrossed", "Legs crossed" },
            {"high_volume", "High Volume" }
        };

    }
}

