using System;
using System.Windows;
using System.Windows.Shell;
using pw;

namespace pwshutdown
{
    public class Program : Application
    {
        [STAThread]
        static void Main()
        {
            // Using a System.Windows.Application derived class to enable easier jumplist access
            Program program = new Program();
            program.Run();
        }

        //

        private const string strShutdownArgument = "shutdown"; // Command line argument to initiate a system shutdown
        private const string strRebootArgument = "reboot"; // Command line argument to initiate a system reboot
        private const string strLogoffArgument = "logoff"; // Command line argument to log the current user of the system

        //

        // Called after the Run method of the Application object has been called
        protected override void OnStartup(StartupEventArgs _startupEventArgs)
        {
            base.OnStartup(_startupEventArgs);

            //

            // Create the jump list if it has not already been created on a previous startup
            CreateJumpList();

            // Go through all the command line arguments and start appropriate behaviour
            bool bArgumentFound = false;
            foreach (string sArgument in _startupEventArgs.Args)
            {
                if (sArgument == strShutdownArgument)
                {
                    ShutdownSystem.Shutdown(true);
                    bArgumentFound = true;
                    break;
                }
                else if (sArgument == strRebootArgument)
                {
                    ShutdownSystem.Reboot();
                    bArgumentFound = true;
                    break;
                }
                else if (sArgument == strLogoffArgument)
                {
                    ShutdownSystem.LogOff();
                    bArgumentFound = true;
                    break;
                }
            }

            // If no behaviour bound command line argument was found, open up the shutdown dialog window
            if (!bArgumentFound)
            {
                ShutdownSystem.OpenShutdownWindow();
            }

            // Close the Application
            this.Shutdown();
        }

        public void CreateJumpList()
        {
            // Only create a jump list if none has been created on a previous startup
            if (JumpList.GetJumpList(Application.Current) != null)
                return;

            // Read the jumplist item titles from the localized system file "shutdown.exe.mui"
            string strShutdown;
            string strReboot;
            string strLogoff;
            try
            {
                DynamicLibrary libLocalizedShutdown = new DynamicLibrary(Environment.SystemDirectory + "\\" + System.Globalization.CultureInfo.CurrentCulture.Name + "\\" + "shutdown.exe.mui");
                strShutdown = libLocalizedShutdown.GetString(2);
                strReboot = libLocalizedShutdown.GetString(3);
                strLogoff = libLocalizedShutdown.GetString(5);
            }
            catch
            {
                // If anything goes wrong we use english default values
                strShutdown = "Shutdown";
                strReboot = "Restart";
                strLogoff = "Log off";
            }

            //

            // Create a new jumplist and bind it to the current application
            JumpList jumpList = new JumpList();
            JumpList.SetJumpList(Application.Current, jumpList);

            // Get the current location of the executable
            string strCurrentExecutablePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            // Create the shutdown jumplist item
            JumpTask jumpTaskShutdown = new JumpTask();
            jumpTaskShutdown.Title = strShutdown;
            jumpTaskShutdown.ApplicationPath = strCurrentExecutablePath;
            jumpTaskShutdown.Arguments = strShutdownArgument;
            jumpTaskShutdown.IconResourcePath = strCurrentExecutablePath;
            jumpTaskShutdown.IconResourceIndex = 3;

            // Create the reboot jumplist item
            JumpTask jumpTaskReboot = new JumpTask();
            jumpTaskReboot.Title = strReboot;
            jumpTaskReboot.ApplicationPath = strCurrentExecutablePath;
            jumpTaskReboot.Arguments = strRebootArgument;
            jumpTaskReboot.IconResourcePath = strCurrentExecutablePath;
            jumpTaskReboot.IconResourceIndex = 2;

            // Create the logoff jumplist item
            JumpTask jumpTaskLogoff = new JumpTask();
            jumpTaskLogoff.Title = strLogoff;
            jumpTaskLogoff.ApplicationPath = strCurrentExecutablePath;
            jumpTaskLogoff.Arguments = strLogoffArgument;
            jumpTaskLogoff.IconResourcePath = strCurrentExecutablePath;
            jumpTaskLogoff.IconResourceIndex = 1;

            // Add the newly created jumpitems to the jumplist
            jumpList.JumpItems.Add(jumpTaskShutdown);
            jumpList.JumpItems.Add(jumpTaskReboot);
            jumpList.JumpItems.Add(jumpTaskLogoff);
            
            // Push the changed jumplist to the shell
            jumpList.Apply();            
        }
    }
}
