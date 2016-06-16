using GK.Library.Data.Interfaces;
using GK.Library.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.LogManager.Interfaces
{
    public interface ILogFactory
    {
        ILog GetLogData(LogEntity.LogClientType logClientType);
    }
}
