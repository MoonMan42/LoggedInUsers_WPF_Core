using Microsoft.Win32;
using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LoggedInUsers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComputerSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IsPingable();
            }

        }

        private void ComputerSearch_Click(object sender, RoutedEventArgs e)
        {
            IsPingable();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            MachineIdTextBox.Text = "";
            UserLabel.Content = "N/A";
            PingableLable.Content = "N/A";
            PingableLable.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void CopyUser_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(UserLabel.Content.ToString().Replace(@"NGHS\", ""));
        }

        private void IsPingable()
        {
            // set display to show running
            UserLabel.Content = "N/A";
            PingableLable.Content = "RUNNING";
            PingableLable.Foreground = new SolidColorBrush(Colors.Yellow);

            // test ping 
            Ping ping = new Ping();
            ping.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);
            byte[] buffer = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            PingOptions options = new PingOptions(64, true);
            AutoResetEvent waiter = new AutoResetEvent(false);

            string machineName = MachineIdTextBox.Text;

            ping.SendAsync(machineName, 1000, buffer, options, waiter);


        }

        private void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            // If the operation was canceled, display a message to the user.
            if (e.Cancelled)
            {
                Console.WriteLine("Ping canceled.");

                // Let the main thread resume.
                // UserToken is the AutoResetEvent object that the main thread
                // is waiting for.
                ((AutoResetEvent)e.UserState).Set();
            }

            // If an error occurred, display the exception to the user.
            if (e.Error != null)
            {
                Console.WriteLine("Ping failed:");
                Console.WriteLine(e.Error.ToString());

                // Let the main thread resume.
                ((AutoResetEvent)e.UserState).Set();
            }

            PingReply reply = e.Reply;

            DisplayReply(reply);

            // Let the main thread resume.
            ((AutoResetEvent)e.UserState).Set();
        }


        private void DisplayReply(PingReply reply)
        {
            if (reply == null)
            {
                UserLabel.Content = $"N/A";
                PingableLable.Content = "FALSE";
                PingableLable.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            if (reply.Status == IPStatus.Success)
            {
                GetUser();
                PingableLable.Content = "TRUE";
                PingableLable.Foreground = new SolidColorBrush(Colors.Green);
            }
        }

        private void GetUser()
        {
            string machineName = MachineIdTextBox.Text;



            string location = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI";
            var registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;

            try
            {
                using (var hive = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, machineName, registryView))
                {
                    using (var key = hive.OpenSubKey(location))
                    {
                        var item = key.GetValue("LastLoggedOnUser");
                        string itemValue = item == null ? "No Logon Found" : item.ToString();
                        UserLabel.Content = itemValue;
                    }
                }
            }
            catch
            {
                UserLabel.Content = "No Logon Found";
            }


        }


    }


}
