using GK.Library.Utility;
using Microsoft.Crm.Sdk.Messages;
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
    public static class ForumHelper
    {
        public static MsCrmResultObject GetUserForums(Guid portalId, Guid portalUserId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                f.new_forumId AS Id
	                                ,f.new_name AS Name
	                                ,f.new_parentforumId AS ParentForumId
	                                ,f.new_parentforumIdName AS ParentForumIdName
	                                ,f.CreatedOn
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
	                                f.statuscode=1 --Active
                                ORDER BY
                                    f.CreatedOn DESC";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalId, portalUserId));
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Forum> lstForum = new List<Forum>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        MsCrmResultObject frRes = ForumHelper.GetForumInfo((Guid)dt.Rows[i]["Id"], portalUserId, sda);

                        if (frRes.Success)
                        {
                            lstForum.Add((Forum)frRes.ReturnObject);
                        }
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = lstForum;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M046"; //"Forum kaydı bulunamadı!!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetForumInfo(Guid forumId, Guid portalUserId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                    f.new_forumId AS Id
	                                    ,f.new_name AS Name
	                                    ,f.new_parentforumId AS ParentForumId
	                                    ,f.new_parentforumIdName AS ParentForumIdName
	                                    ,f.CreatedOn
                                    FROM
	                                    new_forum AS f (NOLOCK)
                                    WHERE
	                                    f.new_forumId='{0}'
                                    AND
	                                    f.statecode=0
                                    AND
	                                    f.statuscode=1 --Active";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, forumId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    Forum fr = new Forum();

                    fr.Id = (Guid)dt.Rows[0]["Id"];
                    fr.Name = dt.Rows[0]["Name"] != DBNull.Value ? dt.Rows[0]["Name"].ToString() : string.Empty;
                    fr.CreatedOn = (DateTime)dt.Rows[0]["CreatedOn"];

                    if (dt.Rows[0]["ParentForumId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference();
                        er.Id = (Guid)dt.Rows[0]["ParentForumId"];
                        er.Name = dt.Rows[0]["ParentForumIdName"] != DBNull.Value ? dt.Rows[0]["ParentForumIdName"].ToString() : string.Empty;
                        er.LogicalName = "new_forum";

                        fr.ParentForum = er;
                    }

                    MsCrmResultObject subjectRes = ForumHelper.GetForumSubjects(fr.Id, sda);
                    if (subjectRes.Success)
                    {
                        fr.ForumSubjects = (List<ForumSubject>)subjectRes.ReturnObject;
                    }

                    MsCrmResultObject subForumRes = ForumHelper.GetSubForums(fr.Id, portalUserId, sda);
                    if (subForumRes.Success)
                    {
                        fr.SubForums = (List<Forum>)subForumRes.ReturnObject;
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = fr;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M047"; //"Foruma ait herhangi bir bilgi bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetForumSubjects(Guid forumId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                fs.new_forumsubjectId AS Id
	                                ,fs.new_name AS Name
	                                ,fs.new_userId AS UserId
	                                ,fs.new_userIdName AS UserIdName
	                                ,fs.new_content AS Content
	                                ,fs.CreatedOn
                                FROM
	                                new_forumsubject AS fs (NOLOCK)
                                WHERE
	                                fs.new_forumId='{0}'
                                AND
	                                fs.statecode=0
                                AND
	                                fs.statuscode=1 --Active
                                ORDER BY
	                                fs.CreatedOn DESC";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, forumId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<ForumSubject> lstSub = new List<ForumSubject>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        ForumSubject fs = new ForumSubject();

                        fs.Id = (Guid)dt.Rows[i]["Id"];
                        fs.Name = dt.Rows[i]["Name"] != DBNull.Value ? dt.Rows[i]["Name"].ToString() : string.Empty;
                        fs.Content = dt.Rows[i]["Content"] != DBNull.Value ? dt.Rows[i]["Content"].ToString() : string.Empty;
                        fs.CreatedOn = (DateTime)dt.Rows[i]["CreatedOn"];
                        fs.CreatedOnString = ((DateTime)dt.Rows[i]["CreatedOn"]).ToString("dd.MM.yyyy HH:mm");

                        if (dt.Rows[i]["UserId"] != DBNull.Value)
                        {
                            EntityReference er = new EntityReference();
                            er.Id = (Guid)dt.Rows[i]["UserId"];
                            er.Name = dt.Rows[i]["UserIdName"] != DBNull.Value ? dt.Rows[i]["UserIdName"].ToString() : string.Empty;
                            er.LogicalName = "new_user";

                            fs.User = er;
                        }

                        MsCrmResultObject resComment = CommentHelper.GetEntityComments(fs.Id, "new_forumsubject", 0, 10, sda);

                        if (resComment.Success)
                        {
                            fs.CommentList = (List<Comment>)resComment.ReturnObject;
                        }

                        lstSub.Add(fs);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = lstSub;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M048"; //"Foruma ait konu başlığı bulunmamaktadır!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetSubForums(Guid forumId, Guid portalUserId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                    f.new_forumId AS Id
	                                    ,f.new_name AS Name
	                                    ,f.new_parentforumId AS ParentForumId
	                                    ,f.new_parentforumIdName AS ParentForumIdName
	                                    ,f.CreatedOn
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
	                                    f.new_parentforumId='{0}'
                                    AND
	                                    f.statecode=0
                                    AND
	                                    f.statuscode=1 --Active
                                    ORDER BY
                                        f.CreatedOn";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, forumId, portalUserId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Forum> lstSubFr = new List<Forum>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Forum fr = new Forum();

                        fr.Id = (Guid)dt.Rows[i]["Id"];
                        fr.Name = dt.Rows[i]["Name"] != DBNull.Value ? dt.Rows[i]["Name"].ToString() : string.Empty;
                        fr.CreatedOn = (DateTime)dt.Rows[i]["CreatedOn"];

                        if (dt.Rows[i]["ParentForumId"] != DBNull.Value)
                        {
                            EntityReference er = new EntityReference();
                            er.Id = (Guid)dt.Rows[i]["ParentForumId"];
                            er.Name = dt.Rows[i]["ParentForumIdName"] != DBNull.Value ? dt.Rows[i]["ParentForumIdName"].ToString() : string.Empty;
                            er.LogicalName = "new_forum";

                            fr.ParentForum = er;
                        }

                        MsCrmResultObject subFrRes = ForumHelper.GetSubForums(fr.Id, portalUserId, sda);

                        if (subFrRes.Success)
                        {
                            fr.SubForums = (List<Forum>)subFrRes.ReturnObject;
                        }

                        lstSubFr.Add(fr);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = lstSubFr;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "Alt forum kaydı bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetForumSubjectInfo(Guid forumSubjectId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                fs.new_forumsubjectId AS Id
	                                ,fs.new_name AS Name
	                                ,fs.new_userId AS UserId
	                                ,fs.new_userIdName AS UserIdName
	                                ,fs.new_content AS Content
	                                ,fs.CreatedOn
	                                ,u.new_imageurl ImageUrl
                                FROM
	                                new_forumsubject AS fs (NOLOCK)
		                                JOIN
			                                new_user AS u (NOLOCK)
				                                ON
				                                u.new_userId=fs.new_userId
                                WHERE
	                                fs.new_forumsubjectId='{0}'
                                AND
	                                fs.statecode=0
                                AND
	                                fs.statuscode=1 --Active";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, forumSubjectId));

                if (dt != null && dt.Rows.Count > 0)
                {

                    ForumSubject fs = new ForumSubject();

                    fs.Id = (Guid)dt.Rows[0]["Id"];
                    fs.Name = dt.Rows[0]["Name"] != DBNull.Value ? dt.Rows[0]["Name"].ToString() : string.Empty;
                    fs.Content = dt.Rows[0]["Content"] != DBNull.Value ? dt.Rows[0]["Content"].ToString() : string.Empty;
                    fs.PortalUserImage = dt.Rows[0]["ImageUrl"] != DBNull.Value ? dt.Rows[0]["ImageUrl"].ToString() : "nouserprofile.jpg";
                    fs.CreatedOn = (DateTime)dt.Rows[0]["CreatedOn"];
                    fs.CreatedOnString = ((DateTime)dt.Rows[0]["CreatedOn"]).ToString("dd MMMM yyyy ddddd HH:mm", new CultureInfo("tr-TR", false));

                    if (dt.Rows[0]["UserId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference();
                        er.Id = (Guid)dt.Rows[0]["UserId"];
                        er.Name = dt.Rows[0]["UserIdName"] != DBNull.Value ? dt.Rows[0]["UserIdName"].ToString() : string.Empty;
                        er.LogicalName = "new_user";

                        fs.User = er;
                    }

                    MsCrmResultObject resComment = CommentHelper.GetEntityComments(fs.Id, "new_forumsubject", 0, 10, sda);

                    if (resComment.Success)
                    {
                        fs.CommentList = (List<Comment>)resComment.ReturnObject;
                    }

                    MsCrmResultObject resultLike = LikeHelper.GetEntityLikeInfo(fs.Id, "new_forumsubject", sda);

                    if (resultLike.Success)
                    {
                        fs.LikeDetail = (LikeInfo)resultLike.ReturnObject;
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = fs;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M049"; //"Forum konu başlığına ait bilgi bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult CreateForumSubject(ForumSubject forumSubject, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = new Entity("new_forumsubject");
                ent["new_name"] = forumSubject.Name;
                ent["new_forumid"] = forumSubject.Forum;
                ent["new_userid"] = forumSubject.User;
                ent["new_content"] = forumSubject.Content;
                ent["new_portalid"] = forumSubject.Portal;

                returnValue.CrmId = service.Create(ent);

                returnValue.Success = true;
                returnValue.Result = "M050"; //"Konu başlığı oluşturuldu.";
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static int GetUserForumSubjectCount(Guid portalId, Guid portalUserId, DateTime start, DateTime end, SqlDataAccess sda)
        {
            int returnValue = 0;

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
                                        COUNT(0)
                                    FROM
                                        new_forumsubject AS sa (NOLOCK)
		                                    JOIN
			                                    new_forum AS f (NOLOCK)
				                                    ON
				                                    sa.new_forumId=f.new_forumId
                                    WHERE
                                        f.new_portalId=@PortalId
                                    AND
                                        sa.new_userId=@UserId
                                    AND
                                        sa.CreatedOn BETWEEN @start AND @end
                                    AND
                                        sa.StateCode=0
                                    AND
	                                    sa.statuscode=1";

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
