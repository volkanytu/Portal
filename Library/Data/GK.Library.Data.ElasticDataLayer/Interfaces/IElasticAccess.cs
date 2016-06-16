using Nest;
using System;
using System.Collections.Generic;
namespace GK.Library.Data.ElasticDataLayer.Interfaces
{
    public interface IElasticAccess
    {
        void CreateDocument(string typeName, object document);
        void DeleteDocument(string indexName, string typeName, string id);
        void DeleteIndex(string indexName);
        List<T> GetSearchResultList<T>(ISearchRequest searchRequest) where T : class;
        string IndexName { get; }
        string GetDateTimeIndexName(DateTime dateTime, string indexName);
    }
}
