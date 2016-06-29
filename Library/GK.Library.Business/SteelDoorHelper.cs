using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GK.Library.Business
{
    public class SteelDoorHelper
    {
        public static MsCrmResult Insert(SteelDoor entity, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = entity.ToCrmEntity();

                returnValue.CrmId = service.Create(ent);
                returnValue.Success = true;
                returnValue.Result = "Çelik kapı talebi başarı ile oluşturuldu.";
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResult Update(SteelDoor entity, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = entity.ToCrmEntity();

                service.Update(ent);
                returnValue.Success = true;
                returnValue.Result = "Çelik kapı talebi başarı ile güncellendi.";
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static SteelDoor GetSteelDoor(Guid id, SqlDataAccess sda)
        {
            #region | SQL QUERY |

            string sqlQuery = @"SELECT
	                                    pc.new_steeldoorid AS Id
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
	                                    ,pc.statuscode AS Status
	                                    ,pc.CreatedOn AS CreatedOn
                                    FROM
                                    new_steeldoor AS pc (NOLOCK)
                                    WHERE
                                    pc.new_steeldoorid={0}";

            #endregion

            DataTable dt = sda.getDataTable(string.Format(sqlQuery, id));

            return dt.ToList<SteelDoor>().FirstOrDefault();
        }

        public static MsCrmResultObj<List<SteelDoor>> GetUserSteelDoors(Guid userId, SqlDataAccess sda)
        {
            MsCrmResultObj<List<SteelDoor>> returnValue = new MsCrmResultObj<List<SteelDoor>>();
            #region | SQL QUERY |

            string sqlQuery = @"SELECT DISTINCT
	                                    pc.new_steeldoorId AS Id
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
	                                    ,pc.statuscode AS Status
	                                    ,sm.Value AS StatusValue
	                                    ,pc.CreatedOn AS CreatedOn
                                    FROM
                                    new_steeldoor AS pc (NOLOCK)
JOIN
    Entity AS e (NOLOCK)
        ON
        e.Name='new_steeldoor'
JOIN
    StringMap AS sm (NOLOCK)
        ON
        sm.ObjectTypeCode=e.ObjectTypeCode
        AND
        sm.AttributeName='statuscode'
        AND
        sm.AttributeValue=pc.statuscode
                                    WHERE
                                    pc.new_userid='{0}'";

            #endregion

            DataTable dt = sda.getDataTable(string.Format(sqlQuery, userId));

            if (dt.Rows.Count > 0)
            {
                List<SteelDoor> steelDoors = dt.ToList<SteelDoor>();

                returnValue.Success = true;
                returnValue.ReturnObject = steelDoors;
            }
            else
            {
                returnValue.Result = "Kayıt bulunamadı.";
            }

            return returnValue;
        }
    }
}