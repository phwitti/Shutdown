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

        // Command line arguments to start appropriate behaviour
        private const string c_sShutdownArgument = "shutdown";
        private const string c_sRebootArgument = "reboot";
        private const string c_sLogoffArgument = "logoff";

        // Use english default values, if localisation fails
        private const string c_sShutdownDefaultTitle = "Shutdown";
        private const string c_sRebootDefaultTitle = "Restart";
        private const string c_sLogoffDefaultTitle = "Log off";

        //

        // Called after the Run method of the Application object has been called
        protected override void OnStartup(StartupEventArgs _startupEventArgs)
        {
            base.OnStartup(_startupEventArgs);

            //
            
            // Create a dictionary that associates the command line arguments with their appropriate behaviour
            ActionDictionary<string> actionDictionary = new ActionDictionary<string>();
            actionDictionary.Add(c_sShutdownArgument, () => ShutdownSystem.Shutdown(true));
            actionDictionary.Add(c_sRebootArgument, () => ShutdownSystem.Reboot());
            actionDictionary.Add(c_sLogoffArgument, () => ShutdownSystem.LogOff());

            // Try to find and start behaviour associated with the given command line arguments
            if (!actionDictionary.ExecuteFirst(_startupEventArgs.Args))
            {
                // If no behaviour bound command line argument was found, open up the shutdown dialog window
                ShutdownSystem.OpenShutdownWindow();

                // There's no possibility to check, if the jumplist has already been created on a previous startup
                // -> Create new one
                CreateJumpList();
            }

            // Close the Application
            this.Shutdown();
        }

        public void CreateJumpList()
        {
            // Create a new jumplist and bind it to the current application
            JumpList jumpList = new JumpList();
            JumpList.SetJumpList(Application.Current, jumpList);

            // Get the current location of the executable
            string sCurrentExecutablePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            // Try to load the localized system file that includes the shutdown option strings
            DynamicLibrary libLocalizedShutdown = new DynamicLibrary();
            try
            {
                libLocalizedShutdown.Load(System.IO.Path.Combine(Environment.SystemDirectory, System.Globalization.CultureInfo.CurrentCulture.Name, "shutdown.exe.mui"));
            }
            catch
            {
                // Nothing to do here: If the module could not be loaded, the GetString-method will return the fallback string.
            }

            // Read the jumplist item titles from the localized system file "shutdown.exe.mui"
            string sShutdown = libLocalizedShutdown.GetString(2, c_sShutdownDefaultTitle);
            string sReboot = libLocalizedShutdown.GetString(3, c_sRebootDefaultTitle);
            string sLogoff = libLocalizedShutdown.GetString(5, c_sLogoffDefaultTitle);

            // Create and add jumpitems to the jumplist
            jumpList.JumpItems.Add(new JumpTask().Initialized(sShutdown, sCurrentExecutablePath, c_sShutdownArgument, sCurrentExecutablePath, 3));
            jumpList.JumpItems.Add(new JumpTask().Initialized(sReboot, sCurrentExecutablePath, c_sRebootArgument, sCurrentExecutablePath, 2));
            jumpList.JumpItems.Add(new JumpTask().Initialized(sLogoff, sCurrentExecutablePath, c_sLogoffArgument, sCurrentExecutablePath, 1));
            
            // Push the changed jumplist to the shell
            jumpList.Apply();            
        }
    }
}
