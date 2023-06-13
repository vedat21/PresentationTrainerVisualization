using FlyleafLib;
using FlyleafLib.MediaPlayer;
using PresentationTrainerVisualization.helper;
using PresentationTrainerVisualization.models.json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Action = PresentationTrainerVisualization.models.json.Action;

namespace PresentationTrainerVisualization
{
    /// <summary>
    /// Interaktionslogik für NewVideoPlayerWindow.xaml
    /// </summary>
    public partial class VideoPlayerWindow : Window
    {
        public Player VideoPlayer { get; set; }
        public Config Config { get; set; }

        private ListBox listBox;
        private ProcessedSessionsData processedSessionsData;
        private List<Action> actions;
        private Action selectedAction; // only has property logAction and Log
        private List<Sentence> sentences;
        private Sentence selectedSentence;
        private bool isActionAnnotation;
        private DispatcherTimer loopTimer;
        private DispatcherTimer videoTimer; // keep track of current time in video


        public string SampleVideo { get; set; } = "C:\\Users\\vedat\\OneDrive\\Desktop\\BachelorNeu\\testdata\\neueDaten\\Downloads\\5344641c.mp4";
        private bool isPlaying = false;
        private int selectedAnnotationIndex = 0;  // keeps track of which annotation is currently selected
        private bool doLoop = false;


        public VideoPlayerWindow(bool isActionAnnotation)
        {
            this.isActionAnnotation = isActionAnnotation;

            Config = new Config();
            Config.Video.BackgroundColor = Colors.DarkGray;
            VideoPlayer = new Player(Config);
            InitializeComponent();
            DataContext = this;
            processedSessionsData = new ProcessedSessionsData();
            actions = processedSessionsData.GetSelectedSession().Actions;
            sentences = processedSessionsData.GetSelectedSession().Sentences;
            videoTimer = new DispatcherTimer();
            loopTimer = new DispatcherTimer();

            Init();
        }

        private void Init()
        {
            // decide which listbox is loaded based on if it shows action or sentence annotations 
            listBox = isActionAnnotation ? (ListBox)FindName("ListBoxActions") : (ListBox)FindName("ListBoxSentences");
            listBox.ItemsSource = isActionAnnotation ? actions : sentences;
            listBox.Visibility = Visibility.Visible;

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



        private void SeekToSelectedPosition()
        {
            TimeSpan position = isActionAnnotation ? actions[selectedAnnotationIndex].Start : sentences[selectedAnnotationIndex].Start;
            VideoPlayer.SeekAccurate((int)position.TotalMilliseconds);
        }

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

        private void StartLoop()
        {
            SetCurrentIndex();

            TimeSpan startPosition = isActionAnnotation ? actions[selectedAnnotationIndex].Start : sentences[selectedAnnotationIndex].Start;
            VideoPlayer.SeekAccurate((int)startPosition.TotalMilliseconds);

            loopTimer = new DispatcherTimer();
            loopTimer.Interval = TimeSpan.FromMilliseconds(1500); // Adjust the interval as needed
            loopTimer.Tick += LoopTimerTick;
            loopTimer.Start();
            doLoop = true;
        }

        private void StopLoop()
        {
            loopTimer.Stop();
            loopTimer = null;
            doLoop = false;
        }

        private void LoopTimerTick(object sender, EventArgs e)
        {
            TimeSpan startPosition = isActionAnnotation ? actions[selectedAnnotationIndex].Start : sentences[selectedAnnotationIndex].Start;
            TimeSpan endPosition = isActionAnnotation ? actions[selectedAnnotationIndex].End : sentences[selectedAnnotationIndex].End;

            // Check if the current position is past the loop endtime
            if (VideoPlayer.CurTime >= (long)endPosition.TotalMilliseconds)
                VideoPlayer.SeekAccurate((int)startPosition.TotalMilliseconds); // Set the position back to the loop starttime
        }

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

        private void LeftArrowButtonClicked(object sender, RoutedEventArgs e)
        {
            selectedAnnotationIndex = Math.Max(selectedAnnotationIndex - 1, 0);
            listBox.SelectedIndex = selectedAnnotationIndex;

            SeekToSelectedPosition();
        }

        private void RightArrowButtonClicked(object sender, RoutedEventArgs e)
        {
            selectedAnnotationIndex = Math.Min(selectedAnnotationIndex + 1, actions.Count - 1);
            listBox.SelectedIndex = selectedAnnotationIndex;

            SeekToSelectedPosition();
        }
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

        public async void HandleListBox(object sender, RoutedEventArgs e)
        {
            if (isActionAnnotation)
                selectedAnnotationIndex = actions.IndexOf((Action)listBox.SelectedItem);
            else
                selectedAnnotationIndex = sentences.IndexOf((Sentence)listBox.SelectedItem);

            SeekToSelectedPosition();
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            VideoPlayer.Stop();
        }
    }
}
