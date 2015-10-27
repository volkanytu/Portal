using GK.Library.Utility;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace GK.Library.Business
{
    public static class PortalHelper
    {
        public static MsCrmResult GetPortalId(string url, SqlDataAccess sda)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                u.new_portalId AS BrandId
                                FROM
	                                new_portalurl AS u (NOLOCK)
		                                JOIN
			                                new_portal AS b (NOLOCK)
				                                ON
				                                b.new_portalId=u.new_portalId
				                                AND
				                                b.statecode=0
				                                AND
				                                b.statuscode=1 --Active
                                WHERE
	                                u.new_name='{0}'
                                AND
	                                u.statecode=0";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, url));
                if (dt != null && dt.Rows.Count > 0)
                {
                    returnValue.CrmId = (Guid)dt.Rows[0]["BrandId"];
                    returnValue.Success = true;
                }
                else
                {
                    returnValue.Result = "M004"; //"Girmiş olduğunuz adres herhangi bir portala ait değil.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetPortalInfo(Guid portalId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                b.new_portalId AS Id
	                                ,b.new_name AS Name
	                                --,b.new_imageurl AS [Image]
	                                ,b.new_languageId AS LanguageId
	                                ,b.new_languageIdName AS LanguageIdName
	                                ,b.statuscode AS StatusCode
                                FROM
	                                new_portal AS b (NOLOCK)
                                WHERE
	                                b.new_portalId='{0}'";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalId));
                if (dt != null && dt.Rows.Count > 0)
                {
                    PortalInfo _portal = new PortalInfo();
                    _portal.Id = (Guid)dt.Rows[0]["Id"];
                    _portal.Name = dt.Rows[0]["Name"].ToString();
                    _portal.StatusCode = (int)dt.Rows[0]["StatusCode"];
                    //_portal.ImagePath = dt.Rows[0]["Image"] != DBNull.Value ? dt.Rows[0]["Image"].ToString() : "no_portallogo.png";

                    if (dt.Rows[0]["LanguageId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference()
                        {
                            Id = (Guid)dt.Rows[0]["LanguageId"],
                            Name = dt.Rows[0]["LanguageIdName"].ToString()
                        };

                        _portal.Language = er;
                    }

                    MsCrmResult portalLogo = PortalHelper.GetPortalLogoAnnotationId(_portal.Id, sda);
                    if (portalLogo.Success)
                    {
                        _portal.LogoImage = AnnotationHelper.GetAnnotationDetail(portalLogo.CrmId, sda);
                    }

                    MsCrmResult portalLoginImage = PortalHelper.GetPortalLoginAnnotationId(_portal.Id, sda);
                    if (portalLoginImage.Success)
                    {
                        _portal.PortalLoginImage = AnnotationHelper.GetAnnotationDetail(portalLoginImage.CrmId, sda);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = _portal;
                }
                else
                {
                    returnValue.Success = true;
                    returnValue.Result = "M005"; //"Portal bilgisi bulunamadı!";
                }

            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult GetPortalLogoAnnotationId(Guid portalId, SqlDataAccess sda)
        {
            MsCrmResult returnValue = new MsCrmResult();

            #region | SQL QUERY |
            string sqlQuery = @"SELECT
	                                a.AnnotationId AS Id
                                FROM
	                                Annotation AS a (NOLOCK)
                                WHERE
									a.ObjectId='{0}'
								AND
	                                CHARINDEX(a.Subject,'logo')=1";
            #endregion

            DataTable dt = sda.getDataTable(string.Format(sqlQuery, portalId));

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Id"] != DBNull.Value)
                {
                    returnValue.Success = true;
                    returnValue.CrmId = (Guid)dt.Rows[0]["Id"];
                }
            }

            return returnValue;
        }

        public static MsCrmResult GetPortalLoginAnnotationId(Guid portalId, SqlDataAccess sda)
        {
            MsCrmResult returnValue = new MsCrmResult();

            #region | SQL QUERY |
            string sqlQuery = @"SELECT
	                                a.AnnotationId AS Id
                                FROM
	                                Annotation AS a (NOLOCK)
                                WHERE
									a.ObjectId='{0}'
								AND
	                                CHARINDEX(a.Subject,'login')=1 ORDER BY a.CreatedOn DESC";
            #endregion

            DataTable dt = sda.getDataTable(string.Format(sqlQuery, portalId));

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Id"] != DBNull.Value)
                {
                    returnValue.Success = true;
                    returnValue.CrmId = (Guid)dt.Rows[0]["Id"];
                }
            }

            return returnValue;
        }

        public static List<Guid> GetPortalList(SqlDataAccess sda)
        {
            List<Guid> returnValue = new List<Guid>();

            #region | SQL QUERY |

            string sqlQuery = @"SELECT p.new_portalId AS Id FROM new_portal AS p (NOLOCK) WHERE p.StateCode=0 AND p.StatusCode=1";

            #endregion

            DataTable dt = sda.getDataTable(sqlQuery);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    returnValue.Add((Guid)dt.Rows[i]["Id"]);
                }
            }

            return returnValue;
        }
    }
}
