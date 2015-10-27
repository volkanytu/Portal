using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GK.Library.Business
{
    public static class EducationHelper
    {
        public static MsCrmResultObject GetEducationList(Guid portalId, Guid portalUserId, int startRow, int endRow, Guid categoryId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"DECLARE @Date DATETIME = GETUTCDATE()
SELECT
*
FROM
(
	                        SELECT
		                        D.*
		                        ,COUNT(0) OVER() AS TotalRow
	                        FROM
	                        (
                                SELECT
                                         A.*
                                        ,ROW_NUMBER() OVER(ORDER BY A.CreatedOn DESC) AS RowNumber
                                FROM
                                (
	                                SELECT DISTINCT
		                                E.new_educationId EducationId		                                
		                                ,E.new_name Name
		                                ,E.new_summary Summary
		                                ,E.new_content [Description]
		                                ,E.new_imageurl [Image]
		                                ,CAST({3}.dbo.fn_UTCToTzSpecificLocalTime(E.CreatedOn, us.TimeZoneBias, us.TimeZoneDaylightBias,us.TimeZoneDaylightYear, us.TimeZoneDaylightMonth, us.TimeZoneDaylightDay, us.TimeZoneDaylightHour,us.TimeZoneDaylightMinute, us.TimeZoneDaylightSecond, 0, us.TimeZoneDaylightDayOfWeek,us.TimeZoneStandardBias, us.TimeZoneStandardYear, us.TimeZoneStandardMonth, us.TimeZoneStandardDay,us.TimeZoneStandardHour, us.TimeZoneStandardMinute, us.TimeZoneStandardSecond, 0,us.TimeZoneStandardDayOfWeek) as DATETIME) CreatedOn
                                        ,(
			                                SELECT 
				                                COUNT(0)
			                                FROM 
				                                new_comment AS PC (NOLOCK) 
			                                WHERE 
				                                PC.new_educationId=E.new_educationId
			                                AND
				                                PC.statecode=0
			                                AND
				                                PC.statuscode=1 --Active
		                                  ) AS CommentCount
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
		                                JOIN
			                                dbo.UserSettingsBase US (NoLock)
				                                ON 
				                                US.SystemUserId ='{2}'
	                                WHERE
                                        E.new_categoryId='{6}' 
                                        AND
		                                @Date BETWEEN E.new_startdate AND E.new_enddate
		                                AND
		                                E.new_portalId = '{0}'
                                        AND
                                        E.StateCode=0
                                        AND
                                        E.StatusCode=1 --Active
                                ) AS A
                        ) AS D
) AS B
    WHERE
	    B.RowNumber BETWEEN {4} AND {5}
    ORDER BY 
        B.RowNumber ASC";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalId, portalUserId, Globals.AdminId, Globals.DatabaseName, startRow, endRow, categoryId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Education> returnList = new List<Education>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Education _education = new Education();
                        _education.EducationId = (Guid)dt.Rows[i]["EducationId"];
                        _education.Name = dt.Rows[i]["Name"] != DBNull.Value ? dt.Rows[i]["Name"].ToString() : string.Empty;
                        _education.Summary = dt.Rows[i]["Summary"] != DBNull.Value ? dt.Rows[i]["Summary"].ToString() : string.Empty;
                        _education.Description = dt.Rows[i]["Description"] != DBNull.Value ? dt.Rows[i]["Description"].ToString() : string.Empty;
                        _education.ImagePath = dt.Rows[i]["Image"] != DBNull.Value ? dt.Rows[i]["Image"].ToString() : "no_image_available.png";
                        _education.CreatedOnString = dt.Rows[i]["CreatedOn"] != DBNull.Value ? ((DateTime)dt.Rows[i]["CreatedOn"]).ToString("dd MMMM yyyy ddddd HH:mm", new CultureInfo("tr-TR", false)) : string.Empty;
                        _education.CommentCount = dt.Rows[i]["CommentCount"] != DBNull.Value ? (int)dt.Rows[i]["CommentCount"] : 0;

                        if (dt.Rows[i]["TotalRow"] != DBNull.Value)
                        {
                            returnValue.RecordCount = (int)dt.Rows[i]["TotalRow"];
                        }

                        if (dt.Rows[i]["CreatedOn"] != DBNull.Value)
                        {
                            _education.CreatedOn = (DateTime)dt.Rows[i]["CreatedOn"];
                        }


                        MsCrmResultObject likeResult = LikeHelper.GetEntityLikeInfo(_education.EducationId, "new_education", sda);

                        if (likeResult.Success)
                        {
                            _education.LikeDetail = (LikeInfo)likeResult.ReturnObject;
                        }

                        returnList.Add(_education);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = returnList;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M016"; //"Eğitim kaydı bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetEducationInfo(Guid educationId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                E.new_educationId EducationId
	                                ,E.new_name Name
	                                ,E.new_summary Summary
	                                ,E.new_content [Description]
	                                ,E.new_imageurl [Image]
	                                ,CAST({2}.dbo.fn_UTCToTzSpecificLocalTime(E.CreatedOn, us.TimeZoneBias, us.TimeZoneDaylightBias,us.TimeZoneDaylightYear, us.TimeZoneDaylightMonth, us.TimeZoneDaylightDay, us.TimeZoneDaylightHour,us.TimeZoneDaylightMinute, us.TimeZoneDaylightSecond, 0, us.TimeZoneDaylightDayOfWeek,us.TimeZoneStandardBias, us.TimeZoneStandardYear, us.TimeZoneStandardMonth, us.TimeZoneStandardDay,us.TimeZoneStandardHour, us.TimeZoneStandardMinute, us.TimeZoneStandardSecond, 0,us.TimeZoneStandardDayOfWeek) as DATETIME) CreatedOn
                                FROM
	                                new_education E (NoLock)
                                INNER JOIN 
	                                dbo.UserSettingsBase US (NoLock)
	                                ON 
	                                US.SystemUserId ='{1}'
                                WHERE
	                                E.new_educationId = '{0}'";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, educationId, Globals.AdminId, Globals.DatabaseName));
                if (dt != null && dt.Rows.Count > 0)
                {
                    Education _education = new Education();
                    _education.EducationId = (Guid)dt.Rows[0]["EducationId"];
                    _education.Name = dt.Rows[0]["Name"].ToString();
                    _education.Summary = dt.Rows[0]["Summary"].ToString();
                    _education.Description = dt.Rows[0]["Description"].ToString();
                    _education.ImagePath = dt.Rows[0]["Image"] != DBNull.Value ? dt.Rows[0]["Image"].ToString() : "no_image_available.png";
                    _education.CreatedOnString = dt.Rows[0]["CreatedOn"] != DBNull.Value ? ((DateTime)dt.Rows[0]["CreatedOn"]).ToString("dd MMMM yyyy ddddd HH:mm", new CultureInfo("tr-TR", false)) : string.Empty;

                    MsCrmResultObject resultLike = LikeHelper.GetEntityLikeInfo(_education.EducationId, "new_graffiti", sda);

                    if (resultLike.Success)
                    {
                        _education.LikeDetail = (LikeInfo)resultLike.ReturnObject;
                    }

                    #region | GET COMMENTS |
                    MsCrmResultObject commentResult = CommentHelper.GetEducationComments(educationId, sda);
                    if (commentResult.Success)
                    {
                        _education.CommentList = (List<Comment>)commentResult.ReturnObject;
                    }
                    #endregion

                    #region | GET ATTACHMENTS |
                    MsCrmResultObject attachmentResult = AttachmentFileHelper.GetEducationAttachmentFiles(educationId, sda);
                    if (attachmentResult.Success)
                    {
                        _education.AttachmentFileList = (List<AttachmentFile>)attachmentResult.ReturnObject;
                    }
                    #endregion

                    returnValue.Success = true;
                    returnValue.ReturnObject = _education;
                }
                else
                {
                    returnValue.Success = true;
                    returnValue.Result = "M017"; //"Eğitim kaydına ulaşılamadı.";
                }

            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResult LogEducation(Guid portalUserId, Guid portalId, Guid educationId, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                Entity ent = new Entity("new_educationlog");
                ent["new_name"] = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                ent["new_portalid"] = new EntityReference("new_portal", portalId);
                ent["new_userid"] = new EntityReference("new_user", portalUserId);
                ent["new_educationid"] = new EntityReference("new_education", educationId);

                returnValue.CrmId = service.Create(ent);
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetEducationCategoryList(Guid portalId, Guid portalUserId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT DISTINCT
	                                E.new_educationcategoryId AS Id                                
	                                ,E.new_name Name
	                                ,E.new_portalId PortalId
	                                ,E.new_portalIdName PortalIdName
                                    ,E.new_imageurl AS ImageUrl
                                FROM
                                new_educationcategory AS E (NOLOCK)
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
		                                new_new_educationcategory_new_role AS ERDF (NOLOCK)
			                                ON
			                                ERDF.new_educationcategoryid =E.new_educationcategoryId
			                                AND
			                                ERDF.new_roleid=RD.new_roleId
                                WHERE
	                                E.new_portalId = '{0}'
                                    AND
                                    E.statuscode=1 --Active";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalId, portalUserId));
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<EntityReference> returnList = new List<EntityReference>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        EntityReference er = new EntityReference()
                        {
                            Id = (Guid)dt.Rows[i]["Id"],
                            Name = dt.Rows[i]["Name"].ToString(),
                            LogicalName = dt.Rows[i]["ImageUrl"] != DBNull.Value ? dt.Rows[i]["ImageUrl"].ToString() : "no_image_available.png"
                        };

                        returnList.Add(er);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = returnList;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M018"; //"Eğitim kategori kaydı bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetEducationCategoryInfo(Guid categoryId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT DISTINCT
	                                E.new_educationcategoryId AS Id                                
	                                ,E.new_name Name
	                                ,E.new_portalId PortalId
	                                ,E.new_portalIdName PortalIdName
                                    ,E.new_imageurl AS ImageUrl
                                FROM
                                new_educationcategory AS E (NOLOCK)	                                
                                WHERE
	                                E.new_educationcategoryId = '{0}'
                                    AND
                                    E.statuscode=1 --Active";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, categoryId));
                if (dt != null && dt.Rows.Count > 0)
                {
                    EntityReference er = new EntityReference()
                    {
                        Id = (Guid)dt.Rows[0]["Id"],
                        Name = dt.Rows[0]["Name"].ToString(),
                        LogicalName = dt.Rows[0]["ImageUrl"] != DBNull.Value ? dt.Rows[0]["ImageUrl"].ToString() : "no_image_available.png"
                    };

                    returnValue.Success = true;
                    returnValue.ReturnObject = er;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M018"; //"Eğitim kategori kaydı bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static int GetEducationEditCount(Guid portalId, Guid portalUserId, DateTime start, DateTime end, SqlDataAccess sda)
        {
            int returnValue = 0;

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
                                        COUNT(0)
                                    FROM
                                        new_educationlog AS el (NOLOCK)
                                    WHERE
                                        el.new_portalId=@PortalId
                                    AND
                                        el.new_userId=@UserId
                                    AND
                                        el.CreatedOn BETWEEN @start AND @end
                                    AND
                                        el.StateCode=0";

                #endregion

                SqlParameter[] parameters = {
                                            new SqlParameter("@PortalId",portalId)
                                            ,new SqlParameter("@UserId",portalUserId)
                                            ,new SqlParameter("@start",start)
                                            ,new SqlParameter("@end",end)
                                        };

                returnValue = (int)sda.ExecuteScalar(sqlQuery, parameters);

            }
            catch (Exception ex)
            {

            }

            return returnValue;
        }
    }
}
