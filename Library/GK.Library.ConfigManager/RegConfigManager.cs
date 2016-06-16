using GK.Library.ConfigManager.Interfaces;
using GK.Library.Entities.CustomEntities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.ConfigManager
{
    public class RegConfigManager : ConfigManager, IConfigManager
    {
        private string _registryName;
        private RegistryKey _baseKey = null;

        public RegConfigManager(string registeryName)
        {
            _registryName = registeryName;

            _baseKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\" + _registryName, RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.ReadKey);

            Task task = Task.Factory.StartNew(() => base.StartConfigCache());

            task.Wait();
        }

        ~RegConfigManager()
        {
            _baseKey.Close();
        }

        public string GetKeyValue(string key)
        {
            return base.GetValue(key);
        }

        public string this[string key]
        {
            get
            {
                return base.GetValue(key);
            }
        }

        public override void FillConfigValues()
        {
            string[] valueNames = _baseKey.GetValueNames();

            ConfigCollection configCollection = new ConfigCollection();

            foreach (string name in valueNames)
            {
                configCollection.Add(new KeyValuePair<string, string>(name, _baseKey.GetValue(name, string.Empty).ToString()));
            }

            base.SetConfigValuesToCache(configCollection);
        }
    }
}
