using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using GK.Library.Business;
using GK.Library.Utility;

namespace GK.Plugins.QuestionAnswer
{
    public class PostCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            SqlDataAccess sda = null;

            try
            {
                sda = new SqlDataAccess();
                sda.openConnection(Globals.ConnectionString);

                #region | SERVICE |
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                #region | Validate Request |
                //Target yoksa veya Entity tipinde değilse, devam etme.
                if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity))
                {
                    return;
                }
                #endregion

                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                #endregion

                Entity entity = (Entity)context.InputParameters["Target"];

                #region | VARIABLES |
                List<ScoreLimit> lstLimits = new List<ScoreLimit>();

                EntityReference portal = null;
                EntityReference user = null;
                int point = 0;

                if (entity.Contains("new_point") && entity["new_point"] != null)
                {
                    point = (int)entity["new_point"];
                }

                if (entity.Contains("new_portalid") && entity["new_portalid"] != null)
                {
                    portal = (EntityReference)entity["new_portalid"];
                }

                if (entity.Contains("new_userid") && entity["new_userid"] != null)
                {
                    user = (EntityReference)entity["new_userid"];
                }
                #endregion

                MsCrmResultObject limitRes = ScoreHelper.GetScoreLimitsByType(ScoreType.Question, sda);

                if (limitRes.Success && point != 0)
                {
                    lstLimits = (List<ScoreLimit>)limitRes.ReturnObject;

                    for (int i = 0; i < lstLimits.Count; i++)
                    {
                        int recCount = 0;
                        DateTime start = GeneralHelper.GetStartDateByScorePeriod(lstLimits[i].Period);
                        DateTime end = GeneralHelper.GetEndDateByScorePeriod(lstLimits[i].Period);

                        recCount = QuestionHelper.GetUserAnswerCount(portal.Id, user.Id, start, end, sda);

                        if (lstLimits[i].Frequency >= recCount)
                        {
                            Score sc = new Score()
                            {
                                Point = point,
                                Portal = portal,
                                User = user,
                                ScoreType = ScoreType.Question
                            };

                            MsCrmResult scoreRes = ScoreHelper.CreateScore(sc, service);

                            break;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                //LOG
                throw new InvalidPluginExecutionException(ex.Message);
            }
            finally
            {
                if (sda != null)
                    sda.closeConnection();
            }
        }
    }
}
