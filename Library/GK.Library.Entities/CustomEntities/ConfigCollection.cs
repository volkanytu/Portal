using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.Entities.CustomEntities
{
    public class ConfigCollection : DataCollection<string, string>
    {
        public ConfigCollection()
        {

        }

        public ConfigCollection(List<KeyValuePair<string, string>> configData)
        {
            base.AddRange(configData);
        }
    }
}
