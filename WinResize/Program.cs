using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinResize
{
    class Program
    {
        static int SW_MAXIMIZE = 3;

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int X);

        [DllImport("user32.dll")]
        public static extern bool SetFocus(IntPtr hWnd);



        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        static void Main(string[] args)
        {
            Console.Title = "WinResize";
            //Read parameters from command line
            if (args.Count() == 0)
            {
                Console.WriteLine("Usage: process name, window name, window number, sleep time (in seconds)");
                return;
            }

            string processName = args[0];
            string windowName = args[1];
            int windowNumber = int.Parse(args[2]);
            int sleepTime = int.Parse(args[3]);
            //Launch process
            for (int i = 0; i < windowNumber; i++)
            {
                Process p = new Process();
                p.StartInfo.FileName = processName;
                p.Start();
                Console.WriteLine("Launching " + processName + " " + (i + 1) + "/" + windowNumber);
                System.Threading.Thread.Sleep(sleepTime * 1000);
            }

            Console.WriteLine("Fixing windows");
            //Process parameters
            int windowWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int windowHeight = Screen.PrimaryScreen.WorkingArea.Height;
            int j = 0;
            foreach (Process proc in Process.GetProcesses())
            {
                if (proc.MainWindowTitle.Contains(windowName))
                {
                    int startPosition = (windowWidth / windowNumber) * j;
                    int width = (windowWidth / windowNumber);

                    IntPtr handle = proc.MainWindowHandle;
                    ShowWindow(handle, SW_MAXIMIZE);
                    MoveWindow(handle, startPosition, 0, width, windowHeight, true);
                    SetFocus(handle);
                    j++;
                }
            }

            Console.WriteLine("Windows fixed, have fun!");
            Console.ReadKey();
        }
    }
}
