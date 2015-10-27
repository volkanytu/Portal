using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using System.Data.SqlClient;
using System.Web.Services.Protocols;

namespace GK.Library.Utility
{
    public class EventLogHelper
    {
        public static void CreateLogBySql(string applicationName, string functionName, string detail, string createdBy, string businessUnit, EventType eventType)
        {
            try
            {
                SqlDataAccess sda = new SqlDataAccess();
                sda.openConnection(Globals.ConnectionString);

                string sqlQuery = @"DECLARE @EventLogID uniqueidentifier
                                    SET @EventLogID=NEWID()

                                    INSERT
                                    INTO
	                                    new_eventlogBase
	                                    (new_eventlogId
	                                    ,CreatedOn
	                                    ,CreatedBy
	                                    ,ModifiedOn
	                                    ,ModifiedBy
	                                    ,OwnerId
	                                    ,OwningBusinessUnit
	                                    ,statecode
	                                    ,statuscode
                                    )
                                    VALUES
                                    (
	                                    @EventLogID
	                                    ,@CreatedOn
	                                    ,@CreatedBy
	                                    ,@CreatedOn
	                                    ,@CreatedBy
	                                    ,@CreatedBy
	                                    ,@BusinessUnit
	                                    ,0
	                                    ,1
                                    )

                                    INSERT
                                    INTO
	                                    new_eventlogExtensionBase
                                    (
	                                    new_eventlogId
	                                    ,new_name
	                                    ,new_applicationname
	                                    ,new_detail
	                                    ,new_eventtype
	                                    ,new_function
                                    )
                                    VALUES
                                    (
	                                    @EventLogID
	                                    ,@Title
	                                    ,@ApplicationName
	                                    ,@Detail
	                                    ,@EventType
	                                    ,@FunctionName
                                    )";

                string title = string.Format("{0} - {1} - {2}", applicationName, functionName, DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                List<SqlParameter> li = new List<SqlParameter>();
                li.Add(new SqlParameter("@CreatedOn", DateTime.UtcNow));
                li.Add(new SqlParameter("@CreatedBy", new Guid(createdBy)));
                li.Add(new SqlParameter("@CreatedBy", new Guid(businessUnit)));
                li.Add(new SqlParameter("@Title", title));
                li.Add(new SqlParameter("@ApplicationName", applicationName));
                li.Add(new SqlParameter("@Detail", detail));
                li.Add(new SqlParameter("@EventType", (int)eventType));
                li.Add(new SqlParameter("@FunctionName", functionName));

                sda.ExecuteNonQuery(sqlQuery, li.ToArray());

                sda.closeConnection();
            }
            catch (Exception ex)
            {

            }
        }

        public EventLogHelper(IOrganizationService orgService)
        {
            this.OrgService = orgService;
        }
        public EventLogHelper(IOrganizationService orgService,string applicationName)
        {
            this.OrgService = orgService;
            this.ApplicationName = applicationName;
        }

        public enum EventType
        {
            Info = 100000000,
            Warning,
            Exception
        }
        public string ApplicationName { get; set; }

        public IOrganizationService OrgService { get; set; }

        public static void GetExceptionString(Exception ex, out string detailedText)
        {
            detailedText = null;
            if (ex != null)
            {
                if (ex is SoapException)
                {
                    SoapException soap = ex as SoapException;
                    detailedText = string.Format("Message:\n {0} \n\n StackTrace:\n {1} \n\n Detail:\n {2}", soap.Message, soap.ToString(), soap.Detail.InnerText);
                }
                else if (ex is SqlException)
                {
                    SqlException sql = ex as SqlException;
                    detailedText = string.Format("Message:\n {0} \n\n StackTrace:\n {1} \n\n SqlErrors:\n {2}", sql.Message, sql.ToString(), sql.Errors.Count);
                }
                else
                {
                    detailedText = string.Format("Message:\n {0} \n\n StackTrace:\n {1}", ex.Message, ex.ToString());
                }
            }
        }

        public void Log(string functionName, string detail, EventType eventType)
        {
            try
            {
                Entity e = new Entity("new_eventlog");
                e["new_name"] = string.Format("{0} - {1} - {2}", this.ApplicationName, functionName, DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                e["new_applicationname"] = this.ApplicationName;
                e["new_function"] = functionName;
                e["new_detail"] = detail;
                e["new_eventtype"] = new OptionSetValue((int)eventType);
                this.OrgService.Create(e);
            }
            catch { }
        }
        public void Log(string functionName, string detail, EventType eventType, string relatedObjectType, string relatedObjectId)
        {
            try
            {
                Entity e = new Entity("new_eventlog");
                e["new_name"] = string.Format("{0} - {1} - {2}", this.ApplicationName, functionName, DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                e["new_applicationname"] = this.ApplicationName;
                e["new_function"] = functionName;
                e["new_detail"] = detail;
                e["new_eventtype"] = new OptionSetValue((int)eventType);
                e["new_objecttype"] = relatedObjectType;
                e["new_objectid"] = relatedObjectId;

                this.OrgService.Create(e);
            }
            catch { }
        }
        public void Log(string functionName, Exception ex, EventType eventType)
        {
            try
            {
                Entity e = new Entity("new_eventlog");
                e["new_name"] = string.Format("{0} - {1} - {2}", this.ApplicationName, functionName, DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                e["new_applicationname"] = this.ApplicationName;
                e["new_function"] = functionName;
                string exDetail = null;
                GetExceptionString(ex, out exDetail);
                e["new_detail"] = exDetail;
                e["new_eventtype"] = new OptionSetValue((int)eventType);
                this.OrgService.Create(e);
            }
            catch { }
        }
        public void Log(string functionName, Exception ex, EventType eventType, string relatedObjectType, string relatedObjectId)
        {
            try
            {
                Entity e = new Entity("new_eventlog");
                e["new_name"] = string.Format("{0} - {1} - {2}", this.ApplicationName, functionName, DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                e["new_applicationname"] = this.ApplicationName;
                e["new_function"] = functionName;
                string exDetail = null;
                GetExceptionString(ex, out exDetail);
                e["new_detail"] = exDetail;
                e["new_eventtype"] = new OptionSetValue((int)eventType);
                e["new_objecttype"] = relatedObjectType;
                e["new_objectid"] = relatedObjectId;

                this.OrgService.Create(e);
            }
            catch { }
        }
    }
}
