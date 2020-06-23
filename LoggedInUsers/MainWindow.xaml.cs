using Microsoft.Win32;
using System;
using System.Net.NetworkInformation;
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
                GetUser();
                IsPingable();
            }

        }

        private void ComputerSearch_Click(object sender, RoutedEventArgs e)
        {
            GetUser();
            IsPingable();
        }

        private void CopyUser_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(UserLabel.Content.ToString().Replace(@"NGHS\", ""));
        }


        private async void GetUser()
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
                        //return itemValue;
                        UserLabel.Content = itemValue;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }

        }

        private async void IsPingable()
        {
            Ping ping = new Ping();
            string machineName = MachineIdTextBox.Text;

            try
            {
                PingReply pingReply = ping.Send(machineName, 1000);
                if (pingReply.Status == IPStatus.Success)
                {
                    //return true; // is pingable
                    PingableLable.Content = "TRUE";
                    PingableLable.Foreground = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    PingableLable.Content = "FALSE";
                    PingableLable.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
            catch (PingException ex)
            {

            }
        }


    }
}
