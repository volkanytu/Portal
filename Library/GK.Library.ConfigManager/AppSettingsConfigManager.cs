using GK.Library.ConfigManager.Interfaces;
using GK.Library.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.ConfigManager
{
    public class AppSettingsConfigManager : ConfigManager, IConfigManager
    {
        public AppSettingsConfigManager()
        {
            Task task = Task.Factory.StartNew(() => base.StartConfigCache());

            task.Wait();
        }

        public string GetKeyValue(string key)
        {
            return base.GetValue(key);
        }

        public string this[string key]
        {
            get { return base.GetValue(key); }
        }

        public override void FillConfigValues()
        {
            var appSettings = ConfigurationManager.AppSettings;

            ConfigCollection configCollection = new ConfigCollection();

            Dictionary<string, string> dictionary = appSettings.AllKeys.ToDictionary(key => key, key => appSettings[key]);

            configCollection.AddRange(dictionary);

            base.SetConfigValuesToCache(configCollection);
        }
    }
}
