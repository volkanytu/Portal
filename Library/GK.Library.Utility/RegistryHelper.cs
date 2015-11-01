using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;


namespace GK.Library.Utility
{
    public class RegistryHelper
    {
        private RegistryKey baseKey = null;

        private RegistryHelper()
        {
            //baseKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\DOTest", RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.ReadKey);
            baseKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Kale", RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.ReadKey);
        }

        public string Value(string keyName)
        {
            //TODO : Check SubKey Kind
            //var cASubKeyKind = baseKey.GetValueKind(keyName); 

            return baseKey.GetValue(keyName, string.Empty).ToString();
        }

        ~RegistryHelper()
        {
            baseKey.Close();
        }

        private static RegistryHelper registryHelperInstance = null;
        private static readonly object lockthread = new object();

        public static RegistryHelper Get
        {
            get
            {
                lock (lockthread)
                {
                    if (registryHelperInstance == null)
                    {
                        registryHelperInstance = new RegistryHelper();
                    }
                    return registryHelperInstance;
                }
            }
        }
    }
}
