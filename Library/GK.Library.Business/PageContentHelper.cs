using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GK.Library.Business
{
    public static class PageContentHelper
    {
        public static MsCrmResult GetPageContent(Guid portalId, PageNames pageName, SqlDataAccess sda)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    pc.new_content AS Content
                                    FROM
	                                    new_pagecontent AS pc (NOLOCK)
                                    WHERE
	                                    pc.new_portalId='{0}'
                                    AND
	                                    pc.new_page={1}
                                    AND
	                                    pc.statecode=0";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, portalId, ((int)pageName).ToString()));

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Content"] != DBNull.Value)
                    {
                        returnValue.Success = true;
                        returnValue.Result = dt.Rows[0]["Content"].ToString();
                    }
                }
                else
                {
                    returnValue.Result = "M051"; //"Sayfa içeriği hazırlanmamıştır.";
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
