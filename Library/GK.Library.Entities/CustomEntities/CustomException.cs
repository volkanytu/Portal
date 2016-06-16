using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.Entities.CustomEntities
{
    public class CustomException : Exception
    {

        public CustomException(List<ValidationItem> validationItemList, string logKey)
            : base(Processor(validationItemList))
        {
            LogKey = logKey;
            //FunctionName = functionName;
        }

        public CustomException(ValidationItem validationItem)
            : base(Processor(validationItem))
        {
            LogKey = validationItem.LogKey;
            //FunctionName = functionName;
        }

        private static string Processor(List<ValidationItem> validationItemList)
        {
            return validationItemList.SerializeToJSON();
        }

        private static string Processor(ValidationItem validationItem)
        {
            List<ValidationItem> validationItemList = new List<ValidationItem>() { validationItem };

            return validationItemList.SerializeToJSON();
        }

        public string LogKey { get; private set; }
        //public string FunctionName { get; private set; }
    }
}
