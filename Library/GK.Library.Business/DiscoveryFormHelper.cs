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
    public static class DiscoveryFormHelper
    {
        public static MsCrmResultObject GetDiscoveryFormInfo(Guid discoveryFormId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    pc.new_discoveryformId AS Id
	                                    ,pc.new_name AS Name
	                                    ,pc.new_firstname AS FirstName
	                                    ,pc.new_lastname AS LastName
	                                    ,pc.new_phonenumber AS PhoneNumber
	                                    ,pc.new_email AS Email
	                                    ,pc.new_cityid AS CityId
	                                    ,pc.new_cityidName AS CityIdName
	                                    ,'new_city' AS CityIdTypeName
	                                    ,pc.new_townid AS TownId
	                                    ,pc.new_townidName AS TownIdName
	                                    ,'new_town' AS TownIdTypeName
	                                    ,pc.new_userid AS UserId
	                                    ,pc.new_useridName AS UserIdName
	                                    ,'new_user' AS UserIdTypeName
	                                    ,pc.new_portalid AS PortalId
	                                    ,pc.new_portalidName AS PortalIdName
	                                    ,'new_portal' AS PortalIdTypeName
	                                    ,pc.new_hometype AS HomeType
	                                    ,pc.new_informedby AS InformedBy
	                                    ,pc.new_visithour AS VisitHour
	                                    ,pc.new_visitdate AS VisitDate
	                                    ,pc.new_formcode AS FormCode
	                                    ,pc.statuscode AS Status
	                                    ,pc.CreatedOn AS CreatedOn
                                    FROM
                                    new_discoveryform AS pc (NOLOCK)
                                    WHERE
                                    pc.new_discoveryformid='{0}'";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, discoveryFormId));

                if (dt.Rows.Count > 0)
                {
                    List<DiscoveryForm> discoveryForm = dt.ToList<DiscoveryForm>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = discoveryForm[0];
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

        public static MsCrmResultObject GetDiscoveryFormInfo(int code, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    pc.new_discoveryformId AS Id
	                                    ,pc.new_name AS Name
	                                    ,pc.new_firstname AS FirstName
	                                    ,pc.new_lastname AS LastName
	                                    ,pc.new_phonenumber AS PhoneNumber
	                                    ,pc.new_email AS Email
	                                    ,pc.new_cityid AS CityId
	                                    ,pc.new_cityidName AS CityIdName
	                                    ,'new_city' AS CityIdTypeName
	                                    ,pc.new_townid AS TownId
	                                    ,pc.new_townidName AS TownIdName
	                                    ,'new_town' AS TownIdTypeName
	                                    ,pc.new_userid AS UserId
	                                    ,pc.new_useridName AS UserIdName
	                                    ,'new_user' AS UserIdTypeName
	                                    ,pc.new_portalid AS PortalId
	                                    ,pc.new_portalidName AS PortalIdName
	                                    ,'new_portal' AS PortalIdTypeName
	                                    ,pc.new_hometype AS HomeType
	                                    ,pc.new_informedby AS InformedBy
	                                    ,pc.new_visithour AS VisitHour
	                                    ,pc.new_visitdate AS VisitDate
	                                    ,pc.new_formcode AS FormCode
	                                    ,pc.statuscode AS Status
	                                    ,pc.CreatedOn AS CreatedOn
                                    FROM
                                    new_discoveryform AS pc (NOLOCK)
                                    WHERE
                                    pc.new_formcode={0}";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, code));

                if (dt.Rows.Count > 0)
                {
                    List<PointCode> pCode = dt.ToList<PointCode>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = pCode[0];
                }
                else
                {
                    returnValue.Result = "Kod bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResult CreateDiscoveryForm(DiscoveryForm discoveryForm, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = discoveryForm.ToCrmEntity();

                returnValue.CrmId = service.Create(ent);
                returnValue.Success = true;
                returnValue.Result = "Form kaydedildi.";
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResult UpdateDiscoveryForm(DiscoveryForm discoveryForm, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = discoveryForm.ToCrmEntity();

                service.Update(ent);
                returnValue.Success = true;
                returnValue.Result = "Form kaydedildi.";
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResultObject GetGiftReuqestListByStatus(DiscoveryFormStatus discoveryFormStatus, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"DECLARE @objectTypeCode INT

                                    SELECT
                                    TOP 1
                                    @objectTypeCode=e.ObjectTypeCode
                                    FROM
                                    Entity AS e (NOLOCK)
                                    WHERE
                                    e.Name='new_discoveryform'

                                    SELECT
                                        pc.new_discoveryformId AS Id
                                        ,pc.new_name AS Name
                                        ,pc.new_firstname AS FirstName
                                        ,pc.new_lastname AS LastName
                                        ,pc.new_phonenumber AS PhoneNumber
                                        ,pc.new_email AS Email
                                        ,pc.new_cityid AS CityId
                                        ,pc.new_cityidName AS CityIdName
                                        ,'new_city' AS CityIdTypeName
                                        ,pc.new_townid AS TownId
                                        ,pc.new_townidName AS TownIdName
                                        ,'new_town' AS TownIdTypeName
                                        ,pc.new_userid AS userId
                                        ,pc.new_useridName AS userIdName
                                        ,'new_user' AS UserIdTypeName
                                        --,pc.new_hometype AS HomeType
                                        --,smHomeType.Value AS HomeTypeValue
                                        --,pc.new_informedby AS InformedBy
                                        --,smInformedBy.Value AS InformedByValue
                                        --,pc.new_visithour AS VisitHour
                                        --,smVisitHour.Value AS VisitHourValue
                                        ,pc.new_visitdate AS VisitDate
                                        ,pc.new_formcode AS FormCode
                                        ,pc.statuscode AS Status
                                        ,pc.CreatedOn AS CreatedOn
                                    FROM
                                    new_discoveryform AS pc (NOLOCK)
--	                                    JOIN
--		                                    StringMap AS smHomeType (NOLOCK)
--			                                    ON
--			                                    smHomeType.ObjectTypeCode=@objectTypeCode
--			                                    AND
--			                                    smHomeType.AttributeName='new_hometype'
--			                                    AND
--			                                    smHomeType.AttributeValue=pc.new_hometype
--	                                    JOIN
--		                                    StringMap AS smInformedBy (NOLOCK)
--			                                    ON
--			                                    smInformedBy.ObjectTypeCode=@objectTypeCode
--			                                    AND
--			                                    smInformedBy.AttributeName='new_informedby'
--			                                    AND
--			                                    smInformedBy.AttributeValue=pc.new_informedby
--	                                    JOIN
--		                                    StringMap AS smVisitHour (NOLOCK)
--			                                    ON
--			                                    smVisitHour.ObjectTypeCode=@objectTypeCode
--			                                    AND
--			                                    smVisitHour.AttributeName='new_visithour'
--			                                    AND
--			                                    smVisitHour.AttributeValue=pc.new_visithour
                                    WHERE
                                    pc.statuscode={0}";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, ((int)discoveryFormStatus)).ToString());

                if (dt.Rows.Count > 0)
                {
                    List<DiscoveryForm> giftList = dt.ToList<DiscoveryForm>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = giftList;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "Keşif form kaydı bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetMaxFormCodeNumber(SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
                                    ISNULL(MAX(df.new_formcode),0) AS MaxCode
                                FROM
                                new_discoveryform AS df (NOLOCK)";
                #endregion

                int maxNo = (int)sda.ExecuteScalar(sqlQuery);

                returnValue.Success = true;
                returnValue.ReturnObject = maxNo;

            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }
    }
}
