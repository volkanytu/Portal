using GK.Library.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.Data.Interfaces
{
    public interface ILog
    {
        void Log(LogEntity logEntity);
    }
}
