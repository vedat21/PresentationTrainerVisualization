using FlyleafLib.MediaPlayer;
using FlyleafLib;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using PresentationTrainerVisualization.helper;
using PresentationTrainerVisualization.models.json;
using System.Collections.Generic;
using System.Windows.Threading;
using Action = PresentationTrainerVisualization.models.json.Action;

namespace PresentationTrainerVisualization
{
    /// <summary>
    /// Interaktionslogik für NewVideoPlayerWindow.xaml
    /// </summary>
    public partial class VideoPlayerWindow : Window
    {
        public Player Player { get; set; }
        public Config Config { get; set; }

        public ListBox istBox;
        private ProcessedSessionsData processedSessionsData;
        private List<Action> actions;
        private Action selectedAction; // only has property logAction and Log
        private List<Sentence> sentences;
        private Sentence selectedSentence;
        private bool isActionAnnotation;
        private DispatcherTimer loopTimer;

        public string SampleVideo { get; set; } = "C:\\Users\\vedat\\OneDrive\\Desktop\\BachelorNeu\\testdata\\neueDaten\\Downloads\\5344641c.mp4";
        private bool isPlaying = false;
        private int selectedAnnotationIndex = 0;  // keeps track of which annotation is currently selected
        private bool doLoop = false;
        private DispatcherTimer timer; // keep track of current time in video


        public VideoPlayerWindow(bool isActionAnnotation)
        {
            this.isActionAnnotation = isActionAnnotation;

            Config = new Config();
            Config.Video.BackgroundColor = Colors.DarkGray;
            Player = new Player(Config);
            InitializeComponent();
            DataContext = this;
            processedSessionsData = new ProcessedSessionsData();
            actions = processedSessionsData.GetActionsFromLastSession();
            sentences = processedSessionsData.GetSentencesFromLastSession();
            timer = new DispatcherTimer();
           

            Init();
        }

        private void Init()
        {
            // decide which listbox is loaded based on if it shows action or sentence annotations 
            istBox = isActionAnnotation ? (ListBox)FindName("ListBoxActions") : (ListBox)FindName("ListBoxSentences");
            istBox.ItemsSource = isActionAnnotation ? actions : sentences;
            istBox.Visibility = Visibility.Visible;

            // Open Video but dont play
            Player.Open(SampleVideo);
            Pause();

            // show current time and total length of video.
            Duration.Text = TimeSpan.FromTicks(Player.Duration).ToString("mm\\:ss");

            // init timer
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentTime.Text = TimeSpan.FromTicks(Player.CurTime).ToString("mm\\:ss");
        }

        private void SeekToSelectedPosition()
        {
            TimeSpan position = isActionAnnotation ? TimeSpan.Parse(actions[selectedAnnotationIndex].Start) : sentences[selectedAnnotationIndex].Start;
            int totalMilliSeconds = (int)position.TotalMilliseconds;
            Player.SeekAccurate(totalMilliSeconds);
        }

        private void StartLoop()
        {
            // Set the initial position to the loop start time
            TimeSpan position = isActionAnnotation ? TimeSpan.Parse(actions[selectedAnnotationIndex].Start) : sentences[selectedAnnotationIndex].Start;
            long totalMilliSeconds = (long)position.TotalMilliseconds;
            Player.CurTime = totalMilliSeconds;


            // Create and start the timer for checking the current position
            loopTimer = new DispatcherTimer();
            loopTimer.Interval = TimeSpan.FromMilliseconds(1000); // Adjust the interval as needed
            loopTimer.Tick += LoopTimer_Tick;
            loopTimer.Start();
            doLoop = true;
        }

        private void StopLoop()
        {

            // Stop and reset the timer
            loopTimer?.Stop();
            loopTimer = null;
            doLoop = false;


            // Stop playing the video
            Pause();
        }
        private void LoopTimer_Tick(object sender, EventArgs e)
        {

            // Set the initial position to the loop start time
            TimeSpan position = isActionAnnotation ? TimeSpan.Parse(actions[selectedAnnotationIndex].Start) : sentences[selectedAnnotationIndex].Start;
            long totalMilliSeconds = (long)position.TotalMilliseconds;


            // Check if the current position is past the loop end time
            if (Player.CurTime >= totalMilliSeconds)
            {
                // Set the position back to the loop start time
                Player.CurTime = totalMilliSeconds;
                Trace.Write(Player.CurTime.ToString());
            }
        }

        /// <summary>
        /// Handles when play/stop button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PlayMediaButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!isPlaying)
                Play();
            else
                Pause();
        }

        public void Play()
        {
            Player.Play();
            isPlaying = true;
            playButtonIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
            timer.Start();
        }

        public void Pause()
        {
            Player.Pause();
            isPlaying = false;
            playButtonIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            timer.Stop();
        }

        private void LeftArrowButtonClicked(object sender, RoutedEventArgs e)
        {
            selectedAnnotationIndex = Math.Max(selectedAnnotationIndex - 1, 0);
            istBox.SelectedIndex = selectedAnnotationIndex;
            SeekToSelectedPosition();
        }

        private void RightArrowButtonClicked(object sender, RoutedEventArgs e)
        {
            selectedAnnotationIndex = Math.Min(selectedAnnotationIndex + 1, actions.Count - 1);
            istBox.SelectedIndex = selectedAnnotationIndex;
            SeekToSelectedPosition();
        }
        private void LoopButtonClicked(object sender, RoutedEventArgs e)
        {

            if (doLoop)
            {
                LoopButton.Background = System.Windows.Media.Brushes.Gray;
                StopLoop();
            }
            else
            {
                LoopButton.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(103, 58, 183));
                StartLoop();
            }

        }

        public async void HandleListBox(object sender, RoutedEventArgs e)
        {
            if (isActionAnnotation)
            {
                selectedAction = (Action)istBox.SelectedItem;
                selectedAnnotationIndex = actions.IndexOf(selectedAction);
            }
            else
            {
                selectedSentence = (Sentence)istBox.SelectedItem;
                selectedAnnotationIndex = sentences.IndexOf(selectedSentence);
            }

            SeekToSelectedPosition();
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            Player.Stop();
        }
    }
}
