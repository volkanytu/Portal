using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace GK.Library.Business
{
    public static class LikeHelper
    {
        public static MsCrmResult LikeEntity(Guid portalId, Guid portalUserId, Guid entityId, string entityName, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = new Entity("new_likerecord");
                ent["new_name"] = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                ent["new_portalid"] = new EntityReference("new_portal", portalId);
                ent["new_userid"] = new EntityReference("new_user", portalUserId);
                ent["new_liketype"] = true;
                ent[entityName + "id"] = new EntityReference(entityName, entityId);

                returnValue.CrmId = service.Create(ent);
                returnValue.Result = "M055"; //"Beğeni kaydınız oluşturuldu.";
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult DislikeEntity(Guid portalId, Guid portalUserId, Guid entityId, string entityName, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = new Entity("new_likerecord");
                ent["new_name"] = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                ent["new_portalid"] = new EntityReference("new_portal", portalId);
                ent["new_userid"] = new EntityReference("new_user", portalUserId);
                ent["new_liketype"] = false;
                ent[entityName + "id"] = new EntityReference(entityName, entityId);

                returnValue.CrmId = service.Create(ent);
                returnValue.Result = "M055";
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult ReportImproperContent(Guid portalId, Guid portalUserId, Guid entityId, string entityName, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = new Entity(entityName.ToLower());
                ent[entityName.ToLower() + "id"] = entityId;
                ent["new_isimpropercontent"] = true;

                service.Update(ent);


                returnValue.Result = "M096";
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetEntityLikeInfo(Guid entityId, string entityName, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"   DECLARE @LikeCount INT
                                    DECLARE @DislikeCount INT

                                    SELECT
	                                    @LikeCount= COUNT(0)
                                    FROM
	                                    new_likerecord AS l (NOLOCK)
                                    WHERE
	                                    l.{1}Id='{0}'
	                                    AND
	                                    l.new_liketype=1
                                        AND
                                        l.statecode=0

                                    SELECT
	                                    @DislikeCount= COUNT(0)
                                    FROM
	                                    new_likerecord AS l (NOLOCK)
                                    WHERE
	                                    l.{1}Id='{0}'
	                                    AND
	                                    l.new_liketype=0
                                        AND
                                        l.statecode=0

                                    SELECT 
	                                    @LikeCount AS LikeCount
	                                    ,@DislikeCount AS DislikeCount";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, entityId, entityName));

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Education> returnList = new List<Education>();
                    LikeInfo _like = new LikeInfo();
                    _like.LikeCount = (int)dt.Rows[0]["LikeCount"];
                    _like.DislikeCount = (int)dt.Rows[0]["DislikeCount"];
                    _like.Entity = new EntityReference(entityName, entityId);

                    returnValue.Success = true;
                    returnValue.ReturnObject = _like;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M056"; //"Beğeni kaydı bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult IsUserLikedBefore(Guid entityId, string entityName, Guid portalUserId, SqlDataAccess sda)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                COUNT(0) AS RecCount
                                FROM
	                                new_likerecord AS l (NOLOCK)
                                WHERE
	                                l.{1}Id='{0}'
	                                AND
	                                l.new_userId='{2}'
	                                AND
	                                l.statecode=0";
                #endregion

                int reCount = (int)sda.ExecuteScalar(string.Format(query, entityId, entityName, portalUserId));

                if (reCount > 0)
                {
                    returnValue.Success = true;
                    returnValue.Result = "M054"; //"Önceden beğeni kaydınız vardır.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }
    }
}
