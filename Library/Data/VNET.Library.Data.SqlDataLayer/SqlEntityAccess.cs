using GK.Library.Entities.CustomEntities;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using GK.Library.Data.SqlDataLayer.Interfaces;

namespace GK.Library.Data.SqlDataLayer
{
    public class SqlEntityAccess : ISqlEntityAccess
    {
        private ISqlAccess _sqlAccess;
        public ISqlAccess SqlAccess
        {
            get
            {
                return _sqlAccess;
            }
        }

        public SqlEntityAccess(ISqlAccess sqlAccess)
        {
            _sqlAccess = sqlAccess;
        }

        public object Create(Entity entity, string PKName)
        {
            SqlQuery query = GetInsertQuery(entity, PKName);

            return _sqlAccess.ExecuteScalar(query.Query, query.parameters.ToArray());
        }

        public void Update(Entity entity, string PKName)
        {
            SqlQuery query = GetUpdateQuery(entity, PKName);

            _sqlAccess.ExecuteNonQuery(query.Query, query.parameters.ToArray());
        }

        public void Delete(string entityName, Guid id, string PKName)
        {
            SqlQuery query = GetDeleteQuery(entityName, id, PKName);

            _sqlAccess.ExecuteNonQuery(query.Query, query.parameters.ToArray());
        }

//        private string GetPKAttributeName(string entityName)
//        {
//            string sqlQuery = @"SELECT
//                                    attr.PhysicalName AS PKName
//                                FROM
//                                Entity AS e (NOLOCK)
//	                                JOIN
//		                                Attribute AS attr (NOLOCK)
//			                                ON
//			                                e.EntityId=attr.EntityId
//                                WHERE
//                                e.Name='{0}'
//                                AND
//                                attr.IsPKAttribute=1";

//            return (string)_sqlAccess.ExecuteScalar(string.Format(sqlQuery, entityName));
//        }

        private SqlQuery GetInsertQuery(Entity entity, string PKName)
        {
            string columnNames = string.Join(",", entity.Attributes.Keys);

            string sqlParameters = "@" + string.Join(",@", entity.Attributes.Keys);

            List<SqlParameter> lstParameters = new List<SqlParameter>();

            foreach (KeyValuePair<string, object> item in entity.Attributes)
            {
                lstParameters.Add(FillSqlParameter(item));
            }

            string sqlQuery = string.Format("INSERT INTO {0} ({1}) OUTPUT INSERTED.{3} VALUES ({2})", entity.LogicalName, columnNames, sqlParameters, PKName);

            return new SqlQuery()
            {
                Query = sqlQuery,
                parameters = lstParameters
            };
        }

        private SqlQuery GetUpdateQuery(Entity entity, string PKName)
        {
            List<string> lstSetQuery = new List<string>();
            List<SqlParameter> lstParameters = new List<SqlParameter>();

            //lstParameters.Add(new SqlParameter("@Id", entity.Id));

            foreach (KeyValuePair<string, object> item in entity.Attributes)
            {
                lstSetQuery.Add(string.Format("{0}=@{0}", item.Key));

                lstParameters.Add(FillSqlParameter(item));
            }

            string setQuery = string.Join(",", lstSetQuery);

            string sqlQuery = string.Format("UPDATE {0} SET {1} WHERE {2}=@Id", entity.LogicalName, setQuery, PKName);

            return new SqlQuery()
            {
                Query = sqlQuery,
                parameters = lstParameters
            };
        }

        private SqlQuery GetDeleteQuery(string entityName, Guid id, string PKName)
        {
            List<SqlParameter> lstParameters = new List<SqlParameter>();

            lstParameters.Add(new SqlParameter("@id", id));

            string sqlQuery = string.Format("DELETE FROM {0} WHERE {1}=@id", entityName, PKName);

            return new SqlQuery()
            {
                Query = sqlQuery,
                parameters = lstParameters
            };
        }

        private SqlParameter FillSqlParameter(KeyValuePair<string, object> item)
        {
            if (item.Value is EntityReference)
            {
                return new SqlParameter("@" + item.Key, ((EntityReference)item.Value).Id);
            }
            else if (item.Value is OptionSetValue)
            {
                return new SqlParameter("@" + item.Key, ((OptionSetValue)item.Value).Value);
            }
            else if (item.Value is Money)
            {
                return new SqlParameter("@" + item.Key, ((Money)item.Value).Value);
            }
            else
            {
                return new SqlParameter("@" + item.Key, item.Value);
            }
        }
    }
}
