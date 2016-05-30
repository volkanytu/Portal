using GK.Library.Utility;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GK.Library.Business
{
    public static class GeneralHelper
    {
        #region | EXTENSION METHODS |

        public static object ToCrmType(this object objectValue, CrmDataType crmDataType)
        {
            object returnValue = objectValue;

            if (returnValue == null)
                return returnValue;


            switch (crmDataType)
            {
                case CrmDataType.UNIQUEIDENTIFIER:
                    if (objectValue.GetType() == typeof(string))
                    {
                        returnValue = new Guid(objectValue.ToString());
                    }
                    break;
                case CrmDataType.STRING:
                    returnValue = returnValue.ToString();
                    break;
                case CrmDataType.INT:
                    returnValue = Convert.ToInt32(returnValue);
                    break;
                case CrmDataType.DATETIME:
                    returnValue = Convert.ToDateTime(returnValue);
                    break;
                case CrmDataType.ENTITYREFERENCE:
                    if (objectValue.GetType() == typeof(EntityReferenceWrapper))
                    {
                        returnValue = ((EntityReferenceWrapper)objectValue).ToCrmEntityReference();
                    }
                    break;
                case CrmDataType.OPTIONSETVALUE:
                    if (objectValue.GetType() == typeof(OptionSetValueWrapper))
                    {
                        returnValue = ((OptionSetValueWrapper)objectValue).ToCrmOptionSetValue();
                    }
                    break;
                case CrmDataType.MONEY:
                    returnValue = new Money(Convert.ToDecimal(objectValue));
                    break;
                case CrmDataType.DECIMAL:
                    returnValue = Convert.ToDecimal(objectValue);
                    break;
                default:
                    break;
            }

            return returnValue;
        }

        public static EntityReference ToCrmEntityReference(this EntityReferenceWrapper entityReferenceWrapper)
        {
            EntityReference returnValue = new EntityReference()
            {
                LogicalName = entityReferenceWrapper.LogicalName,
                Id = entityReferenceWrapper.Id,
                Name = entityReferenceWrapper.Name
            };

            return returnValue;
        }

        public static OptionSetValue ToCrmOptionSetValue(this OptionSetValueWrapper optionSetValueWrapper)
        {
            OptionSetValue returnValue = new OptionSetValue((int)optionSetValueWrapper.AttributeValue);

            return returnValue;
        }

        public static Entity ToCrmEntity(this object entityObject)
        {
            Entity returnValue = null;

            if (entityObject == null)
            {
                return null;
            }

            System.Reflection.MemberInfo info = entityObject.GetType();

            var schemaAttr = info.GetCustomAttributes(typeof(CrmSchemaName), false).OfType<CrmSchemaName>().FirstOrDefault();

            if (schemaAttr != null)
            {
                string entityName = schemaAttr.SchemaName;
                returnValue = new Entity(entityName);
            }
            else
            {
                return null;
            }

            PropertyInfo[] properties = entityObject.GetType().GetProperties();

            foreach (PropertyInfo p in properties)
            {
                var attrFiledName = p.GetCustomAttributes(typeof(CrmFieldName), false).OfType<CrmFieldName>().FirstOrDefault();
                var attrFiledDataType = p.GetCustomAttributes(typeof(CrmFieldDataType), false).OfType<CrmFieldDataType>().FirstOrDefault();

                if (attrFiledName == null || attrFiledDataType == null)
                    continue;

                string fieldName = attrFiledName.FieldName;
                CrmDataType fieldDataType = attrFiledDataType.CrmDataType;

                var objectValue = entityObject.GetType().GetProperty(p.Name).GetValue(entityObject, null);

                if (objectValue != null)
                {
                    object crmFieldValue = objectValue.ToCrmType(fieldDataType);

                    if (objectValue.GetType() == typeof(Guid) && (Guid)crmFieldValue == Guid.Empty)
                        continue;

                    returnValue[fieldName] = objectValue.ToCrmType(fieldDataType);
                }
            }

            return returnValue;
        }

        public static List<TSource> ToList<TSource>(this DataTable dataTable) where TSource : new()
        {
            //TODO:GetAllProperties first persormance
            var dataList = new List<TSource>();

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return dataList;
            }

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            var objFieldNames = (from PropertyInfo aProp in typeof(TSource).GetProperties(flags)
                                 select new
                                 {
                                     Name = aProp.Name
                                     ,
                                     Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                                 }).ToList();
            var dataTblFieldNames = (from DataColumn aHeader in dataTable.Columns
                                     select new
                                     {
                                         Name = aHeader.ColumnName
                                         ,
                                         Type = aHeader.DataType
                                     }).ToList();
            var commonFields = objFieldNames.Select(x => x.Name).Intersect(dataTblFieldNames.Select(x => x.Name)).ToList();

            foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
            {
                var aTSource = new TSource();
                foreach (var aField in commonFields)
                {
                    if (dataRow[aField] != DBNull.Value)
                    {
                        PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField);
                        Type propertyType = Nullable.GetUnderlyingType(propertyInfos.PropertyType) ?? propertyInfos.PropertyType;

                        try
                        {
                            if (propertyType.IsEnum)
                            {
                                object enumValue = Enum.ToObject(propertyType, dataRow[aField]);
                                propertyInfos.SetValue(aTSource, enumValue, null);
                            }
                            else if (propertyType == typeof(EntityReferenceWrapper))
                            {
                                EntityReferenceWrapper erValue = new EntityReferenceWrapper { Id = (Guid)dataRow[aField] };

                                if (dataRow[aField + "Name"] != DBNull.Value)
                                {
                                    erValue.Name = dataRow[aField + "Name"].ToString();
                                }

                                if (dataRow[aField + "TypeName"] != DBNull.Value)
                                {
                                    erValue.LogicalName = dataRow[aField + "TypeName"].ToString();
                                }

                                propertyInfos.SetValue(aTSource, erValue, null);
                            }
                            else if (propertyType == typeof(OptionSetValueWrapper))
                            {
                                if (dataRow.Table.Columns.Contains(aField + "Value") && dataRow[aField + "Value"] != DBNull.Value)
                                {
                                    propertyInfos.SetValue(aTSource, new OptionSetValueWrapper() { AttributeValue = (int)dataRow[aField], Value = dataRow[aField + "Value"].ToString() }, null);
                                }
                                else
                                {
                                    propertyInfos.SetValue(aTSource, new OptionSetValueWrapper() { AttributeValue = (int)dataRow[aField] }, null);
                                }
                            }
                            else
                            {
                                propertyInfos.SetValue(aTSource, dataRow[aField], null);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                dataList.Add(aTSource);
            }
            return dataList;
        }

        public static TSource GetReturnObject<TSource>(this MsCrmResultObject msCrmResultObject) where TSource : new()
        {
            var returnValue = new TSource();

            if (msCrmResultObject.Success)
            {
                returnValue = (TSource)msCrmResultObject.ReturnObject;
            }

            return returnValue;
        }
        #endregion

        public static MsCrmResult SendMail()
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return returnValue;
        }

        public static MsCrmResult SendMail(Guid ObjectId, string ObjectType, Entity[] fromPartyArray, Entity[] toPartyArray, string subject, string mailBody, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                #region Create Email

                Entity email = new Entity("email");
                email["to"] = toPartyArray;
                email["from"] = fromPartyArray;
                email["subject"] = subject;
                email["description"] = mailBody;
                email["directioncode"] = true;

                if (ObjectId != Guid.Empty && !string.IsNullOrEmpty(ObjectType))
                {
                    EntityReference regardingObject = new EntityReference(ObjectType, ObjectId);
                    email.Attributes.Add("regardingobjectid", regardingObject);
                }

                returnValue.CrmId = service.Create(email);
                #endregion

                #region Send Email
                if (Globals.IsSendMailActive == "1")
                {
                    var req = new SendEmailRequest
                    {
                        EmailId = returnValue.CrmId,
                        TrackingToken = string.Empty,
                        IssueSend = true
                    };

                    var res = (SendEmailResponse)service.Execute(req);
                }
                #endregion

                returnValue.Success = true;
            }
            catch (Exception ex)
            {

            }

            return returnValue;
        }

        public static MsCrmResult GetCodeMessageDetail(string code, Guid languageId, SqlDataAccess sda)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, code, languageId));

                if (dt.Rows.Count > 0)
                {
                    returnValue.Result = dt.Rows[0].ToString();
                    returnValue.Success = true;
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static DateTime GetStartDateByScorePeriod(ScorePeriod period)
        {
            DateTime returnValue = new DateTime(2012, 1, 1);

            DateTime baseDate = DateTime.Today;
            var today = baseDate;
            var endOfToday = baseDate.AddDays(1).AddSeconds(-1);
            var yesterday = baseDate.AddDays(-1);
            var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            var lastWeekStart = thisWeekStart.AddDays(-7);
            var lastWeekEnd = thisWeekStart.AddSeconds(-1);
            var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
            var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
            var lastMonthStart = thisMonthStart.AddMonths(-1);
            var lastMonthEnd = thisMonthStart.AddSeconds(-1);

            if (period == ScorePeriod.Daily)
            {
                returnValue = today.ToUniversalTime();
            }
            else if (period == ScorePeriod.Weekly)
            {
                returnValue = thisWeekStart.ToUniversalTime();
            }
            else if (period == ScorePeriod.Monthly)
            {
                returnValue = thisMonthStart.ToUniversalTime();
            }


            return returnValue;
        }

        public static DateTime GetEndDateByScorePeriod(ScorePeriod period)
        {
            DateTime returnValue = new DateTime(2030, 12, 31);

            DateTime baseDate = DateTime.Today;
            var today = baseDate;
            var endOfToday = baseDate.AddDays(1).AddSeconds(-1);
            var yesterday = baseDate.AddDays(-1);
            var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            var lastWeekStart = thisWeekStart.AddDays(-7);
            var lastWeekEnd = thisWeekStart.AddSeconds(-1);
            var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
            var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
            var lastMonthStart = thisMonthStart.AddMonths(-1);
            var lastMonthEnd = thisMonthStart.AddSeconds(-1);

            if (period == ScorePeriod.Daily)
            {
                returnValue = endOfToday.ToUniversalTime();
            }
            else if (period == ScorePeriod.Weekly)
            {
                returnValue = thisWeekEnd;
            }
            else if (period == ScorePeriod.Monthly)
            {
                returnValue = thisMonthEnd.ToUniversalTime();
            }


            return returnValue;
        }

        public static MsCrmResultObject GetModuleRecordCount(Guid portalId, Guid portalUserId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            try
            {
                #region | SQL QUERY |
                string query = @"DECLARE @Date DATETIME = GETUTCDATE()
                                    --   1->Graffiti, 2->Education, 3->Article, 4->Video, 5->Announce, 6->Friend, 7->Forum
                                    SELECT
	                                    COUNT(0) AS RecCount
	                                    ,1 AS RecType
                                    FROM
                                        new_graffiti PG (NoLock)
                                    WHERE
	                                    PG.new_portalId='{0}' 
                                    AND
	                                    PG.statecode=0
                                    AND
	                                    PG.statuscode=1 --Active   
	
                                    UNION ALL

                                    SELECT 
	                                    COUNT(DISTINCT E.new_educationId) AS RecCount
	                                    ,2 AS RecType
                                    FROM
                                    new_education AS E (NOLOCK)
                                        JOIN
                                            new_new_user_new_role AS UR (NOLOCK)
                                                ON
                                                UR.new_userid='{1}'
                                        JOIN
                                            new_role AS RD (NOLOCK)
                                                ON
                                                RD.new_roleId=UR.new_roleid
                                                AND
                                                Rd.statecode=0
                                                AND
                                                RD.statuscode=1 --Active
                                        JOIN
                                            new_new_education_new_role AS ERDF (NOLOCK)
                                                ON
                                                ERDF.new_educationid=E.new_educationId
                                                AND
                                                ERDF.new_roleid=RD.new_roleId
                                    WHERE
                                        @Date BETWEEN E.new_startdate AND E.new_enddate
                                        AND
                                        E.new_portalId = '{0}'
                                        AND
                                        E.StateCode=0
                                        AND
                                        E.StatusCode=1 --Active
    
                                    UNION ALL

                                    SELECT 
                                        COUNT(DISTINCT E.new_articleId) AS RecCount
                                        ,3 AS RecType
                                    FROM
                                    new_article AS E (NOLOCK)
                                        JOIN
                                            new_new_user_new_role AS UR (NOLOCK)
                                                ON
                                                UR.new_userid='{1}'
                                        JOIN
                                            new_role AS RD (NOLOCK)
                                                ON
                                                RD.new_roleId=UR.new_roleid
                                                AND
                                                Rd.statecode=0
                                                AND
                                                RD.statuscode=1 --Active
                                        JOIN
                                            new_new_article_new_role AS ERDF (NOLOCK)
                                                ON
                                                ERDF.new_articleid=E.new_articleId
                                                AND
                                                ERDF.new_roleid =RD.new_roleId
                                    WHERE
                                        @Date BETWEEN E.new_startdate AND E.new_enddate
                                        AND
                                        E.new_portalId = '{0}'
                                        AND
                                        E.statuscode=1 --Active
    
                                    UNION ALL

                                    SELECT 
                                        COUNT(DISTINCT E.new_videoId) AS RecCount
                                        ,4 AS RecType
                                    FROM
                                    new_video AS E (NOLOCK)
                                        JOIN
                                            new_new_user_new_role AS UR (NOLOCK)
                                                ON
                                                UR.new_userid='{1}'
                                        JOIN
                                            new_role AS RD (NOLOCK)
                                                ON
                                                RD.new_roleId=UR.new_roleid
                                                AND
                                                Rd.statecode=0
                                                AND
                                                RD.statuscode=1 --Active
                                        JOIN
                                            new_new_video_new_role AS ERDF (NOLOCK)
                                                ON
                                                ERDF.new_videoid=E.new_videoId
                                                AND
                                                ERDF.new_roleid=RD.new_roleId
                                    WHERE
                                        @Date BETWEEN E.new_startdate AND E.new_enddate
                                     AND
                                        E.new_portalId = '{0}'
                                     AND
                                        E.statuscode=1 --Active

                                    UNION ALL

                                    SELECT 
                                        COUNT(DISTINCT A.new_announcementId) AS RecCount
                                        ,5 AS RecType
                                    FROM
                                        new_announcement A (NoLock)
                                    JOIN
                                        new_new_user_new_role AS UR (NOLOCK)
                                            ON
                                            UR.new_userid='{1}'
                                    JOIN
                                        new_role AS RD (NOLOCK)
                                            ON
                                            RD.new_roleId=UR.new_roleid
                                            AND
                                            Rd.statecode=0
                                            AND
                                            RD.statuscode=1 --Active
                                    JOIN		
                                        new_new_announcement_new_role AS ERDF (NOLOCK)
                                            ON
                                            ERDF.new_announcementid=A.new_announcementId
                                            AND
                                            ERDF.new_roleid=RD.new_roleId
                                    WHERE
                                        @Date BETWEEN A.new_startdate AND A.new_enddate
                                    AND
                                        A.new_portalId='{0}'
                                    AND
                                        A.StateCode = 0
                                    AND
                                        A.StatusCode=1 --Active
    
                                    UNION ALL

                                    SELECT
	                                    COUNT(0) AS RecCount
	                                    ,6 AS RecType
                                    FROM
                                        new_friendship AS f (NOLOCK)
                                    WHERE
                                        f.new_portalId='{0}'
                                    AND
                                        f.statecode=0
                                    AND
                                    (f.new_partyoneId='{1}' OR f.new_partytwoId='{1}')

                                    UNION ALL

                                    SELECT
	                                    COUNT(0) AS RecCount
	                                    ,7 AS RecType
                                    FROM
                                        new_forum AS f (NOLOCK)
                                            JOIN
                                                new_new_forum_new_role AS fr (NOLOCK)
                                                    ON
                                                    fr.new_forumid=f.new_forumId
                                            JOIN
                                                new_role AS r (NOLOCK)
                                                    ON
                                                    r.new_roleId=fr.new_roleid
                                                    AND
                                                    r.statecode=0
                                                    AND
                                                    r.statuscode=1 --Active
                                            JOIN
                                                new_new_user_new_role AS ur (NOLOCK)
                                                    ON
                                                    ur.new_userid='{1}'
                                    WHERE
                                        f.new_parentforumId IS NULL
                                    AND
                                        f.new_portalId='{0}'
                                    AND
                                        f.statecode=0
                                    AND
                                        f.statuscode=1 --Active    ";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalId, portalUserId));

                if (dt.Rows.Count > 0)
                {
                    List<ModuleCount> lstModuleCount = new List<ModuleCount>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ModuleCount mc = new ModuleCount()
                        {
                            RecCount = (int)dt.Rows[i]["RecCount"],
                            RecType = (int)dt.Rows[i]["RecType"]
                        };

                        lstModuleCount.Add(mc);
                    }


                    returnValue.Success = true;
                    returnValue.ReturnObject = lstModuleCount;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "Kayıt sayıları çekildi.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

    }
}
