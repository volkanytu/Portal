using GK.Library.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GK.Library.ConfigManager
{
    public abstract class ConfigManager
    {
        private const int CACHE_INTERVAL = 30 * 1000;
        private System.Timers.Timer _timer;

        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        private static ConfigCollection _configCollection = new ConfigCollection();

        public ConfigManager()
        {
            _timer = new System.Timers.Timer();
            _timer.Interval = CACHE_INTERVAL;
            _timer.AutoReset = true;
        }

        public void StartConfigCache()
        {
            FillConfigValues();

            _timer.Elapsed += _timer_Elapsed;
            _timer.Enabled = true;
            _timer.Start();
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            FillConfigValues();
        }

        public void SetConfigValuesToCache(ConfigCollection configCollection)
        {
            cacheLock.EnterWriteLock();
            try
            {
                _configCollection = configCollection;
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        public string GetValue(string key)
        {
            cacheLock.EnterReadLock();

            try
            {
                return _configCollection[key];
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

        public abstract void FillConfigValues();
    }
}
