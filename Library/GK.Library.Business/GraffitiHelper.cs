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
    public static class GraffitiHelper
    {
        public static MsCrmResult SaveOrUpdateGraffiti(Graffiti graffiti, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                Entity ent = new Entity("new_graffiti");

                ent["new_name"] = graffiti.PortalUser.Name + "-" + DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                ent["new_hasmedia"] = graffiti.HasMedia;

                if (graffiti.PortalUser != null)
                {
                    ent["new_userid"] = graffiti.PortalUser;
                }

                if (graffiti.Portal != null)
                {
                    ent["new_portalid"] = graffiti.Portal;
                }

                if (!string.IsNullOrEmpty(graffiti.Description))
                {
                    ent["new_content"] = graffiti.Description;
                }

                if (!string.IsNullOrEmpty(graffiti.ImagePath))
                {
                    ent["new_imageurl"] = graffiti.ImagePath;
                }

                if (graffiti.GraffitiId != Guid.Empty)
                {
                    ent["new_graffitiid"] = graffiti.GraffitiId;

                    service.Update(ent);
                    returnValue.Success = true;
                    returnValue.Result = "M014"; //"Duvar yazısı güncellendi.";
                }
                else
                {
                    returnValue.CrmId = service.Create(ent);
                    returnValue.Success = true;
                    returnValue.Result = "M015"; //"Duvar yazısı oluşturuldu.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
                returnValue.Success = false;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetGraffities(Guid portalId, int commentCount, int startRow, int endRow, SqlDataAccess sda)
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
		                                PG.new_graffitiId GraffitiId
		                                ,PG.new_content [Description]
		                                ,PG.new_userId PortalUserId
		                                ,PG.new_userIdName PortalUserIdName
		                                ,PG.new_portalId BrandId
		                                ,PG.new_portalIdName BrandIdName
		                                ,PG.new_imageurl [Image]
		                                ,U.new_imageurl UserImage
		                                ,CAST({4}.dbo.fn_UTCToTzSpecificLocalTime(PG.CreatedOn, us.TimeZoneBias, us.TimeZoneDaylightBias,us.TimeZoneDaylightYear, us.TimeZoneDaylightMonth, us.TimeZoneDaylightDay, us.TimeZoneDaylightHour,us.TimeZoneDaylightMinute, us.TimeZoneDaylightSecond, 0, us.TimeZoneDaylightDayOfWeek,us.TimeZoneStandardBias, us.TimeZoneStandardYear, us.TimeZoneStandardMonth, us.TimeZoneStandardDay,us.TimeZoneStandardHour, us.TimeZoneStandardMinute, us.TimeZoneStandardSecond, 0,us.TimeZoneStandardDayOfWeek) as DATETIME) CreatedOn
		                                ,ROW_NUMBER() OVER(ORDER BY PG.CreatedOn DESC) AS RowNumber
                                        ,(
			                                SELECT
				                                COUNT(0)
			                                FROM
				                                new_comment AS c (NOLOCK)
			                                WHERE
				                                c.new_graffitiId=PG.new_graffitiId
                                        ) AS CommentCount
                                        ,(
			                                SELECT
				                                COUNT(0)
			                                FROM
				                                new_likerecord AS lr (NOLOCK)
			                                WHERE
				                                lr.new_graffitiId=PG.new_graffitiId
                                        ) AS LikeCount
	                                FROM
		                                new_graffiti PG (NoLock)
	                                INNER JOIN
		                                new_user U (NoLock)
		                                ON
		                                PG.new_portalId = '{0}'
		                                AND
		                                PG.statecode = 0
		                                AND
		                                U.new_userId = PG.new_userId
	                                INNER JOIN 
		                                dbo.UserSettingsBase US (NoLock)
		                                ON 
		                                US.SystemUserId ='{1}'   
                                    WHERE
	                                    PG.statecode=0
                                    AND
	                                    PG.statuscode=1 --Active    
                                )A
                                WHERE
	                                A.RowNumber BETWEEN {2} AND {3}
                                ORDER BY 
                                    A.RowNumber ASC";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalId, Globals.AdminId, startRow, endRow, Globals.DatabaseName));
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Graffiti> returnList = new List<Graffiti>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Graffiti _graffiti = new Graffiti();
                        _graffiti.GraffitiId = (Guid)dt.Rows[i]["GraffitiId"];
                        _graffiti.Description = dt.Rows[i]["Description"] != DBNull.Value ? dt.Rows[i]["Description"].ToString() : string.Empty;
                        _graffiti.ImagePath = dt.Rows[i]["Image"] != DBNull.Value ? dt.Rows[i]["Image"].ToString() : string.Empty;
                        _graffiti.PortalUserImage = dt.Rows[i]["UserImage"] != DBNull.Value ? dt.Rows[i]["UserImage"].ToString() : "nouserprofile.jpg";
                        _graffiti.CreatedOnString = dt.Rows[i]["CreatedOn"] != DBNull.Value ? ((DateTime)dt.Rows[i]["CreatedOn"]).ToString("dd MMMM yyyy, HH:mm", new CultureInfo("tr-TR", false)) : string.Empty;
                        _graffiti.HasMedia = dt.Rows[i]["Image"] != DBNull.Value ? true : false;

                        _graffiti.LikeCount = (int)dt.Rows[i]["LikeCount"];
                        _graffiti.CommentCount = (int)dt.Rows[i]["CommentCount"];

                        if (dt.Rows[i]["CreatedOn"] != DBNull.Value)
                        {
                            _graffiti.CreatedOn = (DateTime)dt.Rows[i]["CreatedOn"];
                        }

                        if (dt.Rows[i]["PortalUserId"] != DBNull.Value)
                        {
                            EntityReference er = new EntityReference();
                            er.LogicalName = "new_user";
                            er.Id = (Guid)dt.Rows[i]["PortalUserId"];
                            if (dt.Rows[i]["PortalUserIdName"] != DBNull.Value) { er.Name = dt.Rows[i]["PortalUserIdName"].ToString(); }

                            _graffiti.PortalUser = er;
                        }

                        if (dt.Rows[i]["BrandId"] != DBNull.Value)
                        {
                            EntityReference er = new EntityReference();
                            er.LogicalName = "new_brand";
                            er.Id = (Guid)dt.Rows[i]["BrandId"];
                            if (dt.Rows[i]["BrandIdName"] != DBNull.Value) { er.Name = dt.Rows[i]["BrandIdName"].ToString(); }

                            _graffiti.Portal = er;
                        }

                        #region | GET COMMENTS |
                        MsCrmResultObject commentResult = CommentHelper.GetGraffitiComments(_graffiti.GraffitiId, 0, commentCount, sda);
                        //MsCrmResultObject commentResult = CommentHelper.GetGraffitiComments(_graffiti.GraffitiId, sda);
                        if (commentResult.Success)
                        {
                            _graffiti.CommentList = (List<Comment>)commentResult.ReturnObject;
                        }
                        #endregion

                        returnList.Add(_graffiti);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = returnList;
                }
                else
                {
                    returnValue.Success = true;
                    returnValue.Result = "M013"; //"Duvar yazısı bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static int GetUserGraffitiCount(Guid portalId, Guid portalUserId, DateTime start, DateTime end, bool hasMedia, SqlDataAccess sda)
        {
            int returnValue = 0;

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
                                        COUNT(0)
                                    FROM
                                        new_graffiti AS gr (NOLOCK)
                                    WHERE
                                        gr.new_portalId=@PortalId
                                    AND
                                        gr.new_userId=@UserId
                                    AND
                                        gr.CreatedOn BETWEEN @start AND @end
                                    AND
                                        gr.StateCode=0
                                    AND
	                                    gr.statuscode=1 --Etkin
                                    AND
	                                    gr.new_imageurl  " + (hasMedia ? " IS NOT NULL" : " IS NULL");

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
