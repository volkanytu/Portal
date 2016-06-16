using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.Entities.CustomEntities
{
    public class ValidationItem
    {
        public ValidationItem(string logKey)
        {
            LogKey = logKey;
        }

        public string LogKey { get; private set; }
        public string ErrorMessage { get; set; }      
        public string RecordId { get; set; }
    }
}
