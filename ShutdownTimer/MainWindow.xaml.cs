using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ShutdownTimer
{
    /// <summary>
    /// Logic for shuting down the computer after a certain amount of time
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TimeLeft.Text = "Time Left 0 hours 0 minutes 0 seconds";
            ShutDownAt.Text = $"Shutdown time: {DateTime.Now.ToString("HH:mm:ss")}";
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

        }

        private int totalSeconds;
        private DispatcherTimer timer;

        private void TimerStart(int hours, int minutes, int seconds)
        {
            totalSeconds = hours * 3600 + minutes * 60 + seconds;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (totalSeconds > 0)
            {
                totalSeconds--;
                int hours = totalSeconds / 3600;
                int minutes = (totalSeconds % 3600) / 60;
                int seconds = totalSeconds % 60;

                TimeLeft.Text = $"Time left : Hours: {hours} Minutes: {minutes} Seconds: {seconds}";
            }
            else
            {
                timer.Stop();
                TimeLeft.Text = "Time's up!";
                System.Diagnostics.Process.Start("shutdown", $"/s /t 0");

            }
        }

        private void CalculateShutdownTime(object sender, TextChangedEventArgs e)
        {
            DateTime now = DateTime.Now;
            int hoursInt = 0;
            int minutesInt = 0;
            int secondsInt = 0;
            if (!string.IsNullOrEmpty(Hours.Text))
                hoursInt = int.Parse(Hours.Text);
            if (!string.IsNullOrEmpty(Minutes.Text))
                minutesInt = int.Parse(Minutes.Text);
            if (!string.IsNullOrEmpty(Seconds.Text))
                secondsInt = int.Parse(Seconds.Text);

            DateTime shutdownTime = now.AddSeconds(hoursInt * 3600 + minutesInt * 60 + secondsInt);

            string formattedTime = shutdownTime.ToString("HH:mm:ss");
            ShutDownAt.Text = $"Shutdown time: {formattedTime}";

        }
        public void StartShutDown_Click(object sender, RoutedEventArgs e)
        {
            int hoursInt = 0;
            int minutesInt = 0;
            int secondsInt = 0;
            if (!string.IsNullOrEmpty(Hours.Text))
                hoursInt = int.Parse(Hours.Text);
            if (!string.IsNullOrEmpty(Minutes.Text))
                minutesInt = int.Parse(Minutes.Text);
            if (!string.IsNullOrEmpty(Seconds.Text))
                secondsInt = int.Parse(Seconds.Text);

            TimeLeft.Text = $"Time left : Hours: {hoursInt} Minutes: {minutesInt} Seconds: {secondsInt}";

            TimerStart(hoursInt, minutesInt, secondsInt);
            StartShutDown.IsEnabled = false;
        }
    }
}
