using GK.Library.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GK.Library.Business
{
    public static class AnnouncementHelper
    {
        public static MsCrmResultObject GetAnnouncementList(Guid portalId, Guid portalUserId, int start, int end, SqlDataAccess sda)
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
                                     B.*
	                                ,ROW_NUMBER() OVER(ORDER BY B.CreatedOn DESC) AS RowNumber
                                    FROM
                                    (
	                                    SELECT DISTINCT
		                                    A.new_announcementId Id
		                                    ,A.new_name Name
		                                    ,A.new_content Content
		                                    ,A.new_imageurl ImageUrl
		                                    ,CAST({1}.dbo.fn_UTCToTzSpecificLocalTime(A.CreatedOn, us.TimeZoneBias, us.TimeZoneDaylightBias,us.TimeZoneDaylightYear, us.TimeZoneDaylightMonth, us.TimeZoneDaylightDay, us.TimeZoneDaylightHour,us.TimeZoneDaylightMinute, us.TimeZoneDaylightSecond, 0, us.TimeZoneDaylightDayOfWeek,us.TimeZoneStandardBias, us.TimeZoneStandardYear, us.TimeZoneStandardMonth, us.TimeZoneStandardDay,us.TimeZoneStandardHour, us.TimeZoneStandardMinute, us.TimeZoneStandardSecond, 0,us.TimeZoneStandardDayOfWeek) as DATETIME) CreatedOn
	                                    FROM
		                                    new_announcement A (NoLock)
	                                    JOIN
		                                    new_new_user_new_role AS UR (NOLOCK)
			                                    ON
			                                    UR.new_userid='{2}'
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
	                                    INNER JOIN 
		                                    dbo.UserSettingsBase US (NoLock)
		                                    ON 
		                                    US.SystemUserId ='{0}'
	                                    WHERE
		                                    @Date BETWEEN A.new_startdate AND A.new_enddate
	                                    AND
		                                    A.new_portalId='{3}'
	                                    AND
		                                    A.StateCode = 0
	                                    AND
		                                    A.StatusCode=1 --Active
                                    ) AS B
                                ) AS D
                            ) AS C
                                    WHERE
	                                    C.RowNumber BETWEEN {4} AND {5}
                                    ORDER BY 
                                        C.RowNumber ASC";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, Globals.AdminId, Globals.DatabaseName, portalUserId, portalId.ToString(), start.ToString(), end.ToString()));

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Announcement> returnList = new List<Announcement>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Announcement _announcement = new Announcement();
                        _announcement.AnnouncementId = (Guid)dt.Rows[i]["Id"];
                        _announcement.Caption = dt.Rows[i]["Name"] != DBNull.Value ? dt.Rows[i]["Name"].ToString() : string.Empty;
                        _announcement.Description = dt.Rows[i]["Content"] != DBNull.Value ? dt.Rows[i]["Content"].ToString() : string.Empty;
                        _announcement.ImagePath = dt.Rows[i]["ImageUrl"] != DBNull.Value ? dt.Rows[i]["ImageUrl"].ToString() : string.Empty;


                        if (dt.Rows[i]["TotalRow"] != DBNull.Value)
                        {
                            returnValue.RecordCount = (int)dt.Rows[i]["TotalRow"];
                        }

                        if (dt.Rows[i]["CreatedOn"] != DBNull.Value)
                        {
                            _announcement.CreatedOn = (DateTime)dt.Rows[i]["CreatedOn"];
                            _announcement.CreatedOnString = dt.Rows[i]["CreatedOn"] != DBNull.Value ? ((DateTime)dt.Rows[i]["CreatedOn"]).ToString("dd MMMM yyyy ddddd HH:mm", new CultureInfo("tr-TR", false)) : string.Empty;
                        }

                        returnList.Add(_announcement);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = returnList;
                }
                else
                {
                    returnValue.Success = true;
                    returnValue.Result = "M025"; //"Duyuru kaydı bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResultObject GetAnnouncementInfo(Guid announcementId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT 
	                                A.new_announcementId Id
	                                ,A.new_name Name
	                                ,A.new_content Content
	                                ,A.new_imageurl ImageUrl
                                    ,CAST({2}.dbo.fn_UTCToTzSpecificLocalTime(A.CreatedOn, us.TimeZoneBias, us.TimeZoneDaylightBias,us.TimeZoneDaylightYear, us.TimeZoneDaylightMonth, us.TimeZoneDaylightDay, us.TimeZoneDaylightHour,us.TimeZoneDaylightMinute, us.TimeZoneDaylightSecond, 0, us.TimeZoneDaylightDayOfWeek,us.TimeZoneStandardBias, us.TimeZoneStandardYear, us.TimeZoneStandardMonth, us.TimeZoneStandardDay,us.TimeZoneStandardHour, us.TimeZoneStandardMinute, us.TimeZoneStandardSecond, 0,us.TimeZoneStandardDayOfWeek) as DATETIME) CreatedOn
                                FROM
	                                new_announcement A (NoLock)
                                INNER JOIN
	                                SystemUser SU (NoLock)
	                                ON
	                                SU.SystemUserId = '{1}'
                                INNER JOIN 
	                                dbo.UserSettingsBase US (NoLock)
	                                ON 
	                                US.SystemUserId =SU.SystemUserId
                                WHERE
	                                A.new_announcementId = '{0}'
                                ORDER BY 
	                                A.CreatedOn DESC";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, announcementId, Globals.AdminId, Globals.DatabaseName));

                if (dt != null && dt.Rows.Count > 0)
                {
                    Announcement _announcement = new Announcement();
                    _announcement.AnnouncementId = (Guid)dt.Rows[0]["Id"];
                    _announcement.Caption = dt.Rows[0]["Name"] != DBNull.Value ? dt.Rows[0]["Name"].ToString() : string.Empty;
                    _announcement.Description = dt.Rows[0]["Content"] != DBNull.Value ? dt.Rows[0]["Content"].ToString() : string.Empty;
                    _announcement.ImagePath = dt.Rows[0]["ImageUrl"] != DBNull.Value ? dt.Rows[0]["ImageUrl"].ToString() : "no_image_available.png";

                    if (dt.Rows[0]["CreatedOn"] != DBNull.Value)
                    {
                        _announcement.CreatedOnString = dt.Rows[0]["CreatedOn"] != DBNull.Value ? ((DateTime)dt.Rows[0]["CreatedOn"]).ToString("dd MMMM yyyy ddddd HH:mm", new CultureInfo("tr-TR", false)) : string.Empty;
                        _announcement.CreatedOn = (DateTime)dt.Rows[0]["CreatedOn"];
                    }

                    #region | GET ATTACHMENTS |
                    MsCrmResultObject attachmentResult = AttachmentFileHelper.GetAnnouncementAttachmentFiles(announcementId, sda);
                    if (attachmentResult.Success)
                    {
                        _announcement.AttachmentFileList = (List<AttachmentFile>)attachmentResult.ReturnObject;
                    }
                    #endregion

                    returnValue.Success = true;
                    returnValue.ReturnObject = _announcement;
                }
                else
                {
                    returnValue.Result = "M026"; //"İlgili duyuruya ait bilgiye ulaşılamadı.";
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
