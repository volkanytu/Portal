using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using GK.Library.Business;
using GK.Library.Utility;

namespace GK.Plugins.DiscoveryForm
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
                Entity preImage = null;
                if (context.PreEntityImages.Contains("PreImage") && context.PreEntityImages["PreImage"] is Entity)
                {
                    preImage = (Entity)context.PreEntityImages["PreImage"];
                }

                #endregion

                #region | VARIABLES |
                List<ScoreLimit> lstLimits = new List<ScoreLimit>();

                EntityReference portal = null;
                EntityReference user = null;

                if (preImage.Contains("new_portalid") && preImage["new_portalid"] != null)
                {
                    portal = (EntityReference)preImage["new_portalid"];
                }

                if (preImage.Contains("new_userid") && preImage["new_userid"] != null)
                {
                    user = (EntityReference)preImage["new_userid"];
                }
                #endregion

                if (entity.Contains("statuscode") && entity["statuscode"] != null
                    && ((OptionSetValue)entity["statuscode"]).Value == (int)DiscoveryFormStatus.CrmConfirmed)
                {

                    MsCrmResultObject limitRes = ScoreHelper.GetScoreLimitsByType(ScoreType.KGSSales, sda);

                    if (limitRes.Success)
                    {
                        lstLimits = (List<ScoreLimit>)limitRes.ReturnObject;

                        for (int i = 0; i < lstLimits.Count; i++)
                        {
                            Score sc = new Score()
                            {
                                Point = lstLimits[i].Point,
                                Portal = portal,
                                User = user,
                                ScoreType = ScoreType.KGSSales
                            };

                            MsCrmResult scoreRes = ScoreHelper.CreateScore(sc, service);

                            if (scoreRes.Success)
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
