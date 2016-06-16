using GK.Library.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.Data.ElasticDataLayer.Interfaces
{
    public interface ILogDao
    {
        List<LogEntity> GetIntegrationExceptionLogList(string indexName, int from, int size, DateTime startDate, DateTime endDate);

        List<LogEntity> GetVerifiedDataExceptionLogList(string indexName, int from, int size, DateTime startDate, DateTime endDate);
    }
}
