using GK.Library.Utility;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GK.Library.Business
{
    public static class GiftHelper
    {
        public static Dictionary<string, string> InterlinkOrderErrorCodes
        {
            get
            {
                Dictionary<string, string> returnValue = new Dictionary<string, string>();

                returnValue.Add("A2", "Şirket Yok");
                returnValue.Add("A3", "Katalog Yok");
                returnValue.Add("A4", "Şirketin Altında Katalog Yok");
                returnValue.Add("A5", "Ürün Katalog'ta Yok");
                returnValue.Add("A6", "Oluşturulma Tarihi Yok");
                returnValue.Add("A7", "Form Geliş Tarihi Yok");
                returnValue.Add("A8", "Adet Yok");
                returnValue.Add("A9", "Sehir Kodu Yok");
                returnValue.Add("A10", "Alıcı Adı Yok");
                returnValue.Add("A11", "Mükerrer Kayıt");
                returnValue.Add("A12", "Sistemsel hata");

                return returnValue;
            }
        }

        public static MsCrmResultObject GetGiftCategoryList(Guid portalId, Guid portalUserId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT DISTINCT
                                    E.new_giftcategoryId AS Id                                
                                    ,E.new_name Name
                                    ,E.new_portalid PortalId
                                    ,E.new_portalidName PortalIdName
                                    ,'new_portal' AS PortalIdTypeName
                                    ,E.new_imageurl AS ImageUrl
                                FROM
                                new_giftcategory AS E (NOLOCK)
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
                                        new_new_giftcategory_new_role AS ERDF (NOLOCK)
                                            ON
                                            ERDF.new_giftcategoryid =E.new_giftcategoryId
                                            AND
                                            ERDF.new_roleid=RD.new_roleId
                                WHERE
                                    E.new_portalid = '{0}'
                                    AND
                                    E.statuscode=1 --Active";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalId, portalUserId));

                if (dt.Rows.Count > 0)
                {
                    List<GiftCategory> giftCategoryList = dt.ToList<GiftCategory>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = giftCategoryList;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "Hediye kategori kaydı bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetGiftList(Guid portalId, Guid portalUserId, Guid categoryId, string sortType, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"
                                SELECT DISTINCT
                                    E.new_giftId Id
                                    ,E.new_name Name
                                    ,E.new_summary Summary
                                    ,E.new_content Content
                                    ,E.new_imageurl ImageUrl
                                    ,E.new_giftcode AS GiftCode
                                    ,E.new_stock AS Stock
                                    ,E.new_portalid AS PortalId
                                    ,E.new_portalidName AS PortalIdName
                                    ,'new_portal' AS PortalIdTypeName
                                    ,E.new_giftcategoryid AS CategoryId
                                    ,E.new_giftcategoryidName AS CategoryIdName
                                    ,'new_giftcategory' AS CategoryIdTypeName
                                    ,E.new_point AS Point
                                FROM
                                new_gift AS E (NOLOCK)
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
                                        new_new_gift_new_role AS ERDF (NOLOCK)
                                            ON
                                            ERDF.new_giftid=E.new_giftId
                                            AND
                                            ERDF.new_roleid =RD.new_roleId
                                WHERE
                                    E.new_giftcategoryid='{2}' 
                                    AND
                                    E.new_portalid = '{0}'
                                    AND
                                    E.statuscode=1 --Active
                                ORDER BY E.new_point " + sortType;
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalId, portalUserId, categoryId));

                if (dt.Rows.Count > 0)
                {
                    List<Gift> giftList = dt.ToList<Gift>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = giftList;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "Hediye kaydı bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetGiftInfo(Guid giftId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"
                                SELECT DISTINCT
                                    E.new_giftId Id
                                    ,E.new_name Name
                                    ,E.new_summary Summary
                                    ,E.new_content Content
                                    ,E.new_imageurl ImageUrl
                                    ,E.new_giftcode AS GiftCode
                                    ,E.new_stock AS Stock
                                    ,E.new_portalid AS PortalId
                                    ,E.new_portalidName AS PortalIdName
                                    ,'new_portal' AS PortalIdTypeName
                                    ,E.new_giftcategoryid AS CategoryId
                                    ,E.new_giftcategoryidName AS CategoryIdName
                                    ,'new_giftcategory' AS CategoryIdTypeName
                                    ,E.new_point AS Point
                                FROM
                                new_gift AS E (NOLOCK)
                                WHERE
                                    E.new_giftId='{0}'";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, giftId));

                if (dt.Rows.Count > 0)
                {
                    List<Gift> giftList = dt.ToList<Gift>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = giftList[0];
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "Hediye kaydı bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetGiftRequestInfo(Guid giftRequestId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    pc.new_giftrequestId AS Id
	                                    ,pc.new_name AS Name
	                                    ,pc.new_potalid AS PortalId
	                                    ,pc.new_potalidName AS PortalIdName
	                                    ,'new_portal' AS PortalIdTypeName
                                        ,pc.new_userid AS UserId
	                                    ,pc.new_useridName AS UserIdName
	                                    ,'new_user' AS UserIdTypeName
                                        ,pc.new_giftid AS GiftId
	                                    ,pc.new_giftidName AS GiftIdName
	                                    ,'new_gift' AS GiftIdTypeName
	                                    ,pc.new_point AS Point
	                                    ,pc.statuscode AS Status
                                        ,pc.CreatedOn
                                    FROM
                                    new_giftrequest AS pc (NOLOCK)
                                    WHERE
                                    pc.new_giftrequestId='{0}'";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, giftRequestId));

                if (dt.Rows.Count > 0)
                {
                    List<UserGiftRequest> pCode = dt.ToList<UserGiftRequest>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = pCode[0];
                }
                else
                {
                    returnValue.Result = "Kayıt bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResult CreateUserGiftRequest(UserGiftRequest userGiftRequest, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = userGiftRequest.ToCrmEntity();

                returnValue.CrmId = service.Create(ent);
                returnValue.Success = true;
                returnValue.Result = userGiftRequest.Point.ToString() + " puana sahip hediye talebiniz alınmıştır.";
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResult UpdateGiftRequest(UserGiftRequest userGiftRequest, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = userGiftRequest.ToCrmEntity();

                service.Update(ent);
                returnValue.Success = true;
                returnValue.Result = "Hediye talebi güncellendi";
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResultObject GetGiftReuqestListByStatus(Guid portalId, GiftStatus giftStatus, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"
                                SELECT
                                   pc.new_giftrequestId AS Id
	                                ,pc.new_name AS Name
	                                ,pc.new_potalid AS PortalId
	                                ,pc.new_potalidName AS PortalIdName
	                                ,'new_portal' AS PortalIdTypeName
                                    ,pc.new_userid AS UserId
	                                ,pc.new_useridName AS UserIdName
	                                ,'new_user' AS UserIdTypeName
                                    ,pc.new_giftid AS GiftId
	                                ,pc.new_giftidName AS GiftIdName
	                                ,'new_gift' AS GiftIdTypeName
	                                ,pc.new_point AS Point
	                                ,pc.statuscode AS Status
                                    ,pc.CreatedOn
                                FROM
                                new_giftrequest AS pc (NOLOCK)
                                WHERE
                                    pc.new_potalid = '{0}'
                                    AND
                                    pc.statuscode={1} --Active";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalId, ((int)giftStatus)).ToString());

                if (dt.Rows.Count > 0)
                {
                    List<UserGiftRequest> giftList = dt.ToList<UserGiftRequest>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = giftList;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "Hediye kaydı bulunamadı!";
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
