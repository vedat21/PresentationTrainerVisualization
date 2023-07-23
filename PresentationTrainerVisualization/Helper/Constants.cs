using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace PresentationTrainerVisualization.Helper
{
    internal static class Constants
    {
        // paths
        public static string PATH_TO_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory;
        public static string PATH_TO_SESSION_DATA = "C:\\Users\\vedat\\OneDrive\\Desktop\\BachelorNeu\\testdata\\neueDaten\\Downloads\\PracticeSession.json"; // PATH TO SESSION DATA 
        public static string PATH_TO_CONFIG_DATA = PATH_TO_DIRECTORY + @"..\\..\\..\\UserConfig\\configurations.json";
        public static string PATH_TO_GOALSCONFIG_DATA = PATH_TO_DIRECTORY + @"..\\..\\..\\UserConfig\\goals.json";

        // Color used in dashboards
        public static Color GOOD_INDICATOR_COLOR = Color.Green;
        public static Color BAD_INDICATOR_COLOR = Color.Maroon;
        public static Color TIMELINE_COLOR = Color.LightSkyBlue;

        public static System.Windows.Media.Color GOOD_INDICATOR_COLOR_MEDIA = System.Windows.Media.Color.FromArgb(GOOD_INDICATOR_COLOR.A, GOOD_INDICATOR_COLOR.R, GOOD_INDICATOR_COLOR.G, GOOD_INDICATOR_COLOR.B);
        public static System.Windows.Media.Color BAD_INDICATOR_COLOR_MEDIA = System.Windows.Media.Color.FromArgb(BAD_INDICATOR_COLOR.A, BAD_INDICATOR_COLOR.R, BAD_INDICATOR_COLOR.G, BAD_INDICATOR_COLOR.B);

        // When user does not set configuration
        public static int CONFIG_DEFAULT_NUMBER_OF_SESSIONS = 7;

        // The Buttons in GoalSettingWindow should habe same name
        public enum GoalsLabel
        {
            NumberOfSessionsForDays,
            DurationOfSession,
            BadActions,
            GoodActions,
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
            list_of_bad_actions,
            list_of_good_actions,
        }

        public enum ConfigurationLabel
        {
            CONFIGURATION_LAST_X,
            CONFIGURATION_DATES,
        }

        public static Dictionary<string, string> ACTIONS_FROM_VIDEO = new Dictionary<string, string>()
        {
            {"ARMSCROSSED", "Arms crossed" },
            {"BIG_GESTURE", "Big Gesture"},
            {"DANCING", "Dancing"},
            {"GESTURE", "Gesture"},
            {"GOOD_PAUSE", "Good Pause"},
            {"HANDS_NOT_MOVING", "Hands not moving" },
            {"HIGH_VOLUME", "High Volume" },
            {"HANDS_NEAR_FACE", "Hands near Face" },
            {"HMMMM", "HMMMM" },
            {"LEFTHANDNOTVISIBLE", "Left Hand unvisable"},
            {"LEGSCROSSED", "Legs crossed" },
            {"LONG_PAUSE", "Long Pause"},
            {"LONG_TALK", "Long Talk" },
            {"LOW_VOLUME", "Low Volume"},
            {"RIGHTHANDNOTVISIBLE",  "Right Hand unvisable"},
            {"SMILE", "Smile" },
        };

        public static Dictionary<string, string> BAD_ACTIONS_FROM_VIDEO = new Dictionary<string, string>()
        {
            {"ARMSCROSSED", "Arms crossed" },
            {"DANCING", "Dancing"},
            {"HANDS_NOT_MOVING", "Hands not moving" },
            {"HIGH_VOLUME", "High Volume" },
            {"HANDS_NEAR_FACE", "Hands near Face" },
            {"HMMMM", "HMMMM" },
            {"LEFTHANDNOTVISIBLE", "Left Hand unvisable"},
            {"LEGSCROSSED", "Legs crossed" },
            {"LONG_PAUSE", "Long Pause"},
            {"LONG_TALK", "Long Talk" },
            {"LOW_VOLUME", "Low Volume"},
            {"RIGHTHANDNOTVISIBLE",  "Right Hand unvisable"},
        };

        public static Dictionary<string, string> GOOD_ACTIONS_FROM_VIDEO = new Dictionary<string, string>()
        {
            {"BIG_GESTURE", "Big Gesture"},
            {"GESTURE", "Gesture"},
            {"GOOD_PAUSE", "Good Pause"},
            {"RESETPOSTURE", "Reset Posture" },
            {"SMILE", "Smile" },
        };

    }
}

