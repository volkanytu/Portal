using GK.Library.Data.ElasticDataLayer.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GK.Library.Data.ElasticDataLayer
{
    public class ElasticAccess : IElasticAccess
    {
        private readonly string _elasticServerUrl;
        private ElasticClient _elasticClient;
        private string _indexName;

        private const string INDEX_NAME_TEMPLATE = "{0}_{1}";
        private const string DATETIME_PATTERN = "yyyyMM";

        public ElasticAccess(string elasticServerUrl, string indexName)
        {
            _elasticServerUrl = elasticServerUrl;
            _indexName = indexName;

            GetElasticClient();
        }

        public string IndexName
        {
            get
            {
                return GetDateTimeIndexName(DateTime.Now, _indexName);
            }
        }

        public string GetDateTimeIndexName(DateTime dateTime, string indexName)
        {
            string SUFFIX = dateTime.ToString(DATETIME_PATTERN);
            return string.Format(INDEX_NAME_TEMPLATE, indexName, SUFFIX);
        }

        private ElasticClient GetElasticClient()
        {
            var node = new Uri(_elasticServerUrl);

            var settings = new ConnectionSettings(
                node,
                defaultIndex: IndexName
            );

            return _elasticClient = new ElasticClient(settings);
        }

        public void DeleteIndex(string indexName)
        {
            var result = _elasticClient.DeleteIndex(indexName);
        }

        public void DeleteDocument(string indexName, string typeName, string id)
        {
            var req = new DeleteRequest(indexName, typeName, id);

            _elasticClient.Delete(req);
        }

        public void CreateDocument(string typeName, object document)
        {
            var result = _elasticClient.Index(document, p => p
                    .Index(IndexName)
                    .Type(typeName)
                    .Refresh()
                    .Id(Guid.NewGuid().ToString())
                    );

        }

        public List<T> GetSearchResultList<T>(ISearchRequest searchRequest) where T : class
        {
            var searchResults = _elasticClient.Search<T>(searchRequest);

            return (List<T>)searchResults.Documents;
        }
    }
}
