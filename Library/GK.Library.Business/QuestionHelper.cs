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
    public static class QuestionHelper
    {
        public static MsCrmResultObject GetQuestionLevels(Guid portalId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                    ql.new_questionlevelId AS Id
	                                    ,ql.new_name AS Name
	                                    ,ql.new_portalId AS PortalId
	                                    ,ql.new_portalIdName AS PortalIdName
                                        ,ql.new_imageurl AS ImageUrl
                                    FROM
	                                    new_questionlevel AS ql (NOLOCK)
                                    WHERE
	                                    ql.new_portalId='{0}'
                                    AND
	                                    ql.statecode=0
                                    AND
	                                    ql.statuscode=1 --Active";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<QuestionLevel> returnList = new List<QuestionLevel>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        QuestionLevel _questionLevel = new QuestionLevel();
                        _questionLevel.Id = (Guid)dt.Rows[i]["Id"];
                        _questionLevel.Name = dt.Rows[i]["Name"] != DBNull.Value ? dt.Rows[i]["Name"].ToString() : string.Empty;
                        _questionLevel.ImagePath = dt.Rows[i]["ImageUrl"] != DBNull.Value ? dt.Rows[i]["ImageUrl"].ToString() : "no_image_available.png";

                        if (dt.Rows[i]["PortalId"] != DBNull.Value)
                        {
                            EntityReference er = new EntityReference()
                            {
                                Id = (Guid)dt.Rows[i]["PortalId"],
                                Name = dt.Rows[i]["PortalIdName"].ToString()
                            };

                            _questionLevel.Portal = er;
                        }

                        returnList.Add(_questionLevel);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = returnList;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M027"; //"Herhangi bir soru seviyesi tanımı bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetQuestionInfo(Guid questionId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                q.new_questionId AS Id
	                                ,q.new_name AS Name
	                                ,q.new_portalId AS PortalId
	                                ,q.new_portalIdName AS PortalIdName
	                                ,q.new_questionlevelId AS LevelId
	                                ,q.new_questionlevelIdName AS LevelIdName
	                                --,q.new_portal_rubic_cube_definitionid AS DefinationId
	                                --,q.new_portal_rubic_cube_definitionidName AS DefinationIdName
	                                ,q.new_category AS QuestionCategoryValue
	                                ,NULL AS QuestionCategoryName
	                                ,q.new_timecount AS [Time]
                                    ,q.new_point AS [Point]
                                FROM
	                                new_question AS q (NOLOCK)
                                WHERE
	                                q.new_questionId='{0}'
                                AND
	                                q.statecode=0
                                AND
	                                q.statuscode=1 --Active";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, questionId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    Question _question = new Question();
                    _question.Id = (Guid)dt.Rows[0]["Id"];
                    _question.Name = dt.Rows[0]["Name"] != DBNull.Value ? dt.Rows[0]["Name"].ToString() : string.Empty;

                    if (dt.Rows[0]["PortalId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference()
                        {
                            Id = (Guid)dt.Rows[0]["PortalId"],
                            Name = dt.Rows[0]["PortalIdName"].ToString()
                        };

                        _question.Portal = er;
                    }

                    if (dt.Rows[0]["LevelId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference()
                        {
                            Id = (Guid)dt.Rows[0]["LevelId"],
                            Name = dt.Rows[0]["LevelIdName"].ToString()
                        };

                        _question.QuestionLevel = er;
                    }

                    //if (dt.Rows[0]["DefinationId"] != DBNull.Value)
                    //{
                    //    EntityReference er = new EntityReference()
                    //    {
                    //        Id = (Guid)dt.Rows[0]["DefinationId"],
                    //        Name = dt.Rows[0]["DefinationIdName"].ToString()
                    //    };

                    //    _question.QuestionDefination = er;
                    //}

                    if (dt.Rows[0]["QuestionCategoryValue"] != DBNull.Value)
                    {
                        StringMap sm = new StringMap()
                        {
                            Value = (int)dt.Rows[0]["QuestionCategoryValue"],
                            Name = dt.Rows[0]["QuestionCategoryName"] != DBNull.Value ? dt.Rows[0]["QuestionCategoryName"].ToString() : string.Empty
                        };

                        _question.QuestionCategory = sm;
                    }

                    _question.Time = dt.Rows[0]["Time"] != DBNull.Value ? (int)dt.Rows[0]["Time"] : 0;
                    _question.Point = dt.Rows[0]["Point"] != DBNull.Value ? (int)dt.Rows[0]["Point"] : 0;

                    _question.QuestionChoices = (List<QuestionChoices>)GetQuestionChoices(_question.Id, sda).ReturnObject;

                    returnValue.Success = true;
                    returnValue.ReturnObject = _question;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M028"; //"Soruya ait bilgi bulunamadı";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetQuestionChoices(Guid questionId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
	                                    c.new_questionchoiceId AS Id
	                                    ,c.new_name AS Name
	                                    ,c.new_iscorrect AS IsCorrect
	                                    ,c.new_questionId AS QuestionId
	                                    ,c.new_questionIdName AS QUestionIdName
                                    FROM
	                                    new_questionchoice AS c (NOLOCK)
                                    WHERE
	                                    c.new_questionId='{0}'
                                    AND
	                                    c.statecode=0 ORDER BY NEWID()";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, questionId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<QuestionChoices> returnList = new List<QuestionChoices>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        QuestionChoices _questionChoices = new QuestionChoices();
                        _questionChoices.Id = (Guid)dt.Rows[i]["Id"];
                        _questionChoices.Name = dt.Rows[i]["Name"] != DBNull.Value ? dt.Rows[i]["Name"].ToString() : string.Empty;

                        if (dt.Rows[i]["QuestionId"] != DBNull.Value)
                        {
                            EntityReference er = new EntityReference()
                            {
                                Id = (Guid)dt.Rows[i]["QuestionId"],
                                Name = dt.Rows[i]["QuestionIdName"].ToString()
                            };

                            _questionChoices.Question = er;
                        }

                        _questionChoices.IsCorrect = dt.Rows[i]["IsCorrect"] != DBNull.Value ? (bool)dt.Rows[i]["IsCorrect"] : false;

                        returnList.Add(_questionChoices);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = returnList;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M030"; //"Soruya ait şıklar bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject SelectQuestion(Guid portalUserId, Guid portalId, Guid questionLevelId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT TOP 1
	                                q.new_questionId AS QUestionId
                                FROM
	                                new_question AS q (NOLOCK)
                                WHERE
	                                q.new_portalId='{0}'
                                AND
	                                q.new_questionlevelId='{1}'
                                AND
	                                q.statecode=0
                                AND
	                                q.statuscode=1 --Active
                                AND
	                                q.new_questionId NOT IN
	                                (
		                                SELECT
			                                q.new_questionId
		                                FROM
			                                new_questionanswers AS a (NOLOCK)
		                                WHERE
			                                a.new_questionId=q.new_questionId
                                        AND
                                            a.new_userId='{2}'
                                        AND
                                            a.new_point IS NOT NULL AND a.new_point != 0
		                                AND
			                                a.statecode=0
	                                )
                                ORDER BY NEWID()";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalId, questionLevelId, portalUserId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    returnValue = GetQuestionInfo((Guid)dt.Rows[0]["QuestionId"], sda);

                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M029"; //"Soru seviyesine ait uygun soru bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetUserCubeStatus(Guid portalUserId, Guid portalId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
                                *
                                FROM
                                (
	                                SELECT
		                                *
		                                ,DENSE_RANK() OVER (ORDER BY B.TotalPoint DESC) AS Ranking
	                                FROM
	                                (
		                                SELECT
			                                A.UserId
			                                ,ISNULL(SUM(A.Point),0) AS TotalPoint
		                                FROM
		                                (
			                                SELECT DISTINCT
				                                u.new_userId AS UserId
				                                ,a.new_point AS Point
				                                ,a.new_questionanswersId AS AnswerId
			                                FROM
				                                new_user AS u (NOLOCK)
					                                LEFT JOIN
						                                new_questionanswers AS a (NOLOCK)
							                                ON
							                                u.new_userId=a.new_userId
							                                AND
							                                a.statecode=0
					                                JOIN
						                                new_new_user_new_role AS urd (NOLOCK)
							                                ON
							                                u.new_userId=urd.new_userid
					                                JOIN
						                                new_role AS rd (NOLOCK)
							                                ON
							                                urd.new_roleid=rd.new_roleId
							                                AND
							                                rd.statecode=0
							                                AND
							                                rd.statuscode=1 --Active
							                                AND
							                                rd.new_portalId='{1}'
			                                WHERE
				                                u.statecode=0
			                                AND
				                                u.statuscode=1 --Active
		                                ) AS A
		                                GROUP BY
			                                A.UserId
	                                ) AS B
                                ) AS C
                                WHERE
	                                C.UserId='{0}'";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(query, portalUserId, portalId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    UserCubeStatus _cubeStatus = new UserCubeStatus();

                    if (dt.Rows[0]["UserId"] != DBNull.Value)
                    {
                        EntityReference er = new EntityReference()
                        {
                            Id = (Guid)dt.Rows[0]["UserId"]
                        };

                        _cubeStatus.UserId = er;
                    }

                    _cubeStatus.Point = dt.Rows[0]["TotalPoint"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["TotalPoint"]) : 0;
                    _cubeStatus.Rank = dt.Rows[0]["Ranking"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Ranking"]) : 0;

                    returnValue.Success = true;
                    returnValue.ReturnObject = _cubeStatus;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M032"; //"Kullanıcı soru cevaplarına ait bilgiler bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResultObject GetCubeStatusList(Guid portalId, SqlDataAccess sda)
        {

            MsCrmResultObject returnValue = new MsCrmResultObject();

            DateTime start = GeneralHelper.GetStartDateByScorePeriod(ScorePeriod.Monthly);
            DateTime end = GeneralHelper.GetEndDateByScorePeriod(ScorePeriod.Monthly);

            try
            {
                #region | SQL QUERY |
                string query = @"SELECT
                                    *
                                INTO
                                    #temp
                                FROM
                                (
                                    SELECT
                                        *
                                        ,DENSE_RANK() OVER (ORDER BY B.AllPoints DESC) AS Ranking
                                    FROM
                                    (
                                        SELECT
                                            A.UserId
                                            ,ISNULL(SUM(A.Point),0) AS TotalPoint
                                            ,ISNULL(SUM(CASE WHEN A.ScoreType=100000012 THEN 0 ELSE A.Point END),0) AS AllPoints
                                        FROM
                                        (
                                            SELECT DISTINCT
                                                u.new_userId AS UserId
                                                ,a.new_point AS Point
                                                ,a.new_scoreId AS AnswerId
                                                ,a.new_scoretype AS ScoreType
                                            FROM
                                                new_user AS u (NOLOCK)
                                                    LEFT JOIN
                                                        new_score AS a (NOLOCK)
                                                            ON
                                                            u.new_userId=a.new_userId
                                                            --AND
                                                            --a.CreatedOn BETWEEN @start AND @end
                                                            AND
                                                            a.statecode=0
                                                    JOIN
                                                        new_new_user_new_role AS urd (NOLOCK)
                                                            ON
                                                            u.new_userId=urd.new_userid
                                                    JOIN
                                                        new_role AS rd (NOLOCK)
                                                            ON
                                                            urd.new_roleid =rd.new_roleId
                                                            AND
                                                            rd.statecode=0
                                                            AND
                                                            rd.statuscode=1 --Active
                                                            AND
                                                            rd.new_portalId='{0}'
                                            WHERE
                                                u.statecode=0
                                            AND
                                                u.statuscode=1 --Active
                                        ) AS A
                                        GROUP BY
                                            A.UserId
                                    ) AS B
                                ) AS C


                                SELECT
                                    t.*
                                    ,c.FullName AS UserIdName
                                FROM
                                    #temp AS t (NOLOCK)
                                        JOIN
                                            new_user AS u (NOLOCK)
                                                ON
                                                t.UserId=u.new_userId
                                        JOIN
                                            Contact AS c (NOLOCK)
                                                ON
                                                u.new_contactId =c.ContactId
                                ORDER BY 
	                                t.Ranking ASC

                                DROP TABLE #temp";
                #endregion

                SqlParameter[] parameters = {
                                            new SqlParameter("@start",start)
                                            ,new SqlParameter("@end",end)
                                        };

                DataTable dt = sda.getDataTable(string.Format(query, portalId), parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<UserCubeStatus> returnList = new List<UserCubeStatus>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        UserCubeStatus _cubeStatus = new UserCubeStatus();

                        if (dt.Rows[i]["UserId"] != DBNull.Value)
                        {
                            EntityReference er = new EntityReference()
                            {
                                Id = (Guid)dt.Rows[i]["UserId"],
                                Name = dt.Rows[i]["UserIdName"].ToString()
                            };

                            _cubeStatus.UserId = er;
                        }

                        _cubeStatus.Point = dt.Rows[i]["TotalPoint"] != DBNull.Value ? Convert.ToInt32(dt.Rows[i]["TotalPoint"]) : 0;
                        _cubeStatus.AllPoints = dt.Rows[i]["AllPoints"] != DBNull.Value ? Convert.ToInt32(dt.Rows[i]["AllPoints"]) : 0;
                        _cubeStatus.Rank = dt.Rows[i]["Ranking"] != DBNull.Value ? Convert.ToInt32(dt.Rows[i]["Ranking"]) : 0;

                        returnList.Add(_cubeStatus);
                    }

                    returnValue.Success = true;
                    returnValue.ReturnObject = returnList;
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M033"; //"Soru durum listesi bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static MsCrmResult HasUserQuestionLimit(Guid portalUserId, Guid portalId, SqlDataAccess sda)
        {

            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                #region | SQL QUERY |
                string query = @" 
                                    DECLARE @now DATETIME
                                    DECLARE @startDate DATETIME
                                    DECLARE @endDate DATETIME
                                    DECLARE @current INT
                                    DECLARE @limit INT
                                    DECLARE @hasLimit BIT

                                    SET @now=GETUTCDATE()

                                    SELECT
	                                    @limit=sl.new_frequency
	                                    ,@startDate= CASE
		                                    WHEN
			                                    sl.new_scoreperiod=100000000 --DAILY
		                                    THEN
			                                    {2}.dbo.fn_BeginOfToday(@now)
		                                    ELSE
			                                    CASE 
				                                    WHEN
					                                    sl.new_scoreperiod=100000001 --WEEKLY
				                                    THEN
					                                    {2}.dbo.fn_BeginOfThisWeek(@now)
				                                    ELSE
					                                    CASE 
						                                    WHEN
							                                    sl.new_scoreperiod=100000002 --MONTHLY
						                                    THEN
							                                    {2}.dbo.fn_BeginOfThisMonth(@now)
						                                    ELSE
							                                    {2}.dbo.fn_BeginOfThisYear(@now)
					                                    END
			                                    END
	                                    END
	                                    ,@endDate= CASE
		                                    WHEN
			                                    sl.new_scoreperiod=100000000 --DAILY
		                                    THEN
			                                    {2}.dbo.fn_EndOfToday(@now)
		                                    ELSE
			                                    CASE 
				                                    WHEN
					                                    sl.new_scoreperiod=100000001 --WEEKLY
				                                    THEN
					                                    {2}.dbo.fn_EndOfThisWeek(@now)
				                                    ELSE
					                                    CASE 
						                                    WHEN
							                                    sl.new_scoreperiod=100000003 --MONTHLY
						                                    THEN
							                                    {2}.dbo.fn_EndOfThisMonth(@now)
						                                    ELSE
							                                    {2}.dbo.fn_EndOfThisYear(@now)
					                                    END
			                                    END
	                                    END
                                    FROM
	                                    new_scorelimit AS sl (NOLOCK)
                                    WHERE
	                                    sl.new_scoretype=100000000 --InfoCube

                                    SELECT
	                                    @current = COUNT(0)
                                    FROM
	                                    new_questionanswers AS ans (NOLOCK)
                                    WHERE
	                                    ans.new_portalId='{1}'
                                    AND
	                                    ans.new_userId='{0}'
                                    AND
	                                    ans.statecode=0
                                    AND
	                                    ans.statuscode=1 --Active
                                    AND
	                                    ans.CreatedOn BETWEEN @startDate AND @endDate

                                    SELECT 
	                                   @hasLimit= CASE  
		                                    WHEN
			                                    @current < @limit OR @limit IS NULL
		                                    THEN 
			                                    1
		                                    ELSE
			                                    0
	                                    END

                                    SELECT @hasLimit";
                #endregion

                returnValue.Success = (bool)sda.ExecuteScalar(string.Format(query, portalUserId, portalId, Globals.DatabaseName));
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public static int GetUserAnswerCount(Guid portalId, Guid portalUserId, DateTime start, DateTime end, SqlDataAccess sda)
        {
            int returnValue = 0;

            try
            {
                #region | SQL QUERY |

                string sqlQuery = @"SELECT
                                        COUNT(0)
                                    FROM
                                        new_questionanswers AS qa (NOLOCK)
                                    WHERE
                                        qa.new_portalId=@PortalId
                                    AND
                                        qa.new_userId=@UserId
                                    AND
                                        qa.CreatedOn BETWEEN @start AND @end
                                    AND
                                        qa.StateCode=0";

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
