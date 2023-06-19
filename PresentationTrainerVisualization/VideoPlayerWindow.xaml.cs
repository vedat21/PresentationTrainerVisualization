using FlyleafLib;
using FlyleafLib.MediaPlayer;
using PresentationTrainerVisualization.helper;
using PresentationTrainerVisualization.models.json;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Action = PresentationTrainerVisualization.models.json.Action;

namespace PresentationTrainerVisualization
{
    public partial class VideoPlayerWindow : Window
    {
        public string SampleVideo { get; set; } = "C:\\Users\\vedat\\OneDrive\\Desktop\\BachelorNeu\\testdata\\neueDaten\\Downloads\\5344641c.mp4";


        public Player VideoPlayer { get; set; }
        public Config Config { get; set; }

        private ProcessedSessions processedSessionsData;
        private List<Action> actions;
        private List<Sentence> sentences;
        private bool isActionAnnotation;
        private DispatcherTimer loopTimer;
        private DispatcherTimer videoTimer; // keep track of current time in video

        private int selectedAnnotationIndex = 0;  // keeps track of which annotation is currently selected
        private bool doLoop = false;
        private bool isPlaying = false;

        private ListBox usedListBox;


        public VideoPlayerWindow(bool isActionAnnotation)
        {
            this.isActionAnnotation = isActionAnnotation;

            Config = new Config();
            Config.Video.BackgroundColor = Colors.DarkGray;
            VideoPlayer = new Player(Config);
            InitializeComponent();
            DataContext = this;
            processedSessionsData = new ProcessedSessions();
            actions = processedSessionsData.GetSelectedSessionActions();
            sentences = processedSessionsData.SelectedSession.Sentences;
            videoTimer = new DispatcherTimer();
            loopTimer = new DispatcherTimer();

            Init();
        }

        /// <summary>
        /// Inits some more things
        /// </summary>
        private void Init()
        {
            // decide which listbox is loaded based on if it shows action or sentence annotations 
            usedListBox = isActionAnnotation ? (ListBox)FindName("ListBoxActions") : (ListBox)FindName("ListBoxSentences");
            usedListBox.ItemsSource = isActionAnnotation ? actions : sentences;
            usedListBox.Visibility = Visibility.Visible;

            // Open Video but dont play
            VideoPlayer.Open(SampleVideo);
            Pause();

            // show current time and total length of video.
            Duration.Text = TimeSpan.FromTicks(VideoPlayer.Duration).ToString("mm\\:ss");
            videoTimer.Interval = TimeSpan.FromMilliseconds(500);
            videoTimer.Tick += VideoTimeTick;


            if (!doLoop)
            {
                LoopButton.Background = Brushes.Gray;
                LoopButton.BorderBrush = Brushes.Gray;
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
            VideoPlayer.Play();
            isPlaying = true;
            playButtonIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
            videoTimer.Start();
        }

        public void Pause()
        {
            VideoPlayer.Pause();
            isPlaying = false;
            playButtonIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            videoTimer.Stop();
        }

        /// <summary>
        /// Seeks video to position of anmotation with use of selected index.
        /// </summary>
        private void SeekToSelectedPosition()
        {
            TimeSpan position = isActionAnnotation ? actions[selectedAnnotationIndex].Start : sentences[selectedAnnotationIndex].Start;
            position = position.Add(new TimeSpan(0, 0, 0, 0, -300)); // delay
            VideoPlayer.SeekAccurate((int)position.TotalMilliseconds);
        }

        /// <summary>
        /// Depending on the current position in video sets the selected index
        /// </summary>
        private void SetCurrentIndex()
        {
            var currentTime = TimeSpan.FromTicks(VideoPlayer.CurTime);

            if (isActionAnnotation)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    if (actions[i].Start <= currentTime && currentTime <= actions[i].End)
                    {
                        selectedAnnotationIndex = i;
                    }
                }
            }
            else
            {
                for (int i = 0; i < sentences.Count; i++)
                {
                    if (sentences[i].Start <= currentTime && currentTime <= sentences[i].End)
                    {
                        selectedAnnotationIndex = i;
                    }
                }
            }
        }

        /// <summary>
        /// Starts loop of frequence from start to end of object with selected index
        /// </summary>
        private void StartLoop()
        {
            SetCurrentIndex();

            TimeSpan startPosition = isActionAnnotation ? actions[selectedAnnotationIndex].Start : sentences[selectedAnnotationIndex].Start;
            startPosition = startPosition.Add(new TimeSpan(0, 0, 0, 0, -300));
            VideoPlayer.SeekAccurate((int)startPosition.TotalMilliseconds);

            loopTimer = new DispatcherTimer();
            loopTimer.Interval = TimeSpan.FromMilliseconds(1000); // Adjust the interval as needed
            loopTimer.Tick += LoopTimerTick;
            loopTimer.Start();
            doLoop = true;
        }

        /// <summary>
        /// Stops the Loop.
        /// </summary>
        private void StopLoop()
        {
            loopTimer.Stop();
            loopTimer = null;
            doLoop = false;
        }

        /// <summary>
        /// To keep track of loop and set video position to start of loop if video position is past loop endtime.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoopTimerTick(object sender, EventArgs e)
        {
            TimeSpan startPosition = isActionAnnotation ? actions[selectedAnnotationIndex].Start : sentences[selectedAnnotationIndex].Start;
            TimeSpan endPosition = isActionAnnotation ? actions[selectedAnnotationIndex].End : sentences[selectedAnnotationIndex].End;


            // Check if the current position is past the loop endtime
            if (VideoPlayer.CurTime >= (long)endPosition.TotalMilliseconds)
                VideoPlayer.SeekAccurate((int)startPosition.TotalMilliseconds); // Set the position back to the loop starttime
        }

        /// <summary>
        /// To keep track of current position in video and display it and also the annnotation that is shown at the moment.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoTimeTick(object sender, EventArgs e)
        {
            SetCurrentIndex();

            CurrentTime.Text = TimeSpan.FromTicks(VideoPlayer.CurTime).ToString("mm\\:ss");

            string x = isActionAnnotation ? actions[selectedAnnotationIndex].LogActionDisplay : sentences[selectedAnnotationIndex].SentenceText;
            VideoSubTitle.Text = x;

            string start = isActionAnnotation ? actions[selectedAnnotationIndex].Start.ToString("mm\\:ss") : sentences[selectedAnnotationIndex].Start.ToString("mm\\:ss");
            string end = isActionAnnotation ? actions[selectedAnnotationIndex].End.ToString("mm\\:ss") : sentences[selectedAnnotationIndex].End.ToString("mm\\:ss");

            VideoSubTime.Text = start + "-" + end;
        }

        /// <summary>
        /// Jumps to previous annotation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftArrowButtonClicked(object sender, RoutedEventArgs e)
        {
            selectedAnnotationIndex = Math.Max(selectedAnnotationIndex - 1, 0);
            usedListBox.SelectedIndex = selectedAnnotationIndex;

            SeekToSelectedPosition();
        }

        /// <summary>
        /// Jumps to next annotation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightArrowButtonClicked(object sender, RoutedEventArgs e)
        {
            selectedAnnotationIndex = Math.Min(selectedAnnotationIndex + 1, actions.Count - 1);
            usedListBox.SelectedIndex = selectedAnnotationIndex;

            SeekToSelectedPosition();
        }

        /// <summary>
        /// To start or end the loop.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoopButtonClicked(object sender, RoutedEventArgs e)
        {
            if (doLoop)
            {
                LoopButton.Background = Brushes.Gray;
                LoopButton.BorderBrush = Brushes.Gray;
                StopLoop();
            }
            else
            {
                LoopButton.Background = new SolidColorBrush(Color.FromRgb(103, 58, 183));
                LoopButton.BorderBrush = new SolidColorBrush(Color.FromRgb(103, 58, 183));
                StartLoop();
            }
        }

        /// <summary>
        /// Jumps to annotation that is the selcted item in listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void HandleListBox(object sender, RoutedEventArgs e)
        {
            if (isActionAnnotation)
                selectedAnnotationIndex = actions.IndexOf((Action)usedListBox.SelectedItem);
            else
                selectedAnnotationIndex = sentences.IndexOf((Sentence)usedListBox.SelectedItem);

            SeekToSelectedPosition();
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            VideoPlayer.Stop();
        }
    }
}
