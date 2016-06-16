using GK.Library.Data.ElasticDataLayer.Interfaces;
using GK.Library.Data.Interfaces;
using GK.Library.Entities.CustomEntities;
using Nest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GK.Library.Data.ElasticDataLayer
{
    public class ElasticLogDao : ILog, ILogDao
    {
        private IElasticAccess _elasticAccess;
        private string _applicationName;

        public ElasticLogDao(IElasticAccess elasticAccess, string applicationName)
        {
            _elasticAccess = elasticAccess;
            _applicationName = applicationName;
        }

        public void Log(LogEntity logEntity)
        {
            logEntity.ApplicationName = _applicationName;

            _elasticAccess.CreateDocument(logEntity.FunctionName, logEntity);
        }

        public List<LogEntity> GetIntegrationExceptionLogList(string indexName, int from, int size, DateTime startDate, DateTime endDate)
        {
            string getDateTimeIndexName = _elasticAccess.GetDateTimeIndexName(endDate, indexName);

            SearchDescriptor<LogEntity> searchDescriptor = new SearchDescriptor<LogEntity>()
                .Index(getDateTimeIndexName)
                .AllTypes()
                .From(from)
                .Size(size)
                .Query(q => q.Term(p => p.ApplicationName, _applicationName.ToLower(new CultureInfo("en-US", false))) && q.Term(p => p.TryCount, 25) && q.Term(p => p.LogEventType, "100000002"))
                .Filter(ff => ff.Range(n => n
                          .OnField(f => f.CreatedOn)
                          .Greater(startDate.ToUniversalTime())
                          .Lower(endDate.ToUniversalTime())
                          ))
                .SortDescending("createdOn");

            List<LogEntity> logEntityList = _elasticAccess.GetSearchResultList<LogEntity>((ISearchRequest)searchDescriptor);

            return logEntityList;
        }

        public List<LogEntity> GetVerifiedDataExceptionLogList(string indexName, int from, int size, DateTime startDate, DateTime endDate)
        {
            string getDateTimeIndexName = _elasticAccess.GetDateTimeIndexName(endDate, indexName);

            SearchDescriptor<LogEntity> searchDescriptor = new SearchDescriptor<LogEntity>()
                .Index(getDateTimeIndexName)
                .AllTypes()
                .From(from)
                .Size(size)
                .Query(q => q.Term(p => p.ApplicationName, _applicationName.ToLower(new CultureInfo("en-US", false))) && q.Term(p => p.LogEventType, "100000002"))
                .Filter(ff => ff.Range(n => n
                          .OnField(f => f.CreatedOn)
                          .Greater(startDate.ToUniversalTime())
                          .Lower(endDate.ToUniversalTime())
                          ))
                .SortDescending("createdOn");

            List<LogEntity> logEntityList = _elasticAccess.GetSearchResultList<LogEntity>((ISearchRequest)searchDescriptor);

            return logEntityList;
        }

    }
}
