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
    public static class AssemblyRequestHelper
    {
        public static MsCrmResultObj<List<AssemblerInfo>> GetAssemblerList(Guid portalId, Guid cityId, Guid townId, SqlDataAccess sda)
        {
            MsCrmResultObj<List<AssemblerInfo>> returnValue = new MsCrmResultObj<List<AssemblerInfo>>();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT 
	                                    DISTINCT
                                        u.new_userId AS Id
                                        ,u.new_name AS IdName
                                        ,'new_user' AS IdTypeName
                                        ,c.FirstName
                                        ,c.LastName
                                        ,c.JobTitle AS CompanyName
                                        ,c.MobilePhone AS MobilePhoneNumber
                                        ,c.Telephone1 AS WorkPhoneNumber
                                        ,c.new_cityId AS CityId
                                        ,c.new_cityIdName AS CityIdName
                                        ,'new_city' AS CityIdTypeName
                                        ,c.new_townId AS TownId
                                        ,c.new_townIdName AS TownIdName
                                        ,'new_town' AS TownIdTypeName
                                        ,c.new_addressdetail AS AddressDetail
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
                                    WHERE
                                        u.statecode=0
                                    AND
                                        u.statuscode=1 --Active
                                    AND
                                        c.new_markcontact=1
                                    AND
	                                    c.new_cityId='{1}'
                                    AND
	                                    c.new_townId='{2}'";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, portalId, cityId, townId));

                if (dt.Rows.Count > 0)
                {
                    List<AssemblerInfo> assemblerList = dt.ToList<AssemblerInfo>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = assemblerList;
                }
                else
                {
                    returnValue.Result = "Kayıt bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResultObj<List<AssemblerInfo>> GetAllAssemblerList(Guid portalId, SqlDataAccess sda)
        {
            MsCrmResultObj<List<AssemblerInfo>> returnValue = new MsCrmResultObj<List<AssemblerInfo>>();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT 
	                                    DISTINCT
                                        u.new_userId AS Id
                                        ,u.new_name AS IdName
                                        ,'new_user' AS IdTypeName
                                        ,c.FirstName
                                        ,c.LastName
                                        ,c.JobTitle AS CompanyName
                                        ,c.MobilePhone AS MobilePhoneNumber
                                        ,c.Telephone1 AS WorkPhoneNumber
                                        ,c.new_cityId AS CityId
                                        ,c.new_cityIdName AS CityIdName
                                        ,'new_city' AS CityIdTypeName
                                        ,c.new_townId AS TownId
                                        ,c.new_townIdName AS TownIdName
                                        ,'new_town' AS TownIdTypeName
                                        ,c.new_addressdetail AS AddressDetail
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
                                    WHERE
                                        u.statecode=0
                                    AND
                                        u.statuscode=1 --Active
                                    AND
                                        c.new_markcontact=1
                                    AND
	                                    c.new_cityId IS NOT NULL
                                    AND
	                                    c.new_townId IS NOT NULL";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, portalId));

                if (dt.Rows.Count > 0)
                {
                    List<AssemblerInfo> assemblerList = dt.ToList<AssemblerInfo>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = assemblerList;
                }
                else
                {
                    returnValue.Result = "Kayıt bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResultObj<AssemblerInfo> GetAssemblerInfo(Guid userId, SqlDataAccess sda)
        {
            MsCrmResultObj<AssemblerInfo> returnValue = new MsCrmResultObj<AssemblerInfo>();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT 
	                                    DISTINCT
                                        u.new_userId AS Id
                                        ,u.new_name AS IdName
                                        ,'new_user' AS IdTypeName
                                        ,c.FirstName
                                        ,c.LastName
                                        ,c.JobTitle AS CompanyName
                                        ,c.MobilePhone AS MobilePhoneNumber
                                        ,c.Telephone1 AS WorkPhoneNumber
                                        ,c.new_cityId AS CityId
                                        ,c.new_cityIdName AS CityIdName
                                        ,'new_city' AS CityIdTypeName
                                        ,c.new_townId AS TownId
                                        ,c.new_townIdName AS TownIdName
                                        ,'new_town' AS TownIdTypeName
                                        ,c.new_addressdetail AS AddressDetail
                                    FROM
                                    new_user AS u (NOLOCK)
                                        JOIN
                                            Contact AS c (NOLOCK)
                                                ON
                                                u.new_contactId=c.ContactId
                                    WHERE
	                                    u.new_userId='{0}'";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, userId));

                if (dt.Rows.Count > 0)
                {
                    List<AssemblerInfo> assemblerList = dt.ToList<AssemblerInfo>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = assemblerList[0];
                }
                else
                {
                    returnValue.Result = "Kayıt bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }


        public static MsCrmResultObj<List<AssemblyRequestInfo>> GetAssemblyRequestList(Guid userId, SqlDataAccess sda)
        {
            MsCrmResultObj<List<AssemblyRequestInfo>> returnValue = new MsCrmResultObj<List<AssemblyRequestInfo>>();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    req.new_assemblyrequestId AS Id
	                                    ,req.new_name AS Name
	                                    ,req.new_firstname AS FirstName
	                                    ,req.new_lastname AS LastName
	                                    ,req.new_mobilephone AS MobilePhoneNumber
	                                    ,req.new_workphone AS WorkPhoneNumber
	                                    ,req.new_emailaddress AS EmailAddress
	                                    ,req.new_cityid AS CityId
	                                    ,req.new_cityidName AS CityIdName
	                                    ,'new_city' AS CityIdTypeName
	                                    ,req.new_townid AS TownId
	                                    ,req.new_townidName AS TownIdName
	                                    ,'new_town' AS TownIdTypeName
	                                    ,req.new_addressdetail AS AddressDetail
	                                    ,req.new_userid AS AssemblerId
	                                    ,req.new_useridName AS AssemblerIdName
	                                    ,'new_user' AS AssemblerIdTypeName
	                                    ,req.statuscode AS StatusCode
	                                    ,req.CreatedOn
                                    FROM
                                    new_assemblyrequest AS req (NOLOCK)
                                    WHERE
	                                    req.new_userid='{0}'";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, userId));

                if (dt.Rows.Count > 0)
                {
                    List<AssemblyRequestInfo> assemblyRequestList = dt.ToList<AssemblyRequestInfo>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = assemblyRequestList;
                }
                else
                {
                    returnValue.Result = "Kayıt bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResultObj<AssemblyRequestInfo> GetAssemblyRequestInfo(Guid requestId, SqlDataAccess sda)
        {
            MsCrmResultObj<AssemblyRequestInfo> returnValue = new MsCrmResultObj<AssemblyRequestInfo>();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    req.new_assemblyrequestId AS Id
	                                    ,req.new_name AS Name
	                                    ,req.new_firstname AS FirstName
	                                    ,req.new_lastname AS LastName
	                                    ,req.new_mobilephone AS MobilePhoneNumber
	                                    ,req.new_workphone AS WorkPhoneNumber
	                                    ,req.new_emailaddress AS EmailAddress
	                                    ,req.new_cityid AS CityId
	                                    ,req.new_cityidName AS CityIdName
	                                    ,'new_city' AS CityIdTypeName
	                                    ,req.new_townid AS TownId
	                                    ,req.new_townidName AS TownIdName
	                                    ,'new_town' AS TownIdTypeName
	                                    ,req.new_addressdetail AS AddressDetail
	                                    ,req.new_userid AS AssemblerId
	                                    ,req.new_useridName AS AssemblerIdName
	                                    ,'new_user' AS AssemblerIdTypeName
	                                    ,req.statuscode AS StatusCode
	                                    ,req.CreatedOn
                                    FROM
                                    new_assemblyrequest AS req (NOLOCK)
                                    WHERE
	                                    req.new_assemblyrequestId='{0}'";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, requestId));

                if (dt.Rows.Count > 0)
                {
                    List<AssemblyRequestInfo> assemblyRequestList = dt.ToList<AssemblyRequestInfo>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = assemblyRequestList[0];
                }
                else
                {
                    returnValue.Result = "Kayıt bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResult CreateAssemblyRequest(AssemblyRequestInfo requestInfo, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = requestInfo.ToCrmEntity();

                returnValue.CrmId = service.Create(ent);
                returnValue.Success = true;
                returnValue.Result = "Talep kaydedildi.";
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResult UpdateAssemblyRequest(AssemblyRequestInfo requestInfo, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = requestInfo.ToCrmEntity();

                service.Update(ent);
                returnValue.Success = true;
                returnValue.Result = "Talep güncellendi.";
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResult AnswerNpsSurvey(Guid npsSurveyId, int suggest, int suggestPoint, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = new Entity("new_npssurvey");
                ent.Id = npsSurveyId;
                ent["new_issuggest"] = new OptionSetValue(suggest);
                ent["new_suggestpoint"] = suggestPoint;
                ent["statuscode"] = new OptionSetValue((int)NpsSurveyStatus.Answered);

                service.Update(ent);
                returnValue.Success = true;
                returnValue.Result = "Nps Survey güncellendi.";
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
