using Newtonsoft.Json;
using PresentationTrainerVisualization.helper;
using PresentationTrainerVisualization.models.json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using static PresentationTrainerVisualization.helper.Constants;
using Action = PresentationTrainerVisualization.models.json.Action;

namespace PresentationTrainerVisualization.windows
{

    public partial class VideoPlayerWindowOld : Window
    {
        // videoplayer can be inistiated with senteces or actions. This bool is to know which is chosen.
        public bool IsActionVideo { get; set; }

        private ListBox listBox;
        private MediaElement mediaElement;
        private ProcessedSessionsData processedSessionsData;

        private DispatcherTimer videoCurrentTime;
        private DispatcherTimer loopTimer;
        private bool isPlaying = true;
        private bool doLoop = true;

        private List<Action> actions;
        private Action selectedAction; // only has property logAction and Log

        private List<Sentence> sentences;
        private Sentence selectedSentence;

        // keeps track of which annotation is currently selected
        private int selectedAnnotationIndex = 0;

        private TimeSpan loopStartTime = TimeSpan.FromSeconds(10);  // Start time of the loop
        private TimeSpan loopEndTime = TimeSpan.FromSeconds(13);    // End time of the loop


        public VideoPlayerWindowOld()
        {
            // Has to be initialized before InitializeComponent is called.
            IsActionVideo = false; 

            InitializeComponent();
            mediaElement = (MediaElement)FindName("VideoPlayer");
            listBox = IsActionVideo ? (ListBox)FindName("ListBoxActions") : (ListBox)FindName("ListBoxSentences");

            processedSessionsData = new ProcessedSessionsData();
            actions = processedSessionsData.GetActionsFromLastSession();
            sentences = processedSessionsData.GetSentencesFromLastSession();


            // Initialize the timer
            //     videoCurrentTime = new DispatcherTimer();
            //     videoCurrentTime.Interval = TimeSpan.FromSeconds(1); // Update time every second
            //     videoCurrentTime.Tick += Timer_Tick;
            //    mediaElement.Loaded += (sender, e) =>
            //    {
            //         videoCurrentTime.Start();
            //     };

            // Stop the timer when the media is unloaded
            //   mediaElement.Unloaded += (sender, e) =>
            //   {
            //       videoCurrentTime.Stop();
            //   };

            InitValues();
            StartLoop();
            InitListBox();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan videoPosition = mediaElement.Position;
            // Update the time TextBlock

            selectedAction = actions.FirstOrDefault(x => videoPosition >= TimeSpan.Parse(x.Start) && videoPosition <= TimeSpan.Parse(x.End));
            selectedAnnotationIndex = actions.FindIndex(x => videoPosition >= TimeSpan.Parse(x.Start) && videoPosition <= TimeSpan.Parse(x.End));

        }

        private void InitListBox()
        {
            if (IsActionVideo)
                listBox.ItemsSource = actions;
            else
                listBox.ItemsSource = sentences;
        }

        public async void HandleListBox(object sender, RoutedEventArgs e)
        {
            if (IsActionVideo)
            {
                selectedAction = (Action)listBox.SelectedItem;
                selectedAnnotationIndex = actions.IndexOf(selectedAction);
            }
            else
            {
                selectedSentence = (Sentence)listBox.SelectedItem;
                selectedAnnotationIndex = sentences.IndexOf(selectedSentence);
            }

            SmoothSeekToPosition();
        }

        private void SeekToPosition(TimeSpan position)
        {
            if (mediaElement.NaturalDuration.HasTimeSpan && position <= mediaElement.NaturalDuration.TimeSpan)
            {
                mediaElement.MediaOpened += (sender, e) =>
                {
                    mediaElement.Position = position;
                    mediaElement.MediaOpened -= null; // Remove the event handler once the position is set
                };
            }
        }

        private async Task SmoothSeekToPosition()
        {
            TimeSpan position = TimeSpan.Parse(actions[selectedAnnotationIndex].Start);


            const int framesPerSecond = 30;
            const int frameDurationMilliseconds = 1000 / framesPerSecond;

            if (mediaElement.NaturalDuration.HasTimeSpan && position <= mediaElement.NaturalDuration.TimeSpan)
            {
                mediaElement.Pause(); // Pause the video

                double targetFrame = framesPerSecond * position.TotalSeconds;
                double currentFrame = framesPerSecond * mediaElement.Position.TotalSeconds;

                // Calculate the number of frames to skip
                double framesToSkip = targetFrame - currentFrame;

                // Calculate the duration between each frame step
                int stepDuration = (int)(frameDurationMilliseconds / Math.Abs(framesToSkip));

                // Determine the direction of seeking
                int stepDirection = framesToSkip > 0 ? 1 : -1;

                // Adjust the playback rate to maintain audio-video synchronization
                mediaElement.SpeedRatio = 0.001; // Set an initial slow playback rate

                // Perform frame stepping to seek smoothly
                for (int i = 0; i < Math.Abs(framesToSkip); i++)
                {
                    mediaElement.Position = TimeSpan.FromSeconds(currentFrame / framesPerSecond);
                    currentFrame += stepDirection;

                    await Task.Delay(stepDuration);
                }

                mediaElement.SpeedRatio = 1.0; // Reset the playback rate to normal
                mediaElement.Play(); // Resume playing the video
            }
        }

        public void TestIwas(object sender, RoutedEventArgs e)
        {
            List<Action> actions = processedSessionsData.GetSelectedSession().Actions;


            string x = actions[0].Start;
            TimeSpan timeSpan = TimeSpan.Parse("00:00:53.3053082");
            mediaElement.Position = timeSpan;
            mediaElement.Position += TimeSpan.FromMilliseconds(10);
            if (mediaElement.Position >= mediaElement.NaturalDuration.TimeSpan)
            {
                Trace.Write("AAAAAAA");
                mediaElement.Position -= TimeSpan.FromMilliseconds(100);
            }
            //  StopMediaPlayer();


        }

        /// <summary>
        /// Handles when play/stop button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnButtonClickPlayMedia(object sender, RoutedEventArgs e)
        {
            if (!isPlaying)
                PlayMediaPlayer();
            else
                StopMediaPlayer();
        }

        public void PlayMediaPlayer()
        {
            mediaElement.Play();
            isPlaying = true;
            playButtonIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
        }

        public void StopMediaPlayer()
        {
            mediaElement.Pause();
            isPlaying = false;
            playButtonIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
        }


        // Stop the media.
        void OnMouseDownStopMedia(object sender, RoutedEventArgs e)
        {

            // The Stop method stops and resets the media to be played from
            // the beginning.
            //  myMediaElement.Stop();
            mediaElement.Position = new TimeSpan(0, 0, 0);
        }

        /// <summary>
        /// Handles volume change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            if (mediaElement != null)
                mediaElement.Volume = ((Slider)FindName("VolumeSlider")).Value;
        }

        /// <summary>
        /// Handles the speed of video.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ChangeMediaSpeedRatio(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            if (mediaElement != null)
                mediaElement.SpeedRatio = (double)speedRatioSlider.Value;
        }

        private void StartLoop()
        {
            // Set the initial position to the loop start time
            mediaElement.Position = TimeSpan.Parse(actions[selectedAnnotationIndex].Start);

            // Subscribe to the MediaEnded event
            mediaElement.MediaEnded += MediaElement_MediaEnded;

            // Create and start the timer for checking the current position
            loopTimer = new DispatcherTimer();
            loopTimer.Interval = TimeSpan.FromMilliseconds(300); // Adjust the interval as needed
            loopTimer.Tick += LoopTimer_Tick;
            loopTimer.Start();
            doLoop = true;

            // Start playing the video
            mediaElement.Play();
        }

        private void StopLoop()
        {
            // Unsubscribe from the MediaEnded event
            mediaElement.MediaEnded -= MediaElement_MediaEnded;

            // Stop and reset the timer
            loopTimer?.Stop();
            loopTimer = null;
            doLoop = false;


            // Stop playing the video
            mediaElement.Stop();
        }

        private void LoopTimer_Tick(object sender, EventArgs e)
        {

            // Check if the current position is past the loop end time
            if (mediaElement.Position >= TimeSpan.Parse(actions[selectedAnnotationIndex].End))
            {
                // Set the position back to the loop start time
                mediaElement.Position = TimeSpan.Parse(actions[selectedAnnotationIndex].Start);
            }
        }



        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Stop and reset the timer
            loopTimer?.Stop();
            loopTimer = null;

            // Start playing the video again
            mediaElement.Play();
        }

        void InitValues()
        {
            if (mediaElement != null)
            {
                mediaElement.Volume = (double)VolumeSlider.Value;
                mediaElement.SpeedRatio = (double)speedRatioSlider.Value;
            }
        }

        private void LeftButtonClicked(object sender, RoutedEventArgs e)
        {
            selectedAnnotationIndex--;
            selectedAnnotationIndex = Math.Max(selectedAnnotationIndex, 0);
            SmoothSeekToPosition();
        }

        private void RightButtonClicked(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(selectedAnnotationIndex);
            selectedAnnotationIndex++;
      //      selectedActionIndex = Math.Min(selectedActionIndex, actions.Count - 1);
            SmoothSeekToPosition();
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

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            VideoPlayer.Stop();
            VideoPlayer.Source = null;
        }
    }
}
