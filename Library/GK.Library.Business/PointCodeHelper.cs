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
    public static class PointCodeHelper
    {
        public static MsCrmResultObject GetPointCodeInfo(Guid pointCodeId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    pc.new_pointcodeId AS Id
	                                    ,pc.new_name AS Name
	                                    ,pc.new_portalid AS PortalId
	                                    ,pc.new_portalidName AS PortalIdName
	                                    ,'new_portal' AS PortalIdTypeName
	                                    ,pc.new_group AS GroupCode
	                                    ,pc.new_code AS Code
	                                    ,pc.new_point AS Point
	                                    ,pc.statuscode AS Status
                                    FROM
                                    new_pointcode AS pc (NOLOCK)
                                    WHERE
                                    pc.new_pointcodeId='{0}'";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, pointCodeId));

                if (dt.Rows.Count > 0)
                {
                    List<PointCode> pCode = dt.ToList<PointCode>();

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

        public static MsCrmResultObject GetPointCodeInfo(string code, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    pc.new_pointcodeId AS Id
	                                    ,pc.new_name AS Name
	                                    ,pc.new_portalid AS PortalId
	                                    ,pc.new_portalidName AS PortalIdName
	                                    ,'new_portal' AS PortalIdTypeName
	                                    ,pc.new_group AS GroupCode
	                                    ,pc.new_code AS Code
	                                    ,pc.new_point AS Point
	                                    ,pc.statuscode AS Status
                                    FROM
                                    new_pointcode AS pc (NOLOCK)
                                    WHERE
                                    pc.new_code='{0}'";

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

        public static MsCrmResult PassiveCode(Guid pointCodeId, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                SetStateRequest setStateReq = new SetStateRequest();
                setStateReq.EntityMoniker = new EntityReference("new_pointcode", pointCodeId);
                setStateReq.State = new OptionSetValue(1);
                setStateReq.Status = new OptionSetValue(2);

                SetStateResponse response = (SetStateResponse)service.Execute(setStateReq);

                returnValue.Success = true;
                returnValue.Result = "Kod kullanıldı";
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult CreateUserCodeUsage(UserCodeUsage userCodeUsage, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = userCodeUsage.ToCrmEntity();

                returnValue.CrmId = service.Create(ent);
                returnValue.Success = true;
                returnValue.Result = userCodeUsage.Point.ToString() + " puan kazandınız.";
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }
    }
}
