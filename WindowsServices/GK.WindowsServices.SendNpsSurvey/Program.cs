using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

namespace GK.WindowsServices.SendNpsSurvey
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun;

            SendNpsSurvey sendSms = new SendNpsSurvey();
            ServicesToRun = new ServiceBase[] 
            { 
                sendSms
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

                        sendSms.DebugService(args);

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
