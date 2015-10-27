using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using GK.Library.Business;
using GK.Library.Utility;

namespace GK.Plugins.ForumSubject
{
    public class PostUpdate : IPlugin
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

                #region |DEFINE IMAGE IF EXISTS|
                Entity postImage = null;
                if (context.PostEntityImages.Contains("PostImage") && context.PostEntityImages["PostImage"] is Entity)
                {
                    postImage = (Entity)context.PostEntityImages["PostImage"];
                }
                #endregion

                #region | VARIABLES |
                List<ScoreLimit> lstLimits = new List<ScoreLimit>();

                EntityReference portal = null;
                EntityReference user = null;

                if (postImage.Contains("new_portalid") && postImage["new_portalid"] != null)
                {
                    portal = (EntityReference)postImage["new_portalid"];
                }

                if (postImage.Contains("new_userid") && postImage["new_userid"] != null)
                {
                    user = (EntityReference)postImage["new_userid"];
                }
                #endregion

                if (entity.Contains("statuscode") && entity["statuscode"] != null && ((OptionSetValue)entity["statuscode"]).Value == 1)
                {
                    MsCrmResultObject limitRes = ScoreHelper.GetScoreLimitsByType(ScoreType.ForumSubject, sda);

                    if (limitRes.Success)
                    {
                        lstLimits = (List<ScoreLimit>)limitRes.ReturnObject;

                        for (int i = 0; i < lstLimits.Count; i++)
                        {
                            int recCount = 0;
                            DateTime start = GeneralHelper.GetStartDateByScorePeriod(lstLimits[i].Period);
                            DateTime end = GeneralHelper.GetEndDateByScorePeriod(lstLimits[i].Period);

                            recCount = ForumHelper.GetUserForumSubjectCount(portal.Id, user.Id, start, end, sda);

                            if (lstLimits[i].Frequency >= recCount)
                            {
                                Score sc = new Score()
                                {
                                    Point = lstLimits[i].Point,
                                    Portal = portal,
                                    User = user,
                                    ScoreType = ScoreType.ForumSubject
                                };

                                MsCrmResult scoreRes = ScoreHelper.CreateScore(sc, service);

                                break;
                            }
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
