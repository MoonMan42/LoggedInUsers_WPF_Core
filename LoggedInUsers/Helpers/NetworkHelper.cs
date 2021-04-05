using Microsoft.Win32;
using System;
using System.Net.NetworkInformation;
using System.Management;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace LoggedInUsers.Helpers
{
    public class NetworkHelper
    {

        /// <summary>
        /// Pings computer 
        /// </summary>
        /// <param name="computer"></param>
        /// <returns></returns>
        public static async Task<bool> IsPingable(string computer)
        {
            Ping ping = new Ping();

            try
            {
                PingReply reply = ping.Send(computer, 1000);

                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;

        }

        /// <summary>
        /// Get DNS name of server
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public static string GetDns(string machineName)
        {
            try
            {
                IPHostEntry host = Dns.GetHostEntry(machineName);
                return host.HostName.Replace(".nghs.com", "");
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// find user logged in, thick clients only
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public static string GetUser(string machineName)
        {

            string location = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI";
            var registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;

            try
            {
                using (var hive = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, machineName, registryView))
                {
                    using (var key = hive.OpenSubKey(location))
                    {
                        var item = key.GetValue("LastLoggedOnUser");
                        string itemValue = item.ToString();
                        return itemValue;
                    }
                }
            }
            catch
            {
                return "?";
            }

        }

        /// <summary>
        /// Find the up time in the machine, thick client only
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public static string GetUptime(string machineName)
        {

            try
            {
                var scope = new ManagementScope(string.Format(@$"\\{machineName}\root\cimv2"));


                var query = new ObjectQuery("SELECT LastBootUpTime FROM Win32_OperatingSystem");

                scope.Connect();

                var searcher = new ManagementObjectSearcher(scope, query);

                var firstResult = searcher.Get().OfType<ManagementObject>().First();

                return ManagementDateTimeConverter.ToDateTime(firstResult["LastBootUpTime"].ToString()).ToString("d");
            }
            catch
            {
                return "?";
            }
        }

        /// <summary>
        /// Get Process List from another computer. 
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        internal static List<Process> GetProcesses(string machineName)
        {
            List<Process> result = Process.GetProcesses(machineName).ToList();


            return result.OrderBy(r => r.ProcessName).ToList();
        }

        /// <summary>
        ///  Kills a process on remote computer by id.
        /// </summary>
        /// <param name="machineName"></param>
        /// <param name="processId"></param>
        internal static void StopProcess(string machineName, int processId)
        {
            Process process = Process.GetProcessById(processId, machineName);
            process.Close();
        }
    }

}
