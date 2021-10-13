using System.Diagnostics;

namespace LoggedInUsers.Helpers
{
    public static class RDPHelper
    {
        /// <summary>
        /// Opens Goverlan
        /// </summary>
        /// <param name="machine"></param>
        public static void GoverlanHelper(string machine)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.CreateNoWindow = true;


            info.Arguments = $"/c \"C:\\Program Files\\Goverlan Reach Console 9\\goverRMC.exe\" {machine} -rctype:1 -noprompt"; // connect with Goverlan (-rctype:1)

            Process.Start(info);
        }

        /// <summary>
        /// Opens GoverlanVNC (Need to enter password mannually)
        /// </summary>
        /// <param name="machine"></param>
        public static void GoverlanVNCHelper(string machine)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.CreateNoWindow = true;
            info.Arguments = $"/c \"C:\\Program Files\\Goverlan Reach Console 9\\goverRMC.exe\" {machine}  -rctype:3 "; //-noprompt -password nghspass| connect with VNC (-rctype:3)
            Process.Start(info);

        }

        /// <summary>
        /// Opens VNC Ultra
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="size"></param>
        public static void VNCHelper(string machine, int size)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.CreateNoWindow = true;

            info.Arguments = $"/C start C:\\ultravnc.exe \"{machine}\" -password nghspass -scale {size}/100";


            Process.Start(info);
        }

        /// <summary>
        /// Opens MSTSC
        /// </summary>
        /// <param name="machine"></param>
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

        /// <summary>
        /// Opens BigFix, does not put the computer name in automatically
        /// </summary>
        public static void BigFixHelper()
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";

            info.CreateNoWindow = true;

            info.Arguments = "/C \"C:\\Program Files (x86)\\IBM\\Tivoli\\Remote Control\\Controller\\TRCConsole.jar\"";

            Process.Start(info);
        }

        /// <summary>
        /// Opens SCCM (Not really used much)
        /// </summary>
        /// <param name="machine"></param>
        public static void SccmHelper(string machine)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.CreateNoWindow = true;

            info.Arguments = $@"/C \\hqsbisilon2\Global\AS\SCCM\RemoteControlViewer\CmRcViewer.exe {machine}";


            Process.Start(info);
        }

        /// <summary>
        /// opens Beyond Jump
        /// </summary>
        /// <param name="machine"></param>
        public static void BeyondJumpHelper(string machine)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.CreateNoWindow = true;

            // full clients 
            info.Arguments = $"/C \"\"C:\\Program Files\\bomgar\\Representative Console\\nghs.beyondtrustcloud.com\\bomgar-rep.exe\" --run-script \"action=push_and_start_remote&jumpoint=VMASBTRUST&target={machine}\"\"";

            Process.Start(info);
        }


        /// <summary>
        /// opens Beyond VNC
        /// </summary>
        /// <param name="machine"></param>
        public static void BeyondVNCHelper(string machine)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.CreateNoWindow = true;

            // thin clients
            info.Arguments = $"/C \"\"C:\\Program Files\\bomgar\\Representative Console\\nghs.beyondtrustcloud.com\\bomgar-rep.exe\" --run-script \"action=start_vnc_session&target={machine}&jumpoint=VMASBTRUST\"\"";

            Process.Start(info);
        }
    }

}
