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
    public static class SurveyHelper
    {
        public static MsCrmResultObject SelectSurvey(Guid portalId, Guid portalUserId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            bool isLimitOver = false;

            try
            {

                #region | CHECK LIMIT |
                List<ScoreLimit> lstLimits = new List<ScoreLimit>();

                MsCrmResultObject limitRes = ScoreHelper.GetScoreLimitsByType(ScoreType.Survey, sda);

                if (limitRes.Success)
                {
                    lstLimits = (List<ScoreLimit>)limitRes.ReturnObject;

                    for (int i = 0; i < lstLimits.Count; i++)
                    {
                        int recCount = 0;
                        DateTime start = GeneralHelper.GetStartDateByScorePeriod(lstLimits[i].Period);
                        DateTime end = GeneralHelper.GetEndDateByScorePeriod(lstLimits[i].Period);

                        recCount = SurveyHelper.GetUserSurveyAnswerCount(portalId, portalUserId, start, end, sda);

                        if (lstLimits[i].Frequency <= recCount)
                        {
                            returnValue.Result = "Anket cevaplama limitiniz dolmuştur.<br /> Limitler hakkında bilgiye Puanlarım bölümünden ulaşabilirsiniz.";
                            isLimitOver = true;
                            break;

                        }
                    }
                }

                if (isLimitOver)
                {
                    return returnValue;
                }
                #endregion

                #region | SQL QUERY |

                string sqlQuery = @"DECLARE @Date DATETIME = GETUTCDATE()

                                SELECT DISTINCT
	                                E.new_surveyId Id
                                FROM
                                new_survey AS E (NOLOCK)
	                                JOIN
		                                new_new_user_new_role AS UR (NOLOCK)
			                                ON
			                                UR.new_userid='{1}'
	                                JOIN
		                                new_role AS RD (NOLOCK)
			                                ON
			                                RD.new_roleId=UR.new_roleid
			                                AND
			                                Rd.statecode=0
			                                AND
			                                RD.statuscode=1 --Active
	                                JOIN
		                                new_new_survey_new_role AS ERDF (NOLOCK)
			                                ON
			                                ERDF.new_surveyid =E.new_surveyId
			                                AND
			                                ERDF.new_roleid =RD.new_roleId
	                                JOIN
		                                dbo.UserSettingsBase US (NOLOCK)
			                                ON 
			                                US.SystemUserId ='{2}'
                                WHERE
	                                @Date BETWEEN E.new_startdate AND E.new_enddate
	                                AND
	                                E.new_portalId = '{0}'
                                    AND
                                    E.statuscode=1 --Active
	                                AND
	                                E.new_surveyId NOT IN
	                                (
		                                SELECT
			                                a.new_surveyId
		                                FROM
			                                new_surveyanswer AS a (NOLOCK)
		                                WHERE
			                                a.new_surveyId =E.new_surveyId
                                        AND
                                            a.new_userId='{1}'
                                       AND
			                                a.statecode=0
	                                )";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, portalId, portalUserId, Globals.AdminId));

                if (dt.Rows.Count > 0)
                {
                    returnValue = SurveyHelper.GetSurveyInfo((Guid)dt.Rows[0]["Id"], sda);

                }
                else
                {
                    returnValue.Result = "M052"; //"Herhangi bir anket kaydı bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResultObject GetSurveyInfo(Guid surveyId, SqlDataAccess sda)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT                                    
	                                    E.new_surveyId Id
	                                    ,E.new_name Name
	                                    ,E.new_startdate AS StartDate
	                                    ,E.new_enddate AS EndDate
	                                    ,E.new_portalId AS PortalId
	                                    ,E.new_portalIdName AS PortalIdName
	                                    ,CAST({2}.dbo.fn_UTCToTzSpecificLocalTime(E.CreatedOn, us.TimeZoneBias, us.TimeZoneDaylightBias,us.TimeZoneDaylightYear, us.TimeZoneDaylightMonth, us.TimeZoneDaylightDay, us.TimeZoneDaylightHour,us.TimeZoneDaylightMinute, us.TimeZoneDaylightSecond, 0, us.TimeZoneDaylightDayOfWeek,us.TimeZoneStandardBias, us.TimeZoneStandardYear, us.TimeZoneStandardMonth, us.TimeZoneStandardDay,us.TimeZoneStandardHour, us.TimeZoneStandardMinute, us.TimeZoneStandardSecond, 0,us.TimeZoneStandardDayOfWeek) as DATETIME) CreatedOn
                                    FROM
	                                    new_survey AS E (NOLOCK)
		                                    JOIN
			                                    dbo.UserSettingsBase US (NOLOCK)
				                                    ON 
				                                    US.SystemUserId ='{1}'
                                    WHERE
	                                    E.new_surveyId='{0}'";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, surveyId, Globals.AdminId, Globals.DatabaseName));

                if (dt.Rows.Count > 0)
                {
                    Survey sur = new Survey();

                    sur.Id = (Guid)dt.Rows[0]["Id"];
                    sur.Name = dt.Rows[0]["Name"] != DBNull.Value ? dt.Rows[0]["Name"].ToString() : string.Empty;

                    if (dt.Rows[0]["PortalId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference()
                        {
                            Id = (Guid)dt.Rows[0]["PortalId"],
                            Name = dt.Rows[0]["PortalIdName"].ToString()
                        };

                        sur.Portal = er;
                    }

                    if (dt.Rows[0]["StartDate"] != DBNull.Value)
                    {
                        sur.StartDate = (DateTime)dt.Rows[0]["StartDate"];
                    }

                    if (dt.Rows[0]["EndDate"] != DBNull.Value)
                    {
                        sur.EndDate = (DateTime)dt.Rows[0]["EndDate"];
                    }

                    MsCrmResultObject choiceRes = SurveyHelper.GetSurveyChoices(sur.Id, sda);

                    if (choiceRes.Success)
                    {
                        sur.SurveyChoices = (List<SurveyChoices>)choiceRes.ReturnObject;
                    }

                    returnValue.ReturnObject = sur;
                    returnValue.Success = true;
                    returnValue.Result = "Anket bilgisi çekildi.";
                }
                else
                {
                    returnValue.Result = "Anket bilgisi bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public static MsCrmResultObject GetSurveyChoices(Guid surveyId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                    c.new_surveychoiceId AS Id
	                                    ,c.new_name AS Name
	                                    ,c.new_surveyId AS SurveyId
	                                    ,c.new_surveyIdName AS SurveyIdName
                                    FROM
	                                    new_surveychoice AS c (NOLOCK)
                                    WHERE
	                                    c.new_surveyId='{0}'
                                    AND
	                                    c.statecode=0";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, surveyId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<SurveyChoices> returnList = new List<SurveyChoices>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SurveyChoices surveyChoices = new SurveyChoices();
                        surveyChoices.Id = (Guid)dt.Rows[i]["Id"];
                        surveyChoices.Name = dt.Rows[i]["Name"] != DBNull.Value ? dt.Rows[i]["Name"].ToString() : string.Empty;

                        if (dt.Rows[i]["SurveyId"] != DBNull.Value)
                        {
                            EntityReference er = new EntityReference()
                            {
                                Id = (Guid)dt.Rows[i]["SurveyId"],
                                Name = dt.Rows[i]["SurveyIdName"].ToString()
                            };

                            surveyChoices.Survey = er;
                        }

                        returnList.Add(surveyChoices);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = returnList;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "Ankete ait şıklar bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult AnswerSurvey(SurveyAnswer answer, IOrganizationService service)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                Entity ent = new Entity("new_surveyanswer");
                ent["new_name"] = answer.PortalUser.Name + "-" + DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                ent["new_portalid"] = answer.Portal;
                ent["new_userid"] = answer.PortalUser;
                ent["new_surveyid"] = answer.Survey;
                ent["new_surveychoiceid"] = answer.SurveyChoice;

                returnValue.CrmId = service.Create(ent);

                returnValue.Success = true;
                returnValue.Result = "M053"; //"Anket cevabınız alınmıştır. <br /> Katılımınız için teşekkür ederiz.";

            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static int GetUserSurveyAnswerCount(Guid portalId, Guid portalUserId, DateTime start, DateTime end, SqlDataAccess sda)
        {
            int returnValue = 0;

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
                                    COUNT(0)
                                FROM
                                    new_surveyanswer AS sa (NOLOCK)
                                WHERE
                                    sa.new_portalId=@PortalId
                                AND
                                    sa.new_userId=@UserId
                                AND
                                    sa.CreatedOn BETWEEN @start AND @end
                                AND
                                    sa.StateCode=0";

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
