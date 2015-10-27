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
    public static class LocationHelper
    {
        public static MsCrmResultObj<City> GetCityInfo(Guid cityId, SqlDataAccess sda)
        {
            MsCrmResultObj<City> returnValue = new MsCrmResultObj<City>();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    c.new_cityId AS Id
	                                    ,c.new_name AS Name
	                                    ,c.new_citycode AS Code
                                    FROM
                                    new_city AS c (NOLOCK)
                                    WHERE
                                    c.new_cityId='{0}'";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, cityId));

                if (dt.Rows.Count > 0)
                {
                    List<City> cityList = dt.ToList<City>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = cityList[0];
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

        public static MsCrmResultObj<City> GetCityInfo(string cityCode, SqlDataAccess sda)
        {
            MsCrmResultObj<City> returnValue = new MsCrmResultObj<City>();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    c.new_cityId AS Id
	                                    ,c.new_name AS Name
	                                    ,c.new_citycode AS Code
                                    FROM
                                    new_city AS c (NOLOCK)
                                    WHERE
                                    c.new_citycode='{0}'";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, cityCode));

                if (dt.Rows.Count > 0)
                {
                    List<City> cityList = dt.ToList<City>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = cityList[0];
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

        public static MsCrmResultObj<Town> GetTownInfo(Guid townId, SqlDataAccess sda)
        {
            MsCrmResultObj<Town> returnValue = new MsCrmResultObj<Town>();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    t.new_townId AS Id
	                                    ,t.new_name AS Name
	                                    ,t.new_towncode AS Code
	                                    ,t.new_cityId AS CityId
	                                    ,t.new_cityIdName AS CityIdName
	                                    ,'new_city' AS CityIdTypeName
                                    FROM
                                    new_town AS t (NOLOCK)
                                    WHERE
                                    t.new_townId='{0}'";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, townId));

                if (dt.Rows.Count > 0)
                {
                    List<Town> townList = dt.ToList<Town>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = townList[0];
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

        public static MsCrmResultObj<Town> GetTownInfo(string townCode, SqlDataAccess sda)
        {
            MsCrmResultObj<Town> returnValue = new MsCrmResultObj<Town>();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    t.new_townId AS Id
	                                    ,t.new_name AS Name
	                                    ,t.new_towncode AS Code
	                                    ,t.new_cityId AS CityId
	                                    ,t.new_cityIdName AS CityIdName
	                                    ,'new_city' AS CityIdTypeName
                                    FROM
                                    new_town AS t (NOLOCK)
                                    WHERE
                                    t.new_towncode='{0}'";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, townCode));

                if (dt.Rows.Count > 0)
                {
                    List<Town> townList = dt.ToList<Town>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = townList[0];
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

        public static MsCrmResultObj<List<City>> GetCities(SqlDataAccess sda)
        {
            MsCrmResultObj<List<City>> returnValue = new MsCrmResultObj<List<City>>();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    c.new_cityId AS Id
	                                    ,c.new_name AS Name
	                                    ,c.new_citycode AS Code
                                    FROM
                                    new_city AS c (NOLOCK)
                                    WHERE
                                    c.new_countryId='F55A5B6D-B32E-E511-80C4-000D3A216510'";

                #endregion

                DataTable dt = sda.getDataTable(sqlQuery);

                if (dt.Rows.Count > 0)
                {
                    List<City> cityList = dt.ToList<City>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = cityList;
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

        public static MsCrmResultObj<List<Town>> GetTowns(Guid cityId, SqlDataAccess sda)
        {
            MsCrmResultObj<List<Town>> returnValue = new MsCrmResultObj<List<Town>>();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    t.new_townId AS Id
	                                    ,t.new_name AS Name
	                                    ,t.new_towncode AS Code
	                                    ,t.new_cityId AS CityId
	                                    ,t.new_cityIdName AS CityIdName
	                                    ,'new_city' AS CityIdTypeName
                                    FROM
                                    new_town AS t (NOLOCK)
                                    WHERE
                                    t.new_cityId='{0}'";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, cityId));

                if (dt.Rows.Count > 0)
                {
                    List<Town> townList = dt.ToList<Town>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = townList;
                }
                else
                {
                    returnValue.Result = "Kayıt bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
                returnValue.HasException = true;
            }

            return returnValue;
        }

        public static MsCrmResultObj<List<Town>> GetTowns(string cityCode, SqlDataAccess sda)
        {
            MsCrmResultObj<List<Town>> returnValue = new MsCrmResultObj<List<Town>>();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    t.new_townId AS Id
	                                    ,t.new_name AS Name
	                                    ,t.new_towncode AS Code
	                                    ,t.new_cityId AS CityId
	                                    ,t.new_cityIdName AS CityIdName
	                                    ,'new_city' AS CityIdTypeName
                                    FROM
                                    new_town AS t (NOLOCK)
	                                    JOIN
		                                    new_city AS c (NOLOCK)
			                                    ON
			                                    t.new_cityId=c.new_cityId
                                    WHERE
                                    c.new_citycode='{0}'";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, cityCode));

                if (dt.Rows.Count > 0)
                {
                    List<Town> townList = dt.ToList<Town>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = townList;
                }
                else
                {
                    returnValue.Result = "Kayıt bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
                returnValue.HasException = true;
            }

            return returnValue;
        }

        public static MsCrmResultObj<List<Town>> GetAllTowns(SqlDataAccess sda)
        {
            MsCrmResultObj<List<Town>> returnValue = new MsCrmResultObj<List<Town>>();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
	                                    t.new_townId AS Id
	                                    ,t.new_name AS Name
	                                    ,t.new_towncode AS Code
	                                    ,t.new_cityId AS CityId
	                                    ,t.new_cityIdName AS CityIdName
	                                    ,'new_city' AS CityIdTypeName
                                    FROM
                                    new_town AS t (NOLOCK)
	                                    JOIN
		                                    new_city AS c (NOLOCK)
			                                    ON
			                                    t.new_cityId=c.new_cityId";

                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery));

                if (dt.Rows.Count > 0)
                {
                    List<Town> townList = dt.ToList<Town>();

                    returnValue.Success = true;
                    returnValue.ReturnObject = townList;
                }
                else
                {
                    returnValue.Result = "Kayıt bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
                returnValue.HasException = true;
            }

            return returnValue;
        }

    }
}
