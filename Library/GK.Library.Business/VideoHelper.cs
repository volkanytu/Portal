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
    public static class VideoHelper
    {
        public static MsCrmResultObject GetVideoList(Guid portalId, Guid portalUserId, int startRow, int endRow, Guid categoryId, SqlDataAccess sda)
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
		                                    E.new_videoId AS Id                                
		                                    ,E.new_name Name
		                                    ,E.new_summary Summary
		                                    ,E.new_imageurl [Image]
											,E.new_videourl AS [Video]
		                                    ,CAST({3}.dbo.fn_UTCToTzSpecificLocalTime(E.CreatedOn, us.TimeZoneBias, us.TimeZoneDaylightBias,us.TimeZoneDaylightYear, us.TimeZoneDaylightMonth, us.TimeZoneDaylightDay, us.TimeZoneDaylightHour,us.TimeZoneDaylightMinute, us.TimeZoneDaylightSecond, 0, us.TimeZoneDaylightDayOfWeek,us.TimeZoneStandardBias, us.TimeZoneStandardYear, us.TimeZoneStandardMonth, us.TimeZoneStandardDay,us.TimeZoneStandardHour, us.TimeZoneStandardMinute, us.TimeZoneStandardSecond, 0,us.TimeZoneStandardDayOfWeek) as DATETIME) CreatedOn
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
		                                    JOIN
			                                    dbo.UserSettingsBase US (NOLOCK)
				                                    ON 
				                                    US.SystemUserId ='{2}'
	                                    WHERE
                                            " + (categoryId == Guid.Empty ? "" : " E.new_categoryId='{6}' AND ") +
                                            @" @Date BETWEEN E.new_startdate AND E.new_enddate
		                                    AND
		                                    E.new_portalId = '{0}'
                                            AND
                                            E.statuscode=1 --Active
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
                    List<Video> returnList = new List<Video>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Video _video = new Video();
                        _video.Id = (Guid)dt.Rows[i]["Id"];
                        _video.Name = dt.Rows[i]["Name"] != DBNull.Value ? dt.Rows[i]["Name"].ToString() : string.Empty;
                        _video.Summary = dt.Rows[i]["Summary"] != DBNull.Value ? dt.Rows[i]["Summary"].ToString() : string.Empty;
                        _video.ImagePath = dt.Rows[i]["Image"] != DBNull.Value ? dt.Rows[i]["Image"].ToString() : string.Empty;
                        _video.VideoPath = dt.Rows[i]["Video"] != DBNull.Value ? dt.Rows[i]["Video"].ToString() : string.Empty;
                        _video.CreatedOnString = dt.Rows[i]["CreatedOn"] != DBNull.Value ? ((DateTime)dt.Rows[i]["CreatedOn"]).ToString("dd MMMM yyyy ddddd HH:mm", new CultureInfo("tr-TR", false)) : string.Empty;
                        //_video.CommentCount = dt.Rows[i]["CommentCount"] != DBNull.Value ? (int)dt.Rows[i]["CommentCount"] : 0;

                        if (dt.Rows[i]["TotalRow"] != DBNull.Value)
                        {
                            returnValue.RecordCount = (int)dt.Rows[i]["TotalRow"];
                        }

                        if (dt.Rows[i]["CreatedOn"] != DBNull.Value)
                        {
                            _video.CreatedOn = (DateTime)dt.Rows[i]["CreatedOn"];
                        }

                        returnList.Add(_video);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = returnList;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M022"; //"Video kaydı bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetVideoInfo(Guid videoId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
                                    E.new_videoId AS Id                                
		                            ,E.new_name Name
		                            ,E.new_summary Summary
		                            ,E.new_imageurl [Image]
									,E.new_videourl AS [Video]
                                    ,E.new_youtubeurl AS [YoutubeUrl]
	                                ,CAST({2}.dbo.fn_UTCToTzSpecificLocalTime(E.CreatedOn, us.TimeZoneBias, us.TimeZoneDaylightBias,us.TimeZoneDaylightYear, us.TimeZoneDaylightMonth, us.TimeZoneDaylightDay, us.TimeZoneDaylightHour,us.TimeZoneDaylightMinute, us.TimeZoneDaylightSecond, 0, us.TimeZoneDaylightDayOfWeek,us.TimeZoneStandardBias, us.TimeZoneStandardYear, us.TimeZoneStandardMonth, us.TimeZoneStandardDay,us.TimeZoneStandardHour, us.TimeZoneStandardMinute, us.TimeZoneStandardSecond, 0,us.TimeZoneStandardDayOfWeek) as DATETIME) CreatedOn
                                FROM
	                                new_video E (NoLock)
                                 INNER JOIN 
	                                dbo.UserSettingsBase US (NoLock)
	                                ON 
	                                US.SystemUserId ='{1}'
                                WHERE
	                                E.new_videoId = '{0}'";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, videoId, Globals.AdminId, Globals.DatabaseName));
                if (dt != null && dt.Rows.Count > 0)
                {
                    Video _video = new Video();
                    _video.Id = (Guid)dt.Rows[0]["Id"];
                    _video.Name = dt.Rows[0]["Name"].ToString();
                    _video.Summary = dt.Rows[0]["Summary"].ToString();
                    _video.ImagePath = dt.Rows[0]["Image"] != DBNull.Value ? dt.Rows[0]["Image"].ToString() : "no-video-bg.png";
                    _video.VideoPath = dt.Rows[0]["Video"] != DBNull.Value ? dt.Rows[0]["Video"].ToString() : string.Empty;
                    _video.YouTubeUrl = dt.Rows[0]["YoutubeUrl"] != DBNull.Value ? dt.Rows[0]["YoutubeUrl"].ToString() : string.Empty;
                    _video.CreatedOnString = dt.Rows[0]["CreatedOn"] != DBNull.Value ? ((DateTime)dt.Rows[0]["CreatedOn"]).ToString("dd MMMM yyyy ddddd HH:mm", new CultureInfo("tr-TR", false)) : string.Empty;

                    #region | GET COMMENTS |
                    MsCrmResultObject commentResult = CommentHelper.GetEntityComments(videoId, "new_video", 0, 100, sda);
                    if (commentResult.Success)
                    {
                        _video.CommentList = (List<Comment>)commentResult.ReturnObject;
                    }
                    #endregion

                    MsCrmResultObject likeResult = LikeHelper.GetEntityLikeInfo(_video.Id, "new_video", sda);

                    if (likeResult.Success)
                    {
                        _video.LikeDetail = (LikeInfo)likeResult.ReturnObject;
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = _video;
                }
                else
                {
                    returnValue.Success = true;
                    returnValue.Result = "M023"; //"Video detayı bulunamadı!";
                }

            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult LogVideo(Guid portalUserId, Guid portalId, Guid videoId, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                Entity ent = new Entity("new_videolog");
                ent["new_name"] = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                ent["new_portalid"] = new EntityReference("new_portal", portalId);
                ent["new_userid"] = new EntityReference("new_user", portalUserId);
                ent["new_videoid"] = new EntityReference("new_video", videoId);

                returnValue.CrmId = service.Create(ent);
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetVideoCategoryList(Guid portalId, Guid portalUserId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT DISTINCT
	                                E.new_videocategoryId AS Id                                
	                                ,E.new_name Name
	                                ,E.new_portalId PortalId
	                                ,E.new_portalIdName PortalIdName
                                    ,E.new_imageurl AS ImageUrl
                                FROM
                                new_videocategory AS E (NOLOCK)
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
		                                new_new_videocategory_new_role AS ERDF (NOLOCK)
			                                ON
			                                ERDF.new_videocategoryid =E.new_videocategoryId
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
                            LogicalName = dt.Rows[i]["ImageUrl"] != DBNull.Value ? dt.Rows[i]["ImageUrl"].ToString() : "no-video-bg.png"
                        };

                        returnList.Add(er);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = returnList;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M024"; //"Video kategori kaydı bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetVideoCategoryInfo(Guid categoryId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT DISTINCT
	                                E.new_videocategoryId AS Id                                
	                                ,E.new_name Name
	                                ,E.new_portalId PortalId
	                                ,E.new_portalIdName PortalIdName
                                    ,E.new_imageurl AS ImageUrl
                                FROM
                                new_videocategory AS E (NOLOCK)	                                
                                WHERE
	                                E.new_videocategoryId = '{0}'
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
                        LogicalName = dt.Rows[0]["ImageUrl"] != DBNull.Value ? dt.Rows[0]["ImageUrl"].ToString() : "no-video-large.png"
                    };

                    returnValue.Success = true;
                    returnValue.ReturnObject = er;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M024";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static int GetVideoEditCount(Guid portalId, Guid portalUserId, DateTime start, DateTime end, SqlDataAccess sda)
        {
            int returnValue = 0;

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
                                        COUNT(0)
                                    FROM
                                        new_videolog AS vl (NOLOCK)
                                    WHERE
                                        vl.new_portalId=@PortalId
                                    AND
                                        vl.new_userId=@UserId
                                    AND
                                        vl.CreatedOn BETWEEN @start AND @end
                                    AND
                                        vl.StateCode=0";

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
