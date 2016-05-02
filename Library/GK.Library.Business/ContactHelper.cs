using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace GK.Library.Business
{
    public static class ContactHelper
    {
        public static MsCrmResult CreateOrUpdateProfile(Contact contact, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                Entity ent = new Entity("contact");

                if (!string.IsNullOrEmpty(contact.MobilePhone))
                {
                    ent["mobilephone"] = contact.MobilePhone;
                }

                if (!string.IsNullOrEmpty(contact.WorkPhone))
                {
                    ent["telephone1"] = contact.WorkPhone;
                }

                if (!string.IsNullOrEmpty(contact.IdentityNumber))
                {
                    ent["new_identitynumber"] = contact.IdentityNumber;
                }

                if (contact.Gender != null)
                {
                    ent["gendercode"] = new OptionSetValue((int)contact.Gender);
                }

                //if (contact.BirthDate != null)
                //{
                ent["birthdate"] = contact.BirthDate;
                //}

                if (!string.IsNullOrEmpty(contact.Description))
                {
                    ent["description"] = contact.Description;
                }

                if (!string.IsNullOrEmpty(contact.EmailAddress))
                {
                    ent["emailaddress1"] = contact.EmailAddress;
                }

                if (!string.IsNullOrEmpty(contact.FirstName))
                {
                    ent["firstname"] = contact.FirstName;
                }

                if (!string.IsNullOrEmpty(contact.LastName))
                {
                    ent["lastname"] = contact.LastName;
                }

                if (!string.IsNullOrEmpty(contact.Title))
                {
                    ent["jobtitle"] = contact.Title;
                }
                if (!string.IsNullOrEmpty(contact.FunctionName))
                {
                    ent["new_functionname"] = contact.FunctionName;
                }

                if (contact.ParentAccount != null)
                {
                    ent["parentaccountid"] = contact.ParentAccount;
                }

                if (contact.CityId != null)
                {
                    ent["new_cityid"] = contact.CityId;
                }

                if (contact.TownId != null)
                {
                    ent["new_townid"] = contact.TownId;
                }

                if (!string.IsNullOrEmpty(contact.AddressDetail))
                {
                    ent["new_addressdetail"] = contact.AddressDetail;
                }

                if (contact.MarkContact != null)
                {
                    ent["new_markcontact"] = contact.MarkContact;
                }

                if (contact.ContactId != Guid.Empty)
                {
                    ent["contactid"] = contact.ContactId;

                    service.Update(ent);
                    returnValue.Success = true;
                    returnValue.Result = "M009"; //"Profil güncellendi.";
                }
                else
                {
                    returnValue.CrmId = service.Create(ent);
                    returnValue.Success = true;
                    returnValue.Result = "M010"; //"Profil oluşturuldu.";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetContactInfo(Guid contactId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
                                     C.ContactId
                                    ,C.FirstName
                                    ,C.LastName
                                    ,C.JobTitle
                                    ,C.new_functionname FunctionName
                                    ,C.AccountId
                                    ,C.AccountIdName
                                    ,C.EMailAddress1 EmailAddress
                                    ,C.MobilePhone
                                    ,C.Telephone1
                                    ,C.new_identitynumber AS IdentityNo
                                    ,C.GenderCode
                                    ,DATEADD(HH,3,C.BirthDate) AS BirthDate
                                    ,C.Description
                                    ,C.new_cityId AS CityId
                                    ,C.new_cityIdName AS CityIdName
                                    ,C.new_townId AS TownId
                                    ,C.new_townIdName AS TownIdName
                                    ,C.new_addressdetail AS AddressDetail
                                    ,C.new_markcontact AS MarkContact
                                FROM
	                                Contact C (NoLock)
                                WHERE
	                                C.ContactId = '{0}'
	                                AND
	                                C.StateCode = 0";
                #endregion
                DataTable dt = sda.getDataTable(string.Format(query, contactId));
                if (dt != null && dt.Rows.Count > 0)
                {
                    Contact _contact = new Contact();
                    _contact.ContactId = (Guid)dt.Rows[0]["ContactId"];
                    _contact.FirstName = dt.Rows[0]["FirstName"] != DBNull.Value ? dt.Rows[0]["FirstName"].ToString() : string.Empty;
                    _contact.LastName = dt.Rows[0]["LastName"] != DBNull.Value ? dt.Rows[0]["LastName"].ToString() : string.Empty;
                    _contact.Title = dt.Rows[0]["JobTitle"] != DBNull.Value ? dt.Rows[0]["JobTitle"].ToString() : string.Empty;
                    _contact.FunctionName = dt.Rows[0]["FunctionName"] != DBNull.Value ? dt.Rows[0]["FunctionName"].ToString() : string.Empty;
                    _contact.EmailAddress = dt.Rows[0]["EmailAddress"] != DBNull.Value ? dt.Rows[0]["EmailAddress"].ToString() : string.Empty;
                    _contact.MobilePhone = dt.Rows[0]["MobilePhone"] != DBNull.Value ? dt.Rows[0]["MobilePhone"].ToString() : string.Empty;
                    _contact.WorkPhone = dt.Rows[0]["Telephone1"] != DBNull.Value ? dt.Rows[0]["Telephone1"].ToString() : string.Empty;
                    _contact.IdentityNumber = dt.Rows[0]["IdentityNo"] != DBNull.Value ? dt.Rows[0]["IdentityNo"].ToString() : string.Empty;
                    if (dt.Rows[0]["GenderCode"] != DBNull.Value) { _contact.Gender = (int)dt.Rows[0]["GenderCode"]; }
                    if (dt.Rows[0]["BirthDate"] != DBNull.Value) { _contact.BirthDate = ((DateTime)dt.Rows[0]["BirthDate"]).ToLocalTime(); }
                    _contact.Description = dt.Rows[0]["Description"] != DBNull.Value ? dt.Rows[0]["Description"].ToString() : string.Empty;

                    _contact.MarkContact = dt.Rows[0]["MarkContact"] != DBNull.Value ? (bool)dt.Rows[0]["MarkContact"] : false;

                    if (dt.Rows[0]["AccountId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference();
                        er.LogicalName = "account";
                        er.Id = (Guid)dt.Rows[0]["AccountId"];
                        if (dt.Rows[0]["AccountIdName"] != DBNull.Value) { er.Name = dt.Rows[0]["AccountIdName"].ToString(); }

                        _contact.ParentAccount = er;
                    }

                    if (dt.Rows[0]["CityId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference();
                        er.LogicalName = "new_city";
                        er.Id = (Guid)dt.Rows[0]["CityId"];
                        if (dt.Rows[0]["CityIdName"] != DBNull.Value) { er.Name = dt.Rows[0]["CityIdName"].ToString(); }

                        _contact.CityId = er;
                    }

                    if (dt.Rows[0]["TownId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference();
                        er.LogicalName = "new_town";
                        er.Id = (Guid)dt.Rows[0]["TownId"];
                        if (dt.Rows[0]["TownIdName"] != DBNull.Value) { er.Name = dt.Rows[0]["TownIdName"].ToString(); }

                        _contact.TownId = er;
                    }

                    if (dt.Rows[0]["AccountId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference();
                        er.LogicalName = "account";
                        er.Id = (Guid)dt.Rows[0]["AccountId"];
                        if (dt.Rows[0]["AccountIdName"] != DBNull.Value) { er.Name = dt.Rows[0]["AccountIdName"].ToString(); }

                        _contact.ParentAccount = er;
                    }

                    _contact.AddressDetail = dt.Rows[0]["AddressDetail"] != DBNull.Value ? dt.Rows[0]["AddressDetail"].ToString() : string.Empty;

                    returnValue.ReturnObject = _contact;
                    returnValue.Success = true;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M007"; //"No contact info found!";
                }

            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject SearchContact(Guid portalId, Guid userId, string key, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            try
            {
                #region | SQL QUERY |
                string query = @"SELECT DISTINCT
                                        u.new_userId AS UserId
                                        ,u.new_name AS UserName
                                        ,u.new_imageurl AS ImageUrl
                                        ,c.FullName
                                        ,c.JobTitle
                                        ,CASE WHEN fr.new_friendshipId IS NOT NULL THEN 1 ELSE 0 END AS IsFriend
                                    FROM
                                    new_user AS u (NOLOCK)
                                        JOIN
                                            new_new_user_new_role AS ur (NOLOCK)
                                                ON
                                                ur.new_userid=u.new_userId
                                        JOIN
                                            new_role AS r (NOLOCK)
                                                ON
                                                ur.new_roleid=r.new_roleId
                                                AND
                                                r.statecode=0
                                                AND
                                                r.statuscode=1 --Active
                                                AND
                                                r.new_portalId='{0}'
                                        JOIN
                                            Contact AS c (NOLOCK)
                                                ON
                                                u.new_contactId=c.ContactId
                                        LEFT JOIN
		                                    new_friendship AS fr (NOLOCK)
			                                    ON
			                                    '{2}' IN (fr.new_partyOneId,fr.new_partyTwoId)
			                                    AND
			                                    u.new_userId IN (fr.new_partyOneId,fr.new_partyTwoId)
			                                    AND
			                                    fr.statecode=0
                                    WHERE
                                        u.statecode=0
                                    AND
                                        u.new_userId!='{2}'
                                    AND
                                        u.statuscode=1 --Active
                                    AND
                                        c.FullName LIKE '%{1}%'";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalId, key, userId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<UserFriends> lstUser = new List<UserFriends>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        UserFriends uf = new UserFriends()
                        {
                            UserId = (Guid)dt.Rows[i]["UserId"],
                            UserName = dt.Rows[i]["UserName"].ToString(),
                            FullName = dt.Rows[i]["FullName"].ToString(),
                            ImageUrl = dt.Rows[i]["ImageUrl"] != DBNull.Value ? dt.Rows[i]["ImageUrl"].ToString() : "nouserprofile.jpg",
                            JobTitle = dt.Rows[i]["JobTitle"] != DBNull.Value ? dt.Rows[i]["JobTitle"].ToString() : "---",
                            UserType = (int)dt.Rows[i]["IsFriend"]
                        };

                        lstUser.Add(uf);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = lstUser;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M057"; //"Herhangi bir kayıt bulunamadı!";
                }

            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static string GetCityCode(Guid cityId, SqlDataAccess sda)
        {
            string returnValue = string.Empty;

            #region | SQL QUERY |

            string sqlQuery = @"SELECT c.new_citycode AS Code FROM new_city AS c (NOLOCK) WHERE c.new_cityId='{0}'";
            #endregion

            DataTable dt = sda.getDataTable(string.Format(sqlQuery, cityId));

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Code"] != DBNull.Value)
                {
                    returnValue = dt.Rows[0]["Code"].ToString();
                }
            }

            return returnValue;
        }
    }
}
