﻿using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using LoggedInUsers.Helpers;
using LoggedInUsers.Properties;
using System.Windows.Controls;
using System.ComponentModel;

namespace LoggedInUsers
{

    public partial class MainWindow : Window
    {
        private int vncScreenSize;
        private string _vncOption;
        private string _rdpOption;

        private BackgroundWorker bgWorker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();

            // get vncScreenSize
            vncScreenSize = Settings.Default.VncScreenSize;
            _vncOption = Settings.Default.VncOption;

            // check the related fields
            LoadRdpSelectedOption();
            LoadVNCScreenSize();
            LoadVNCSelectedOption();


            // setup background worker to retrieve computer info
            bgWorker.DoWork += GetSignedInUSer_Work;
            bgWorker.DoWork += GetMachineUptime_Work;
        }



        private void ComputerSearch(bool doOpenConnection = false)
        {
            // clear recent results 
            ClearDisplay();

            // remove empty space and upercase the input
            string s = MachineIdTextBox.Text.Replace(" ", "").ToLower();

            // check if machine text box has something in it. 
            if (MachineIdTextBox.Text != null && MachineIdTextBox.Text != "")
            {
                // if s(computer)pings 
                if (NetworkHelper.IsPingable(s))
                {
                    // resolve the dns of the computer 
                    string machineName = NetworkHelper.GetDns(s);
                    DnsLabel.Content = machineName;

                    char[] machine = machineName.ToCharArray();

                    string shortName = $"{machine[6]}{machine[7]}";

                    // find if computer is thin client or full client 
                    if (shortName.Equals("mc") || shortName.Equals("tc"))
                    {
                        UserLabel.Content = "nghspass";

                        if (doOpenConnection)
                        {
                            // get chosen vnc option
                            string vncOption = Settings.Default.VncOption;

                            // connect to the computer
                            switch (vncOption)
                            {
                                case "Ultra":
                                    RDPHelper.VNCHelper(machineName, Settings.Default.VncScreenSize);
                                    break;
                                case "GoverlanVNC":
                                    RDPHelper.GoverlanVNCHelper(machineName);
                                    break;
                                case "BeyondVNC":
                                    RDPHelper.BeyondVNCHelper(machineName);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        bgWorker.RunWorkerAsync(); // find the user background task

                        if (doOpenConnection)
                        {
                            // start in chosen rdp session
                            string rpdOption = Settings.Default.RdpOption;

                            // automatically RDP to machine
                            switch (rpdOption)
                            {
                                case "GoverlanRDP":
                                    RDPHelper.GoverlanHelper(machineName);
                                    break;
                                case "BeyondRDP":
                                    RDPHelper.BeyondJumpHelper(machineName);
                                    break;
                            }
                        }

                    }

                }
            }
        }

        private void ComputerSearch_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                //SearchAndOpenComputer(true);
                ComputerSearch();
            }

        }

        #region MenuHeader


        private void RDPOption_Click(object sender, RoutedEventArgs e)
        {
            //reserve the item selected
            MenuItem m = sender as MenuItem;

            // clear all values
            GoverlanRDP.IsChecked = false;
            BeyondRDP.IsChecked = false;

            // check the option again
            m.IsChecked = true;

            // save settings
            _rdpOption = m.Name;
            Settings.Default.RdpOption = _rdpOption;
            Settings.Default.Save();
        }

        private void VNCOption_Click(object sender, RoutedEventArgs e)
        {
            MenuItem m = sender as MenuItem;

            // clear all values
            GoverlanVNC.IsChecked = false;
            Ultra.IsChecked = false;

            // check the option again 
            m.IsChecked = true;

            // save settings
            _vncOption = m.Name;
            Settings.Default.VncOption = _vncOption;
            Settings.Default.Save();
        }

        private void VNCScreenSize_Click(object sender, RoutedEventArgs e)
        {
            // get the checked option
            MenuItem m = sender as MenuItem;
            vncScreenSize = int.Parse(m.Header.ToString().Replace("%", ""));

            // clear all values 
            Vnc100.IsChecked = false;
            Vnc85.IsChecked = false;
            Vnc80.IsChecked = false;
            Vnc75.IsChecked = false;
            Vnc50.IsChecked = false;

            // check the saved one from settings
            m.IsChecked = true;

            // save value to settings for next use
            Settings.Default.VncScreenSize = vncScreenSize;
            Settings.Default.Save();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void LoadRdpSelectedOption()
        {
            switch (_rdpOption)
            {
                case "GoverlanRDP":
                    GoverlanRDP.IsChecked = true;
                    break;
                case "BeyondRDP":
                    BeyondRDP.IsChecked = true;
                    break;
                default:
                    GoverlanRDP.IsChecked = true;
                    break;

            }
        }

        private void LoadVNCSelectedOption()
        {
            switch (_vncOption)
            {
                case "GoverlanVNC":
                    GoverlanVNC.IsChecked = true;
                    break;
                case "Ultra":
                    Ultra.IsChecked = true;
                    break;
                case "BeyondVNC":
                    BeyondVNC.IsChecked = true;
                    break;
                default:
                    GoverlanVNC.IsChecked = true;
                    break;
            }
        }

        private void LoadVNCScreenSize()
        {
            if (vncScreenSize == 100)
            {
                Vnc100.IsChecked = true;
            }
            else if (vncScreenSize == 85)
            {
                Vnc85.IsChecked = true;
            }
            else if (vncScreenSize == 80)
            {
                Vnc80.IsChecked = true;
            }
            else if (vncScreenSize == 75)
            {
                Vnc75.IsChecked = true;
            }
            else if (vncScreenSize == 50)
            {
                Vnc50.IsChecked = true;
            }
            else
            {
                vncScreenSize = 85;
                Vnc85.IsChecked = true;
            }
        }
        #endregion

        #region Buttons
        private void ComputerSearch_Click(object sender, RoutedEventArgs e)
        {
            //SearchAndOpenComputer(false);
            ComputerSearch();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearDisplay();
        }

        private void CopyUser_Click(object sender, RoutedEventArgs e)
        {
            string results = UserLabel.Content.ToString();
            if (results != null)
            {
                Clipboard.SetDataObject(results.Replace(@"NGHS\", "").Replace(" ", ""));
            }
        }

        private void CopyDns_Click(object sender, RoutedEventArgs e)
        {
            string results = DnsLabel.Content.ToString();

            if (results != null)
            {
                Clipboard.SetDataObject(results);
            }
        }

        private void RestartComputer_Click(object sender, RoutedEventArgs e)
        {
            var machine = MachineIdTextBox.Text;
            if (machine != null && machine.Length > 0)
            {


                MessageBoxResult result = MessageBox.Show($"Are you sure you want to restart {machine}", "Confirmation", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Process p = new Process();
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.FileName = "cmd.exe";
                        startInfo.Arguments = $"/C shutdown /r /m {machine} /t 0";
                        p.StartInfo = startInfo;

                        p.Start();

                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }

        }

        private void ContinuePing_Click(object sender, RoutedEventArgs e)
        {
            var machine = MachineIdTextBox.Text;

            Process.Start("ping.exe", $"-t {machine}");


        }

        private void RDPOpen_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = (sender as Button).Name;
            string m = MachineIdTextBox.Text.Replace(" ", "");

            switch (buttonName)
            {
                case "Beyond":
                    string shortName = $"{m[6]}{m[7]}".ToLower();

                    if (shortName.Equals("mc") || shortName.Equals("tc"))
                    {
                        RDPHelper.BeyondVNCHelper(m); // vnc
                    }
                    else
                        RDPHelper.BeyondJumpHelper(m); // jump connect 

                    break;
                case "Goverlan":
                    RDPHelper.GoverlanHelper(m);
                    break;
                case "VNC":
                    if (_vncOption == "GoverlanVNC")
                    {
                        RDPHelper.GoverlanVNCHelper(m);
                    }
                    else if (_vncOption == "Ultra")
                    {
                        RDPHelper.VNCHelper(m, vncScreenSize);
                    }
                    else if (_vncOption == "BeyondVNC")
                    {
                        RDPHelper.BeyondVNCHelper(m);
                    }
                    break;
                case "RDP":
                    RDPHelper.RdpHelper(m);
                    break;
                case "BigFix":
                    RDPHelper.BigFixHelper();
                    break;

                default:
                    MessageBox.Show("How did you do this?");
                    break;
            }
        }

        #endregion


        #region Other Functions
        private void ClearDisplay()
        {
            UserLabel.Content = "";
            DnsLabel.Content = "";
            UpTimeLabel.Content = "";
        }


        #endregion Background stuff


        #region BackGroundWorker
        private void GetSignedInUSer_Work(object sender, DoWorkEventArgs e)
        {

            // set content to results 
            Dispatcher.Invoke(() =>
            {
                UserLabel.Content = NetworkHelper.GetUser(MachineIdTextBox.Text).Replace(@"NGHS\", "").Replace(" ", ""); ; // get user info from computer

            });

        }

        private void GetMachineUptime_Work(object sender, DoWorkEventArgs e)
        {

            Dispatcher.Invoke(() =>
            {
                UpTimeLabel.Content = NetworkHelper.GetUptime(MachineIdTextBox.Text); // get computer uptime
            });
        }




        #endregion


    }

}
