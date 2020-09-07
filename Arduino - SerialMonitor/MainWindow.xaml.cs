using System;
using System.Windows;
using System.Windows.Media;
using System.IO.Ports;
using System.Windows.Threading;

namespace Arduino___SerialMonitor
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort arduino;
        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            autoScroll_check.IsChecked = true;
            odbieraj_box.IsEnabled = false;
            Send_button.IsEnabled = false;
            odbieraj_box.IsChecked = true;
            porty();
        }

        private void porty()
        {
            string[] porty = SerialPort.GetPortNames();
            foreach (string ports in porty)
            {
                comboBox.Items.Add(ports);
            }
            arduino = new SerialPort();
            arduino.BaudRate = 9600;
            arduino.Parity = Parity.None;
            arduino.StopBits = StopBits.One;
            arduino.DataBits = 8;
            arduino.Handshake = Handshake.None;
            arduino.RtsEnable = true;
            arduino.DtrEnable = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (arduino.IsOpen)
                {
                    SerialMonitor_box.AppendText(arduino.ReadLine());
                    if (autoScroll_check.IsChecked == true)
                    {
                        SerialMonitor_box.ScrollToEnd();
                    }
                }
            }
            catch(System.Exception)
            {
                arduino_label.Foreground = Brushes.Red;
                arduino_label.Content = "Stan: Wystąpił błąd";
            }
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                arduino.PortName = comboBox.Text;
                arduino.Open();
                odbieraj_box.IsEnabled = true;
                Send_button.IsEnabled = true;
                arduino_label.Foreground = Brushes.Green;
                arduino_label.Content = "Stan: Połączono";
            }

            catch (SystemException)
            {
                arduino_label.Foreground = Brushes.Red;
                arduino_label.Content = "Stan: Wystąpił błąd";
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                arduino.Close();
                odbieraj_box.IsEnabled = false;
                Send_button.IsEnabled = false;
                arduino_label.Foreground = Brushes.Red;
                arduino_label.Content = "Stan: Rozłączono";
            }

            catch (SystemException)
            {
                arduino_label.Foreground = Brushes.Red;
                arduino_label.Content = "Stan: Wystąpił błąd";
            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                arduino.Close();
                Environment.Exit(0);
            }

            catch (SystemException)
            {
                arduino_label.Foreground = Brushes.Red;
                arduino_label.Content = "Stan: Wystąpił błąd";
            }
        }

        private void refresh_button_Click(object sender, RoutedEventArgs e)
        {
            comboBox.Items.Clear();
            porty();
            arduino_label.Foreground = Brushes.Goldenrod;
            arduino_label.Content = "Stan: Odświeżono";
        }

        private void odbieraj_box_Checked_2(object sender, RoutedEventArgs e)
        {
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, Convert.ToInt32(0.01));
            timer.Start();
        }

        private void odbieraj_box_Unchecked_1(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void send_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(send_box.Text))
            {
                string text = send_box.Text;
                arduino.WriteLine(text);
                send_box.Clear();
            }
        }

        private void clear_button_Click(object sender, RoutedEventArgs e)
        {
            SerialMonitor_box.Clear();
        }
    }
}