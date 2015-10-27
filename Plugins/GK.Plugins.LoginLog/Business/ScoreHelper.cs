using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GK.Library.Business
{
    public static class ScoreHelper
    {
        public static MsCrmResultObject GetScoreLimitsByType(ScoreType scoreType, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                sl.new_scorelimitId AS Id
	                                ,sl.new_name AS Name
	                                ,sl.new_scoretype AS ScoreType
	                                ,sl.new_frequency AS Frequency
	                                ,sl.new_point AS Point
	                                ,sl.new_scoreperiod AS Period
                                FROM
	                                new_scorelimit AS sl (NOLOCK)
                                WHERE
	                                sl.new_scoretype=@ScoreType --Sisteme giriş
                                AND
	                                sl.statecode=0
                                AND
	                                sl.statuscode=1 --Aktif
                                ORDER BY
	                                sl.new_scoreperiod ASC";
                #endregion

                SqlParameter[] parameters = {
                                            new SqlParameter("@ScoreType",(int)scoreType)
                                            };

                DataTable dt = sda.getDataTable(query, parameters);


                if (dt != null && dt.Rows.Count > 0)
                {
                    List<ScoreLimit> returnList = new List<ScoreLimit>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ScoreLimit _scoreLimit = new ScoreLimit();
                        _scoreLimit.Id = (Guid)dt.Rows[i]["Id"];
                        _scoreLimit.Name = dt.Rows[i]["Name"] != DBNull.Value ? dt.Rows[i]["Name"].ToString() : string.Empty;
                        _scoreLimit.ScoreType = (ScoreType)(int)dt.Rows[i]["ScoreType"];
                        _scoreLimit.Period = (ScorePeriod)(int)dt.Rows[i]["Period"];
                        _scoreLimit.Point = (int)dt.Rows[i]["Point"];
                        _scoreLimit.Frequency = (int)dt.Rows[i]["Frequency"];

                        returnList.Add(_scoreLimit);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = returnList;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "Skor Limit kaydı bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult CreateScore(Score score, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = new Entity("new_score");
                ent["new_name"] = score.ScoreType.ToString() + "-" + score.User.Name + "-" + DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                ent["new_portalid"] = score.Portal;
                ent["new_userid"] = score.User;
                ent["new_point"] = score.Point;
                ent["new_scoretype"] = new OptionSetValue((int)score.ScoreType);

                returnValue.CrmId = service.Create(ent);
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }
    }
}
