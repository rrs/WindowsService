using System;
using System.Runtime.InteropServices;

namespace Rrs.WinowsService
{
    public static class ConsoleHelper
    {
        private const uint WM_CHAR = 0x0102;
        private const int VK_ENTER = 0x0D;
        private const int ATTACH_PARENT_PROCESS = -1;

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        public static void Run(string serviceName, string[] args, string serviceDisplayName = null, string serviceDescription = null)
        {
            bool install = false, uninstall = false;

            // redirect console output to parent process;
            // must be before any calls to Console.WriteLine()
            AttachConsole(ATTACH_PARENT_PROCESS);
            IntPtr cw = GetConsoleWindow();
            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "-i":
                    case "-install":
                        install = true; break;
                    case "-u":
                    case "-uninstall":
                        uninstall = true; break;
                    default:
                        Console.Error.WriteLine("Argument not expected: " + arg);
                        break;
                }
            }

            if (uninstall)
            {
                ServiceAppInstaller.Install(serviceName, true, args, serviceDisplayName, serviceDescription);
            }
            if (install)
            {
                ServiceAppInstaller.Install(serviceName, false, args, serviceDisplayName, serviceDescription);
            }

            // Send the Enter key to the console window no matter where it is.
            SendMessage(cw, WM_CHAR, (IntPtr)VK_ENTER, IntPtr.Zero);

            FreeConsole();
        }
    }

}
