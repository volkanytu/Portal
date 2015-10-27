using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;


namespace GK.Library.Business
{
    public static class LoginHelper
    {
        public static MsCrmResult LoginControl(Guid portalId, string userName, string password, SqlDataAccess sda)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                U.new_userId UserId
                                FROM
	                                new_user U (NoLock)
                                WHERE
	                                U.new_name = '{0}'
	                                AND
	                                U.new_password = '{1}'
	                                AND
	                                U.statecode = 0
	                                AND
	                                U.statuscode = {2}";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, userName, password, (int)PortalUserStatus.Active));
                if (dt != null && dt.Rows.Count > 0)
                {
                    returnValue.CrmId = (Guid)dt.Rows[0]["UserId"];

                    MsCrmResultObject roleResult = PortalUserHelper.GetPortalUserRoles(portalId, returnValue.CrmId, sda);
                    returnValue.Success = roleResult.Success;
                    returnValue.Result = roleResult.Result;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M035"; //"Hatalı kullanıcı adı veya şifre!";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static MsCrmResult LogLogIn(Guid portalUserId, Guid portalId, DateTime loginDate, string ipAddress, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                Entity ent = new Entity("new_loginlog");
                ent["new_name"] = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                ent["new_logindate"] = loginDate;
                ent["new_portalid"] = new EntityReference("new_portal", portalId);
                ent["new_userid"] = new EntityReference("new_user", portalUserId);
                ent["new_ipaddress"] = ipAddress;

                returnValue.CrmId = service.Create(ent);
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult LogLogOut(Guid loginLogId, DateTime logoutDate, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                Entity ent = new Entity("new_loginlog");
                ent["new_loginlogid"] = loginLogId;
                ent["new_logoutdate"] = logoutDate;

                service.Update(ent);
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static int GetUserLoginCount(Guid portalId, Guid portalUserId, DateTime start, DateTime end, SqlDataAccess sda)
        {
            int returnValue = 0;

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                COUNT(0)
                                FROM
	                                new_loginlog AS ll (NOLOCK)
                                WHERE
	                                ll.new_portalId=@PortalId
                                AND
	                                ll.new_userId=@UserId
                                AND
	                                ll.new_logindate BETWEEN @start AND @end
                                AND
                                    ll.StateCode=0";

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
