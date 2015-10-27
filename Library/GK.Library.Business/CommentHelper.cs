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
    public static class CommentHelper
    {
        public static MsCrmResult SaveOrUpdateComment(Comment comment, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                Entity ent = new Entity("new_comment");

                ent["new_name"] = comment.PortalUser.Name + "-" + DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                if (comment.PortalUser != null)
                {
                    ent["new_userid"] = comment.PortalUser;
                }

                if (comment.PortalUser != null)
                {
                    ent["new_portalid"] = comment.Portal;
                }

                if (comment.Education != null)
                {
                    ent["new_educationid"] = comment.Education;
                }

                if (comment.Article != null)
                {
                    ent["new_articleid"] = comment.Article;
                }

                if (comment.Graffiti != null)
                {
                    ent["new_graffitiid"] = comment.Graffiti;
                }

                if (comment.Video != null)
                {
                    ent["new_videoid"] = comment.Video;
                }

                if (comment.ForumSubject != null)
                {
                    ent["new_forumsubjectid"] = comment.ForumSubject;
                }

                if (comment.Description != null && comment.Description != "")
                {
                    ent["new_content"] = comment.Description;

                }

                if (comment.CommentId != Guid.Empty)
                {
                    ent["new_commentid"] = comment.CommentId;

                    service.Update(ent);
                    returnValue.Success = true;
                    returnValue.Result = "Yorum kaydı başarıyla güncellendi";
                }
                else
                {
                    returnValue.CrmId = service.Create(ent);
                    returnValue.Success = true;
                    returnValue.Result = "M058"; //"Yorum başarı ile gönderildi.";
                }

            }
            catch (Exception)
            {

                throw;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetGraffitiComments(Guid graffitiId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                PC.new_commentId CommentId
	                                ,PC.new_userId PortalUserId
	                                ,PC.new_userIdName PortalUserIdName
	                                ,U.new_imageurl [Image]
                                    ,PC.new_content Comment
	                                ,CAST({2}.dbo.fn_UTCToTzSpecificLocalTime(PC.CreatedOn, us.TimeZoneBias, us.TimeZoneDaylightBias,us.TimeZoneDaylightYear, us.TimeZoneDaylightMonth, us.TimeZoneDaylightDay, us.TimeZoneDaylightHour,us.TimeZoneDaylightMinute, us.TimeZoneDaylightSecond, 0, us.TimeZoneDaylightDayOfWeek,us.TimeZoneStandardBias, us.TimeZoneStandardYear, us.TimeZoneStandardMonth, us.TimeZoneStandardDay,us.TimeZoneStandardHour, us.TimeZoneStandardMinute, us.TimeZoneStandardSecond, 0,us.TimeZoneStandardDayOfWeek) as DATETIME) CreatedOn
                                FROM
	                                new_comment PC (NoLock)
                                INNER JOIN
	                                new_user U (NoLock)
	                                ON
	                                PC.new_graffitiId = '{0}'
	                                AND
	                                U.new_userId = PC.new_userId
                                INNER JOIN 
	                                dbo.UserSettingsBase US (NoLock)
	                                ON 
	                                US.SystemUserId ='{1}' 
                                WHERE
	                                PC.statecode = 0
                                    AND
                                    PC.statuscode=1 --Active
                                ORDER BY
	                                PC.CreatedOn ASC";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, graffitiId, Globals.AdminId, Globals.DatabaseName));
                returnValue.Success = true;

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Comment> returnList = new List<Comment>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Comment _comment = new Comment();
                        _comment.CommentId = (Guid)dt.Rows[i]["CommentId"];
                        _comment.Description = dt.Rows[i]["Comment"] != DBNull.Value ? dt.Rows[i]["Comment"].ToString() : string.Empty;
                        _comment.PortalUserImage = dt.Rows[i]["Image"] != DBNull.Value ? dt.Rows[i]["Image"].ToString() : "nouserprofile.jpg";
                        _comment.CreatedOnString = dt.Rows[i]["CreatedOn"] != DBNull.Value ? ((DateTime)dt.Rows[i]["CreatedOn"]).ToString("dd MMMM yyyy ddddd HH:mm", new CultureInfo("tr-TR", false)) : string.Empty;

                        if (dt.Rows[i]["CreatedOn"] != DBNull.Value)
                        {
                            _comment.CreatedOn = (DateTime)dt.Rows[i]["CreatedOn"];
                        }

                        if (dt.Rows[i]["PortalUserId"] != DBNull.Value)
                        {
                            EntityReference er = new EntityReference();
                            er.LogicalName = "new_user";
                            er.Id = (Guid)dt.Rows[i]["PortalUserId"];
                            if (dt.Rows[i]["PortalUserIdName"] != DBNull.Value) { er.Name = dt.Rows[i]["PortalUserIdName"].ToString(); }

                            _comment.PortalUser = er;
                        }

                        returnList.Add(_comment);
                    }

                    returnValue.ReturnObject = returnList;
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetGraffitiComments(Guid graffitiId, int start, int end, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                A.*
                                FROM
                                (
                                SELECT
	                                PC.new_commentId CommentId
	                                ,PC.new_userId PortalUserId
	                                ,PC.new_userIdName PortalUserIdName
	                                ,U.new_imageurl [Image]
                                    ,PC.new_content Comment
	                                ,CAST({2}.dbo.fn_UTCToTzSpecificLocalTime(PC.CreatedOn, us.TimeZoneBias, us.TimeZoneDaylightBias,us.TimeZoneDaylightYear, us.TimeZoneDaylightMonth, us.TimeZoneDaylightDay, us.TimeZoneDaylightHour,us.TimeZoneDaylightMinute, us.TimeZoneDaylightSecond, 0, us.TimeZoneDaylightDayOfWeek,us.TimeZoneStandardBias, us.TimeZoneStandardYear, us.TimeZoneStandardMonth, us.TimeZoneStandardDay,us.TimeZoneStandardHour, us.TimeZoneStandardMinute, us.TimeZoneStandardSecond, 0,us.TimeZoneStandardDayOfWeek) as DATETIME) CreatedOn
                                    ,ROW_NUMBER() OVER(ORDER BY PC.CreatedOn DESC) AS RowNumber
                                FROM
	                                new_comment PC (NoLock)
                                INNER JOIN
	                                new_user U (NoLock)
	                                ON
	                                PC.new_graffitiId = '{0}'
	                                AND
	                                U.new_userId = PC.new_userId
                                INNER JOIN 
	                                dbo.UserSettingsBase US (NoLock)
	                                ON 
	                                US.SystemUserId ='{1}' 
                                WHERE
	                                PC.statecode = 0
                                    AND
                                    PC.statuscode=1 --Active
                                 )A
                                WHERE
	                                A.RowNumber BETWEEN {3} AND {4}
                                ORDER BY 
                                    A.RowNumber ASC";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, graffitiId, Globals.AdminId, Globals.DatabaseName, start, end));

                returnValue.Success = true;

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Comment> returnList = new List<Comment>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Comment _comment = new Comment();
                        _comment.CommentId = (Guid)dt.Rows[i]["CommentId"];
                        _comment.Description = dt.Rows[i]["Comment"] != DBNull.Value ? dt.Rows[i]["Comment"].ToString() : string.Empty;
                        _comment.PortalUserImage = dt.Rows[i]["Image"] != DBNull.Value ? dt.Rows[i]["Image"].ToString() : "nouserprofile.jpg";
                        _comment.CreatedOnString = dt.Rows[i]["CreatedOn"] != DBNull.Value ? ((DateTime)dt.Rows[i]["CreatedOn"]).ToString("dd MMMM yyyy ddddd HH:mm", new CultureInfo("tr-TR", false)) : string.Empty;

                        if (dt.Rows[i]["CreatedOn"] != DBNull.Value)
                        {
                            _comment.CreatedOn = (DateTime)dt.Rows[i]["CreatedOn"];
                        }

                        if (dt.Rows[i]["PortalUserId"] != DBNull.Value)
                        {
                            EntityReference er = new EntityReference();
                            er.LogicalName = "new_user";
                            er.Id = (Guid)dt.Rows[i]["PortalUserId"];
                            if (dt.Rows[i]["PortalUserIdName"] != DBNull.Value) { er.Name = dt.Rows[i]["PortalUserIdName"].ToString(); }

                            _comment.PortalUser = er;
                        }

                        returnList.Add(_comment);
                    }

                    returnValue.ReturnObject = returnList;
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetEducationComments(Guid educationId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                PC.new_commentId CommentId
	                                ,PC.new_userId PortalUserId
	                                ,PC.new_userIdName PortalUserIdName
	                                ,U.new_imageurl [Image]
                                    ,PC.new_content Comment
	                                ,CAST({2}.dbo.fn_UTCToTzSpecificLocalTime(PC.CreatedOn, us.TimeZoneBias, us.TimeZoneDaylightBias,us.TimeZoneDaylightYear, us.TimeZoneDaylightMonth, us.TimeZoneDaylightDay, us.TimeZoneDaylightHour,us.TimeZoneDaylightMinute, us.TimeZoneDaylightSecond, 0, us.TimeZoneDaylightDayOfWeek,us.TimeZoneStandardBias, us.TimeZoneStandardYear, us.TimeZoneStandardMonth, us.TimeZoneStandardDay,us.TimeZoneStandardHour, us.TimeZoneStandardMinute, us.TimeZoneStandardSecond, 0,us.TimeZoneStandardDayOfWeek) as DATETIME) CreatedOn
                                FROM
	                                new_comment PC (NoLock)
                                INNER JOIN
	                                new_user U (NoLock)
	                                ON
	                                PC.new_educationId = '{0}'
	                                AND
	                                U.new_userId = PC.new_userId
                                INNER JOIN 
	                                dbo.UserSettingsBase US (NoLock)
	                                ON 
	                                US.SystemUserId = '{1}' 
                                WHERE
	                                PC.statecode = 0
                                    AND
                                    PC.statuscode=1 --Active
                                ORDER BY
	                                PC.CreatedOn ASC";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, educationId, Globals.AdminId, Globals.DatabaseName));
                returnValue.Success = true;

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Comment> returnList = new List<Comment>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Comment _comment = new Comment();
                        _comment.CommentId = (Guid)dt.Rows[i]["CommentId"];
                        _comment.Description = dt.Rows[i]["Comment"] != DBNull.Value ? dt.Rows[i]["Comment"].ToString() : string.Empty;
                        _comment.PortalUserImage = dt.Rows[i]["Image"] != DBNull.Value ? dt.Rows[i]["Image"].ToString() : "nouserprofile.jpg";
                        _comment.CreatedOnString = dt.Rows[i]["CreatedOn"] != DBNull.Value ? ((DateTime)dt.Rows[i]["CreatedOn"]).ToString("dd MMMM yyyy ddddd HH:mm", new CultureInfo("tr-TR", false)) : string.Empty;

                        if (dt.Rows[i]["CreatedOn"] != DBNull.Value)
                        {
                            _comment.CreatedOn = (DateTime)dt.Rows[i]["CreatedOn"];
                        }

                        if (dt.Rows[i]["PortalUserId"] != DBNull.Value)
                        {
                            EntityReference er = new EntityReference();
                            er.LogicalName = "new_user";
                            er.Id = (Guid)dt.Rows[i]["PortalUserId"];
                            if (dt.Rows[i]["PortalUserIdName"] != DBNull.Value) { er.Name = dt.Rows[i]["PortalUserIdName"].ToString(); }

                            _comment.PortalUser = er;
                        }


                        returnList.Add(_comment);
                    }

                    returnValue.ReturnObject = returnList;
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetArticleComments(Guid articleId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                PC.new_commentId CommentId
	                                ,PC.new_userId PortalUserId
	                                ,PC.new_userIdName PortalUserIdName
	                                ,U.new_imageurl [Image]
                                    ,PC.new_content Comment
	                                ,CAST({2}.dbo.fn_UTCToTzSpecificLocalTime(PC.CreatedOn, us.TimeZoneBias, us.TimeZoneDaylightBias,us.TimeZoneDaylightYear, us.TimeZoneDaylightMonth, us.TimeZoneDaylightDay, us.TimeZoneDaylightHour,us.TimeZoneDaylightMinute, us.TimeZoneDaylightSecond, 0, us.TimeZoneDaylightDayOfWeek,us.TimeZoneStandardBias, us.TimeZoneStandardYear, us.TimeZoneStandardMonth, us.TimeZoneStandardDay,us.TimeZoneStandardHour, us.TimeZoneStandardMinute, us.TimeZoneStandardSecond, 0,us.TimeZoneStandardDayOfWeek) as DATETIME) CreatedOn
                                FROM
	                                new_comment PC (NoLock)
                                INNER JOIN
	                                new_user U (NoLock)
	                                ON
	                                PC.new_articleId = '{0}'
	                                AND
	                                U.new_userId = PC.new_userId
                                INNER JOIN 
	                                dbo.UserSettingsBase US (NoLock)
	                                ON 
	                                US.SystemUserId = '{1}' 
                                WHERE
	                                PC.statecode = 0
                                    AND
                                    PC.statuscode=1 --Active
                                ORDER BY
	                                PC.CreatedOn ASC";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, articleId, Globals.AdminId, Globals.DatabaseName));
                returnValue.Success = true;

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Comment> returnList = new List<Comment>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Comment _comment = new Comment();
                        _comment.CommentId = (Guid)dt.Rows[i]["CommentId"];
                        _comment.Description = dt.Rows[i]["Comment"] != DBNull.Value ? dt.Rows[i]["Comment"].ToString() : string.Empty;
                        _comment.PortalUserImage = dt.Rows[i]["Image"] != DBNull.Value ? dt.Rows[i]["Image"].ToString() : "nouserprofile.jpg";
                        _comment.CreatedOnString = dt.Rows[i]["CreatedOn"] != DBNull.Value ? ((DateTime)dt.Rows[i]["CreatedOn"]).ToString("dd MMMM yyyy ddddd HH:mm", new CultureInfo("tr-TR", false)) : string.Empty;

                        if (dt.Rows[i]["CreatedOn"] != DBNull.Value)
                        {
                            _comment.CreatedOn = (DateTime)dt.Rows[i]["CreatedOn"];
                        }

                        if (dt.Rows[i]["PortalUserId"] != DBNull.Value)
                        {
                            EntityReference er = new EntityReference();
                            er.LogicalName = "new_user";
                            er.Id = (Guid)dt.Rows[i]["PortalUserId"];
                            if (dt.Rows[i]["PortalUserIdName"] != DBNull.Value) { er.Name = dt.Rows[i]["PortalUserIdName"].ToString(); }

                            _comment.PortalUser = er;
                        }


                        returnList.Add(_comment);
                    }

                    returnValue.ReturnObject = returnList;
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetEntityComments(Guid entityId, string entityName, int start, int end, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                A.*
                                FROM
                                    (
	                                    SELECT
		                                    D.*
		                                    ,COUNT(0) OVER() AS TotalRow
	                                    FROM
	                                    (
                                            SELECT
	                                            PC.new_commentId CommentId
	                                            ,PC.new_userId PortalUserId
	                                            ,PC.new_userIdName PortalUserIdName
	                                            ,U.new_imageurl [Image]
                                                ,PC.new_content Comment
	                                            ,CAST({2}.dbo.fn_UTCToTzSpecificLocalTime(PC.CreatedOn, us.TimeZoneBias, us.TimeZoneDaylightBias,us.TimeZoneDaylightYear, us.TimeZoneDaylightMonth, us.TimeZoneDaylightDay, us.TimeZoneDaylightHour,us.TimeZoneDaylightMinute, us.TimeZoneDaylightSecond, 0, us.TimeZoneDaylightDayOfWeek,us.TimeZoneStandardBias, us.TimeZoneStandardYear, us.TimeZoneStandardMonth, us.TimeZoneStandardDay,us.TimeZoneStandardHour, us.TimeZoneStandardMinute, us.TimeZoneStandardSecond, 0,us.TimeZoneStandardDayOfWeek) as DATETIME) CreatedOn
                                                ,ROW_NUMBER() OVER(ORDER BY PC.CreatedOn DESC) AS RowNumber
                                            FROM
	                                            new_comment PC (NoLock)
                                            INNER JOIN
	                                            new_user U (NoLock)
	                                            ON
	                                            PC.{5}Id = '{0}'
	                                            AND
	                                            U.new_userId = PC.new_userId
                                            INNER JOIN 
	                                            dbo.UserSettingsBase US (NoLock)
	                                            ON 
	                                            US.SystemUserId ='{1}' 
                                            WHERE
	                                            PC.statecode = 0
                                                AND
                                                PC.statuscode=1 --Active
                                        ) AS D
                                 )AS A
                                WHERE
	                                A.RowNumber BETWEEN {3} AND {4}
                                ORDER BY 
                                    A.RowNumber ASC";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, entityId, Globals.AdminId, Globals.DatabaseName, start, end, entityName.ToLower()));

                returnValue.Success = true;

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Comment> returnList = new List<Comment>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Comment _comment = new Comment();
                        _comment.CommentId = (Guid)dt.Rows[i]["CommentId"];
                        _comment.Description = dt.Rows[i]["Comment"] != DBNull.Value ? dt.Rows[i]["Comment"].ToString() : string.Empty;
                        _comment.PortalUserImage = dt.Rows[i]["Image"] != DBNull.Value ? dt.Rows[i]["Image"].ToString() : "nouserprofile.jpg";
                        _comment.CreatedOnString = dt.Rows[i]["CreatedOn"] != DBNull.Value ? ((DateTime)dt.Rows[i]["CreatedOn"]).ToString("dd MMMM yyyy ddddd HH:mm", new CultureInfo("tr-TR", false)) : string.Empty;

                        if (dt.Rows[i]["TotalRow"] != DBNull.Value)
                        {
                            returnValue.RecordCount = (int)dt.Rows[i]["TotalRow"];
                        }

                        if (dt.Rows[i]["CreatedOn"] != DBNull.Value)
                        {
                            _comment.CreatedOn = (DateTime)dt.Rows[i]["CreatedOn"];
                        }

                        if (dt.Rows[i]["PortalUserId"] != DBNull.Value)
                        {
                            EntityReference er = new EntityReference();
                            er.LogicalName = "new_user";
                            er.Id = (Guid)dt.Rows[i]["PortalUserId"];
                            if (dt.Rows[i]["PortalUserIdName"] != DBNull.Value) { er.Name = dt.Rows[i]["PortalUserIdName"].ToString(); }

                            _comment.PortalUser = er;
                        }

                        returnList.Add(_comment);
                    }

                    returnValue.ReturnObject = returnList;
                }
                else
                {
                    returnValue.Result = "M006"; //Henüz yorum yok
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static int GetUserCommentCount(Guid portalId, Guid portalUserId, DateTime start, DateTime end, SqlDataAccess sda)
        {
            int returnValue = 0;

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
                                        COUNT(0)
                                    FROM
                                        new_comment AS cm (NOLOCK)
                                    WHERE
	                                    cm.new_portalId=@PortalId
                                    AND
                                        cm.new_userId=@UserId
                                    AND
                                        cm.CreatedOn BETWEEN @start AND @end
                                    AND
                                        cm.StateCode=0
                                    AND
	                                    cm.statuscode=1 --Etkin";

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
