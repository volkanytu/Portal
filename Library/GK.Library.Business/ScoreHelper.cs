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
                ent["new_name"] = score.ScoreType.ToString() + "-" + DateTime.Now.ToString("dd.MM.yyyy HH:mm");
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

        public static MsCrmResultObject GetUserScoreDetails(Guid portalId, Guid portalUserId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
                                    sc.new_scoreId AS Id
                                FROM
                                    new_score AS sc (NOLOCK)
                                WHERE
                                    sc.new_portalId=@PortalId
                                AND
                                    sc.new_userId=@UserId
                                AND
                                    sc.StateCode=0
                                ORDER BY
	                                sc.CreatedOn DESC";
                #endregion

                SqlParameter[] parameters = {
                                            new SqlParameter("@PortalId",portalId)
                                            ,new SqlParameter("@UserId",portalUserId)
                                            };

                DataTable dt = sda.getDataTable(query, parameters);


                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Score> returnList = new List<Score>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Score _score = ScoreHelper.GetScoreDetail((Guid)dt.Rows[i]["Id"], sda);

                        if (_score != null && _score.Id != Guid.Empty)
                        {
                            returnList.Add(_score);
                        }
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = returnList;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "Puan kaydı bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static Score GetScoreDetail(Guid scoreId, SqlDataAccess sda)
        {

            Score returnValue = new Score();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
                                    sc.new_scoreId AS Id
                                    ,sc.new_name AS Name
                                    ,sc.new_point AS Point
                                    ,sc.new_portalId AS PortalId
                                    ,sc.new_portalIdName AS PortalIdName
                                    ,sc.new_scoretype AS ScoreType
                                    ,sm.Value AS ScoreTypeString
                                    ,sc.new_userId AS UserId
                                    ,sc.new_userIdName AS UserIdName
                                    ,sc.CreatedOn
                                FROM
                                    new_score AS sc (NOLOCK)
		                                JOIN
			                                Entity AS e (NOLOCK)
				                                ON
				                                e.Name='new_score'
		                                JOIN
			                                StringMap AS sm (NOLOCK)
				                                ON
				                                sm.ObjectTypeCode=e.ObjectTypeCode
				                                AND
				                                sm.AttributeName='new_scoretype'	
				                                AND
				                                sm.AttributeValue=sc.new_scoretype	
                                WHERE
                                sc.new_scoreId=@ScoreId";
                #endregion

                SqlParameter[] parameters = {
                                            new SqlParameter("@ScoreId",scoreId)
                                            };

                DataTable dt = sda.getDataTable(query, parameters);


                if (dt.Rows.Count > 0)
                {
                    returnValue.Id = (Guid)dt.Rows[0]["Id"];
                    returnValue.Name = dt.Rows[0]["Name"].ToString();
                    returnValue.Point = (int)dt.Rows[0]["Point"];
                    returnValue.ScoreType = (ScoreType)(int)dt.Rows[0]["ScoreType"];
                    returnValue.ScoreTypeString = dt.Rows[0]["ScoreTypeString"].ToString();
                    returnValue.CreatedOn = ((DateTime)dt.Rows[0]["CreatedOn"]).ToLocalTime();
                    returnValue.CreatedOnString = ((DateTime)dt.Rows[0]["CreatedOn"]).ToLocalTime().ToString("dd.MM.yyyy HH:mm");

                    if (dt.Rows[0]["PortalId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference()
                        {
                            Id = (Guid)dt.Rows[0]["PortalId"],
                            Name = dt.Rows[0]["PortalIdName"].ToString()
                        };

                        returnValue.Portal = er;
                    }

                    if (dt.Rows[0]["UserId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference()
                        {
                            Id = (Guid)dt.Rows[0]["UserId"],
                            Name = dt.Rows[0]["UserIdName"].ToString()
                        };

                        returnValue.User = er;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return returnValue;
        }

        public static int GetUserScore(Guid portalId, Guid portalUserId, SqlDataAccess sda)
        {
            int returnValue = 0;

            #region | SQL QUERY |
            string query = @"SELECT
                                    sc.new_scoreId AS Id
                                    ,ISNULL(sc.new_point,0) AS Point
                                FROM
                                    new_score AS sc (NOLOCK)
                                WHERE
                                    sc.new_portalId=@PortalId
                                AND
                                    sc.new_userId=@UserId
                                AND
                                    sc.StateCode=0
                                ORDER BY
	                                sc.CreatedOn DESC";
            #endregion

            SqlParameter[] parameters = {
                                            new SqlParameter("@PortalId",portalId)
                                            ,new SqlParameter("@UserId",portalUserId)
                                            };

            DataTable dt = sda.getDataTable(query, parameters);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    returnValue += (int)dt.Rows[i]["Point"];
                }
            }

            return returnValue;
        }
    }
}
