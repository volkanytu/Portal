using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

namespace GK.WindowsServices.ProcessDiscoveryForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new ProcessDiscoveryForms() 
            };

            if (System.Environment.UserInteractive)
            {

                string parameter = string.Concat(args);

                switch (parameter)
                {

                    case "--install":

                        ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });

                        break;

                    case "--uninstall":

                        ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });

                        break;

                    case "":

                        ProcessDiscoveryForms service = new ProcessDiscoveryForms();
                        service.DebugService(args);

                        break;
                }

            }
            else
            {
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
