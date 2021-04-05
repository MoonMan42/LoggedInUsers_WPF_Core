using System.Diagnostics;

namespace LoggedInUsers.Helpers
{
    public static class RDPHelper
    {

        public static void GoverlanHelper(string machine)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.CreateNoWindow = true;


            info.Arguments = $"/c \"C:\\Program Files\\Goverlan Reach Console 9\\goverRMC.exe\" {machine} -rctype:1 -noprompt"; // connect with Goverlan (-rctype:1)

            Process.Start(info);
        }

        public static void GoverlanVNCHelper(string machine)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.CreateNoWindow = true;
            info.Arguments = $"/c \"C:\\Program Files\\Goverlan Reach Console 9\\goverRMC.exe\" {machine}  -rctype:3 "; //-noprompt -password nghspass| connect with VNC (-rctype:3)
            Process.Start(info);

        }

        public static void VNCHelper(string machine, int size)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.CreateNoWindow = true;

            info.Arguments = $"/C start C:\\ultravnc.exe \"{machine}\" -password nghspass -scale {size}/100";


            Process.Start(info);
        }

        public static void RdpHelper(string machine)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.CreateNoWindow = true;

            if (machine != null || machine == string.Empty)
            {
                info.Arguments = $"/c mstsc /console /V:{machine}";
            }
            else
            {
                info.Arguments = $"/c mstsc /console";
            }

            Process.Start(info);
        }

        public static void BigFixHelper()
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";

            info.CreateNoWindow = true;

            info.Arguments = "/C \"C:\\Program Files (x86)\\IBM\\Tivoli\\Remote Control\\Controller\\TRCConsole.jar\"";

            Process.Start(info);
        }

        public static void SccmHelper(string machine)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.CreateNoWindow = true;

            info.Arguments = $@"/C \\hqsbisilon2\Global\AS\SCCM\RemoteControlViewer\CmRcViewer.exe {machine}";


            Process.Start(info);
        }
    }

}
