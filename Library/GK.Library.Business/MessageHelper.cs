using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;

namespace GK.Library.Business
{
    public static class MessageHelper
    {
        public static MsCrmResult CreateMessage(MessageInfo messageInfo, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = messageInfo.ToCrmEntity();

                returnValue.CrmId = service.Create(ent);
                returnValue.Success = true;
                returnValue.Result = "Mesaj başarı ile oluşturuldu.";
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MessageInfo GetMessageInfo(Guid messageId, SqlDataAccess sda)
        {
            #region | SQL QUERY |
            string sqlQuery = @"SELECT
	                                m.new_messageId AS Id
	                                ,m.new_name AS Name
	                                ,m.new_from AS FromId
	                                ,m.new_fromName AS FromIdName
	                                ,'new_user' AS FromIdTypeName
	                                ,m.new_to AS ToId
	                                ,m.new_toName AS ToIdName
	                                ,'new_user' AS ToIdTypeName
	                                ,m.new_content AS Content
	                                ,m.CreatedOn
	                                ,m.StatusCode
	                                ,m.new_portalId AS PortalId
	                                ,m.new_portalIdName AS PortalIdName
	                                ,'new_portal' AS PortalIdTypeName
	                                --,uFrom.new_imageurl AS FromImageUrl
	                                --,uTo.new_imageurl AS ToImageUrl
                                FROM
                                new_message AS m (NOLOCK)
                                WHERE
                                m.new_messageId='{0}'";
            #endregion

            DataTable dt = sda.getDataTable(string.Format(sqlQuery, messageId));

            if (dt.Rows.Count > 0)
            {
                List<MessageInfo> mInfo = dt.ToList<MessageInfo>();

                return mInfo[0];
            }
            else
            {
                return null;
            }
        }

        public static List<MessageInfo> GetMessagesBetweenUsers(Guid portalId, Guid fromId, Guid toId, SqlDataAccess sda)
        {
            #region | SQL QUERY |
            string sqlQuery = @"SELECT
	                                m.new_messageId AS Id
	                                ,m.new_name AS Name
	                                ,m.new_from AS FromId
	                                ,m.new_fromName AS FromIdName
	                                ,'new_user' AS FromIdTypeName
	                                ,m.new_to AS ToId
	                                ,m.new_toName AS ToIdName
	                                ,'new_user' AS ToIdTypeName
	                                ,m.new_content AS Content
	                                ,m.CreatedOn
	                                ,m.StatusCode
	                                ,m.new_portalId AS PortalId
	                                ,m.new_portalIdName AS PortalIdName
	                                ,'new_portal' AS PortalIdTypeName
	                                ,uFrom.new_imageurl AS FromImageUrl
	                                ,uTo.new_imageurl AS ToImageUrl
                                FROM
                                new_message AS m (NOLOCK)
	                                JOIN
		                                new_user AS uFrom (NOLOCK)
			                                ON
			                                m.new_from=uFrom.new_userId
	                                JOIN
		                                new_user AS uTo (NOLOCK)
			                                ON
			                                m.new_to=uTo.new_userId
                                WHERE
                                m.new_portalId='{0}'
                                AND
                                (
	                                (m.new_from='{1}' AND m.new_to='{2}')
	                                OR
	                                (m.new_from='{2}' AND m.new_to='{1}')
                                )
                                AND
                                m.statecode=0
                                ORDER BY
	                                m.CreatedOn ASC";
            #endregion

            DataTable dt = sda.getDataTable(string.Format(sqlQuery, portalId, fromId, toId));

            if (dt.Rows.Count > 0)
            {
                List<MessageInfo> mList = dt.ToList<MessageInfo>();

                return mList;
            }
            else
            {
                return null;
            }
        }

        public static List<MessageInfo> GetUserRecentContacts(Guid portalId, Guid portalUserId, SqlDataAccess sda)
        {

            #region | SQL QUERY |

            string sqlQuery = @"SELECT
	                                B.*
	                                ,uFrom.new_imageurl AS FromImageUrl
	                                ,uTo.new_imageurl AS ToImageUrl
                                FROM
                                (
	                                SELECT
		                                A.*
		                                ,DENSE_RANK() OVER (PARTITION BY A.UserId ORDER BY A.CreatedOn DESC) AS Ranking
	                                FROM
	                                (
		                                SELECT
			                                m.Id
			                                --,m.new_name AS Name
			                                ,PortalId
			                                ,PortalIdName
			                                ,'new_portal' AS PortalIdTypeName
			                                ,m.FromId
			                                ,m.FromIdName
			                                ,'new_user' AS FromIdTypeName
			                                ,m.ToId
			                                ,m.ToIdName
			                                ,'new_user' AS ToIdTypeName
			                                ,m.Content
			                                ,m.CreatedOn
			                                --,m.StatusCode
			                                ,CASE WHEN m.FromId='{0}' THEN m.ToId ELSE m.FromId END AS UserId
			                                ,CASE WHEN m.FromId='{0}' THEN m.ToIdName ELSE m.FromIdName END AS UserIdName
		                                FROM
		                                v_Messages AS m (NOLOCK)
		                                WHERE
		                                m.PortalId='{1}'
		                                AND
		                                (m.FromId='{0}' OR ToId='{0}')
		                                --AND
		                                --m.statecode=0
	                                ) AS A
                                ) AS B
	                                JOIN
		                                new_user AS uFrom (NOLOCK)
			                                ON
			                                B.FromId=uFrom.new_userId
	                                JOIN
		                                new_user AS uTo (NOLOCK)
			                                ON
			                                B.ToId=uTo.new_userId
                                WHERE
                                B.Ranking=1 ORDER BY B.CreatedOn DESC";
            #endregion

            DataTable dt = sda.getDataTable(string.Format(sqlQuery, portalUserId, portalId));

            if (dt.Rows.Count > 0)
            {
                List<MessageInfo> mList = dt.ToList<MessageInfo>();

                return mList;
            }
            else
            {
                return null;
            }
        }

        public static MsCrmResult UpdateMessage(MessageInfo messageInfo, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = messageInfo.ToCrmEntity();

                service.Update(ent);

                returnValue.CrmId = messageInfo.Id;
                returnValue.Success = true;
                returnValue.Result = "Mesaj başarı ile güncellendi.";
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }
    }
}