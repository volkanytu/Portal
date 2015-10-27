using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GK.Library.Business
{
    public static class PortalUserHelper
    {
        public static MsCrmResultObject GetPortalUserDetail(Guid portalId, Guid portalUserId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                U.new_userId UserId
                                    ,U.new_name AS Name
	                                ,U.new_contactId ContactId
	                                ,U.new_contactIdName ContactIdName
	                                ,U.new_iscontractapproved IsDisclaimer
	                                ,U.new_contactapprovedate DisclaimerDate
	                                --,U.new_first_login_date FirstLoginDate
	                                --,U.new_is_password_changed_ever IsPasswordChange
	                                --,U.new_password_changing_date PasswordChangeDate
	                                --,U.new_isuserlogined IsLoginEver
	                                ,U.new_iswelcome IsMessageGenerated
                                    ,U.new_imageurl [Image]
                                    ,U.new_languageId AS LanguageId
                                    ,U.new_languageIdName AS LanguageIdName

                                FROM
	                                new_user U (NoLock)
                                WHERE
	                                U.new_userId = '{0}'
	                                AND
	                                U.statecode = 0
	                                AND
	                                U.statuscode = {1}";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalUserId, (int)PortalUserStatus.Active));
                if (dt != null && dt.Rows.Count > 0)
                {
                    PortalUser user = new PortalUser();
                    user.PortalUserId = (Guid)dt.Rows[0]["UserId"];
                    user.UserName = dt.Rows[0]["Name"] != DBNull.Value ? dt.Rows[0]["Name"].ToString() : string.Empty; ;
                    user.IsDisclaimerApproved = dt.Rows[0]["IsDisclaimer"] != DBNull.Value ? (bool)dt.Rows[0]["IsDisclaimer"] : false;
                    if (dt.Rows[0]["DisclaimerDate"] != DBNull.Value) { user.DisclaimerApproveDate = (DateTime)dt.Rows[0]["DisclaimerDate"]; }
                    //if (dt.Rows[0]["FirstLoginDate"] != DBNull.Value) { user.FirstLoginDate = (DateTime)dt.Rows[0]["FirstLoginDate"]; }
                    //user.IsPasswordChangeEver = dt.Rows[0]["IsPasswordChange"] != DBNull.Value ? (bool)dt.Rows[0]["IsPasswordChange"] : false;
                    //if (dt.Rows[0]["PasswordChangeDate"] != DBNull.Value) { user.PasswordChangeDate = (DateTime)dt.Rows[0]["PasswordChangeDate"]; }
                    //user.IsUserLoginEver = dt.Rows[0]["IsLoginEver"] != DBNull.Value ? (bool)dt.Rows[0]["IsLoginEver"] : false;
                    user.IsWelcomeMessageGenerate = dt.Rows[0]["IsMessageGenerated"] != DBNull.Value ? (bool)dt.Rows[0]["IsMessageGenerated"] : false;
                    //user.IsWelcomeSmsGenerate = dt.Rows[0]["IsSmsGenerated"] != DBNull.Value ? (bool)dt.Rows[0]["IsSmsGenerated"] : false;
                    user.Image = dt.Rows[0]["Image"] != DBNull.Value ? dt.Rows[0]["Image"].ToString() : "nouserprofile.jpg"; ;

                    if (dt.Rows[0]["LanguageId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference()
                        {
                            Id = (Guid)dt.Rows[0]["LanguageId"],
                            Name = dt.Rows[0]["LanguageIdName"].ToString(),
                            LogicalName = "new_language"
                        };

                        user.Language = er;
                    }

                    returnValue.Success = true;


                    #region | GET USER ROLES |
                    MsCrmResultObject userRolesResult = GetPortalUserRoles(portalId, portalUserId, sda); // Portal kullanıcısına ait rolleri alınır.
                    if (userRolesResult.Success)
                    {
                        user.RoleList = (List<Role>)userRolesResult.ReturnObject;
                    }
                    else
                    {
                        returnValue.Success = userRolesResult.Success;
                        returnValue.Result = userRolesResult.Result;
                    }
                    #endregion

                    #region | GET CONTACT INFO |
                    if (returnValue.Success) // Kullanıcının rolü var ise
                    {
                        if (dt.Rows[0]["ContactId"] != DBNull.Value) // Boş olamaz
                        {
                            Guid contactId = (Guid)dt.Rows[0]["ContactId"];
                            MsCrmResultObject contactResult = ContactHelper.GetContactInfo(contactId, sda); // Portal kullanıcısının ait olduğu kullanıcının bilgileri alınır.

                            if (contactResult.Success)
                            {
                                user.ContactInfo = (Contact)contactResult.ReturnObject;
                            }
                            else
                            {
                                returnValue.Success = contactResult.Success;
                                returnValue.Result = contactResult.Result;
                            }
                        }
                        else
                        {
                            returnValue.Success = false;
                            returnValue.Result = "M007"; //"Portal kullanıcısına ait İlgili Kişi kaydı yoktur!";
                        }
                    }

                    #endregion

                    returnValue.ReturnObject = user;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M008"; //"Portal kullanıcısı bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResultObject GetPortalUserRoles(Guid portalId, Guid portalUserId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                R.new_name Name
	                                ,UR.new_roleid RoleId
                                FROM
	                                new_new_user_new_role UR (NoLock)
                                INNER JOIN
	                                new_role R (NoLock)
	                                ON
	                                UR.new_userid = '{0}'
	                                AND
									R.new_portalId = '{1}'
									AND
	                                R.new_roleId = UR.new_roleid
	                                AND
	                                R.statecode = 0
									AND
									R.statuscode=1 --Active";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalUserId, portalId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Role> returnList = new List<Role>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Role _role = new Role();
                        _role.RoleId = (Guid)dt.Rows[i]["RoleId"];
                        _role.Name = dt.Rows[i]["Name"] != DBNull.Value ? dt.Rows[i]["Name"].ToString() : string.Empty;
                        returnList.Add(_role);
                    }

                    returnValue.ReturnObject = returnList;
                    returnValue.Success = true;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "Kullancıya ait rol bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult SendContactPassword(string userName, SqlDataAccess sda)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                U.new_pass UserId
                                FROM
	                                new_user U (NoLock)
                                WHERE
	                                U.new_name = '{0}'
	                                AND
	                                C.StateCode = 0";
                #endregion
                DataTable dt = sda.getDataTable(string.Format(query, userName));

                if (dt != null && dt.Rows.Count > 0)
                {
                    string password = dt.Rows[0]["Password"].ToString();
                    if (!string.IsNullOrEmpty(password))
                    {
                        returnValue = GeneralHelper.SendMail();
                    }
                    else
                    {
                        returnValue.Success = false;
                        returnValue.Result = "Kullanıcının kayıtlı şifresi bulunmamaktadır!";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static MsCrmResult UpdateWelcomePageGenerated(Guid portalUserId, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                Entity ent = new Entity("new_user");
                ent["new_userid"] = portalUserId;
                ent["new_iswelcome"] = true;

                service.Update(ent);

                returnValue.Success = true;
                returnValue.Result = "Hoş geldin mesajı gösterildi.";
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult UpdateContractApprove(Guid portalUserId, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                Entity ent = new Entity("new_user");
                ent["new_userid"] = portalUserId;
                ent["new_iscontractapproved"] = true;
                ent["new_contactapprovedate"] = DateTime.Now;

                service.Update(ent);

                returnValue.Success = true;
                returnValue.Result = "Sözleşme okundu.";
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult UpdateUserPassword(Guid portalUserId, string newPassword, string oldPassword, IOrganizationService service, SqlDataAccess sda)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                if (sda != null)
                {
                    MsCrmResult oldPassRes = CheckOldPasswordCorrect(portalUserId, oldPassword, sda);

                    if (!oldPassRes.Success)
                    {
                        return oldPassRes;
                    }
                }

                Entity ent = new Entity("new_user");
                ent["new_userid"] = portalUserId;
                ent["new_password"] = newPassword.Trim();

                service.Update(ent);

                returnValue.Success = true;
                returnValue.Result = "M011"; //"Şifreniz güncellendi.";
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult CheckOldPasswordCorrect(Guid portalUserId, string oldPassword, SqlDataAccess sda)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                #region | SQL QUERY |
                string sqlQuery = @"SELECT
	                                    COUNT(0) AS RecCount
                                    FROM
		                                    new_user AS u (NOLOCK)
                                    WHERE
	                                    u.new_userId='{0}'
                                    AND
	                                    u.new_password='{1}'";
                #endregion

                int recCount = (int)sda.ExecuteScalar(string.Format(sqlQuery, portalUserId, oldPassword.Trim()));

                if (recCount > 0)
                {
                    returnValue.Success = true;
                    returnValue.Result = "Eski şifre ile bilgiler eşleşti.";
                }
                else
                {
                    returnValue.Result = "Eski şifre bilginiz yanlıştır.<br />Lütfen kontrol ediniz.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult CheckPhoneNumberMatch(string userName, string phoneNumber, string portalId, SqlDataAccess sda)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
                                        u.new_userId AS PortalUserId
                                        ,c.ContactId
                                        ,c.MobilePhone
                                    FROM
                                    new_user AS u (NOLOCK)
	                                    JOIN
		                                    new_new_user_new_role AS ur (NOLOCK)
			                                    ON
			                                    u.new_userId=ur.new_userid
	                                    JOIN
		                                    new_role AS r (NOLOCK)
			                                    ON
			                                    r.new_roleId=ur.new_roleid
			                                    AND
			                                    r.new_portalId='{2}'
	                                    JOIN
		                                    Contact AS c (NOLOCK)
			                                    ON
			                                    c.ContactId=u.new_contactId
                                    WHERE
                                    u.new_name='{0}'
                                    AND
                                    u.statecode=0
                                    AND
                                    u.statuscode=1 --Active
                                    AND
                                    c.MobilePhone='{1}'";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, userName, phoneNumber, portalId));

                if (dt.Rows.Count > 0)
                {
                    returnValue.Success = true;
                    returnValue.Result = dt.Rows[0]["ContactId"].ToString();
                    returnValue.CrmId = (Guid)dt.Rows[0]["PortalUserId"];
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
