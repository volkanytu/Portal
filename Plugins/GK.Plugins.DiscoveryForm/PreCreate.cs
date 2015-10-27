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
    public class PreCreate : IPlugin
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

                MsCrmResultObject resultMaxCode = DiscoveryFormHelper.GetMaxFormCodeNumber(sda);

                if (resultMaxCode.Success)
                {
                    int maxCode = (int)resultMaxCode.ReturnObject;

                    if (maxCode == 0)
                    {
                        maxCode = 200000;
                    }

                    entity["new_formcode"] = ++maxCode;
                }
                else
                {
                    entity["new_formcode"] = -1;
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
