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
    public static class ArticleHelper
    {
        public static MsCrmResultObject GetArticleList(Guid portalId, Guid portalUserId, int startRow, int endRow, Guid categoryId, SqlDataAccess sda)
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
		                                    E.new_articleId ArticleId		                                
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
				                                    PC.new_articleId =E.new_articleId
			                                    AND
				                                    PC.statecode=0
			                                    AND
				                                    PC.statuscode=1 --Active
		                                      ) AS CommentCount
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
		                                    JOIN
			                                    dbo.UserSettingsBase US (NOLOCK)
				                                    ON 
				                                    US.SystemUserId ='{2}'
	                                    WHERE
                                            E.new_categoryId='{6}' 
                                            AND
		                                    @Date BETWEEN E.new_startdate AND E.new_enddate
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
                    List<Article> returnList = new List<Article>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Article _article = new Article();
                        _article.ArticleId = (Guid)dt.Rows[i]["ArticleId"];
                        _article.Name = dt.Rows[i]["Name"] != DBNull.Value ? dt.Rows[i]["Name"].ToString() : string.Empty;
                        _article.Summary = dt.Rows[i]["Summary"] != DBNull.Value ? dt.Rows[i]["Summary"].ToString() : string.Empty;
                        _article.Description = dt.Rows[i]["Description"] != DBNull.Value ? dt.Rows[i]["Description"].ToString() : string.Empty;
                        _article.ImagePath = dt.Rows[i]["Image"] != DBNull.Value ? dt.Rows[i]["Image"].ToString() : "no_image_available.png";
                        _article.CreatedOnString = dt.Rows[i]["CreatedOn"] != DBNull.Value ? ((DateTime)dt.Rows[i]["CreatedOn"]).ToString("dd MMMM yyyy ddddd HH:mm", new CultureInfo("tr-TR", false)) : string.Empty;
                        _article.CommentCount = dt.Rows[i]["CommentCount"] != DBNull.Value ? (int)dt.Rows[i]["CommentCount"] : 0;

                        if (dt.Rows[i]["TotalRow"] != DBNull.Value)
                        {
                            returnValue.RecordCount = (int)dt.Rows[i]["TotalRow"];
                        }

                        if (dt.Rows[i]["CreatedOn"] != DBNull.Value)
                        {
                            _article.CreatedOn = (DateTime)dt.Rows[i]["CreatedOn"];
                        }

                        MsCrmResultObject likeResult = LikeHelper.GetEntityLikeInfo(_article.ArticleId, "new_article", sda);

                        if (likeResult.Success)
                        {
                            _article.LikeDetail = (LikeInfo)likeResult.ReturnObject;
                        }

                        returnList.Add(_article);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = returnList;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M019"; //"Makale kaydı bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetArticleInfo(Guid articleId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
                                        A.new_articleId ArticleId
	                                    ,A.new_name Name
	                                    ,A.new_summary Summary
	                                    ,A.new_content [Description]
	                                    ,A.new_imageurl [Image]
	                                    ,CAST({2}.dbo.fn_UTCToTzSpecificLocalTime(A.CreatedOn, us.TimeZoneBias, us.TimeZoneDaylightBias,us.TimeZoneDaylightYear, us.TimeZoneDaylightMonth, us.TimeZoneDaylightDay, us.TimeZoneDaylightHour,us.TimeZoneDaylightMinute, us.TimeZoneDaylightSecond, 0, us.TimeZoneDaylightDayOfWeek,us.TimeZoneStandardBias, us.TimeZoneStandardYear, us.TimeZoneStandardMonth, us.TimeZoneStandardDay,us.TimeZoneStandardHour, us.TimeZoneStandardMinute, us.TimeZoneStandardSecond, 0,us.TimeZoneStandardDayOfWeek) as DATETIME) CreatedOn
                                FROM
	                                new_article A (NoLock)
                                 INNER JOIN 
	                                dbo.UserSettingsBase US (NoLock)
	                                ON 
	                                US.SystemUserId ='{1}'
                                WHERE
	                                A.new_articleId = '{0}'";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, articleId, Globals.AdminId, Globals.DatabaseName));
                if (dt != null && dt.Rows.Count > 0)
                {
                    Article _article = new Article();
                    _article.ArticleId = (Guid)dt.Rows[0]["ArticleId"];
                    _article.Name = dt.Rows[0]["Name"].ToString();
                    _article.Summary = dt.Rows[0]["Summary"].ToString();
                    _article.Description = dt.Rows[0]["Description"].ToString();
                    _article.ImagePath = dt.Rows[0]["Image"] != DBNull.Value ? dt.Rows[0]["Image"].ToString() : "no_image_available.png";
                    _article.CreatedOnString = dt.Rows[0]["CreatedOn"] != DBNull.Value ? ((DateTime)dt.Rows[0]["CreatedOn"]).ToString("dd MMMM yyyy ddddd HH:mm", new CultureInfo("tr-TR", false)) : string.Empty;

                    #region | GET COMMENTS |
                    MsCrmResultObject commentResult = CommentHelper.GetArticleComments(articleId, sda);
                    if (commentResult.Success)
                    {
                        _article.CommentList = (List<Comment>)commentResult.ReturnObject;
                    }
                    #endregion

                    #region | GET ATTACHMENTS |
                    MsCrmResultObject attachmentResult = AttachmentFileHelper.GetArticleAttachmentFiles(articleId, sda);
                    if (attachmentResult.Success)
                    {
                        _article.AttachmentFileList = (List<AttachmentFile>)attachmentResult.ReturnObject;
                    }
                    #endregion

                    returnValue.Success = true;
                    returnValue.ReturnObject = _article;
                }
                else
                {
                    returnValue.Success = true;
                    returnValue.Result = "M020"; //"İstenilen makaleye ait detay bulunamadı!";
                }

            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult SaveOrUpdateArticle(Article article, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                Entity ent = new Entity("new_article");

                if (!string.IsNullOrEmpty(article.Name))
                {
                    ent["new_name"] = article.Name;
                }

                if (!string.IsNullOrEmpty(article.Summary))
                {
                    ent["new_summary"] = article.Summary;
                }

                if (!string.IsNullOrEmpty(article.Description))
                {
                    ent["new_content"] = article.Description;
                }

                if (!string.IsNullOrEmpty(article.ImagePath))
                {
                    ent["new_imageurl"] = article.ImagePath;
                }

                if (article.ArticleId != Guid.Empty)
                {
                    ent["new_articleid"] = article.ArticleId;

                    service.Update(ent);
                    returnValue.Success = true;
                    returnValue.Result = "Makale başarıyla güncellendi";
                }
                else
                {
                    returnValue.CrmId = service.Create(ent);
                    returnValue.Success = true;
                    returnValue.Result = "Makale başarıyla oluşturuldu";
                }


            }
            catch (Exception)
            {

                throw;
            }
            return returnValue;
        }

        public static MsCrmResult LogArticle(Guid portalUserId, Guid portalId, Guid articleId, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                Entity ent = new Entity("new_articlelog");
                ent["new_name"] = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                ent["new_portalid"] = new EntityReference("new_portal", portalId);
                ent["new_userid"] = new EntityReference("new_user", portalUserId);
                ent["new_articleid"] = new EntityReference("new_article", articleId);

                returnValue.CrmId = service.Create(ent);
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetArticleCategoryList(Guid portalId, Guid portalUserId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT DISTINCT
	                                E.new_articlecategoryId AS Id                                
	                                ,E.new_name Name
	                                ,E.new_portalId PortalId
	                                ,E.new_portalIdName PortalIdName
                                    ,E.new_imageurl AS ImageUrl
                                FROM
                                new_articlecategory AS E (NOLOCK)
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
		                                new_new_articlecategory_new_role AS ERDF (NOLOCK)
			                                ON
			                                ERDF.new_articlecategoryid =E.new_articlecategoryId
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
                    returnValue.Result = "M021"; //"Makale kategori kaydı bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetArticleCategoryInfo(Guid categoryId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT DISTINCT
	                                E.new_articlecategoryId AS Id                                
	                                ,E.new_name Name
	                                ,E.new_portalId PortalId
	                                ,E.new_portalIdName PortalIdName
                                    ,E.new_imageurl AS ImageUrl
                                FROM
                                new_articlecategory AS E (NOLOCK)	                                
                                WHERE
	                                E.new_articlecategoryId = '{0}'
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

        public static int GetArticleEditCount(Guid portalId, Guid portalUserId, DateTime start, DateTime end, SqlDataAccess sda)
        {
            int returnValue = 0;

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
                                        COUNT(0)
                                    FROM
                                        new_articlelog AS al (NOLOCK)
                                    WHERE
                                        al.new_portalId=@PortalId
                                    AND
                                        al.new_userId=@UserId
                                    AND
                                        al.CreatedOn BETWEEN @start AND @end
                                    AND
                                        al.StateCode=0";

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
