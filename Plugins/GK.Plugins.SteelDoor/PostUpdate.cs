using GK.Library.Business;
using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Plugins.SteelDoor
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
                string customerFirstName = null;
                string customerLastName = null;
                string customerMobilePhone = null;
                string customerFullName = null;
                string assemblerFullName = null;

                int statusCode = 0;
                int oldStatusCode = 0;

                AssemblerInfo assemblerInfo = null;
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

                    MsCrmResultObj<AssemblerInfo> resultAssembler = AssemblyRequestHelper.GetAssemblerInfo(user.Id, sda);

                    if (resultAssembler.Success)
                    {
                        assemblerInfo = resultAssembler.ReturnObject;
                    }
                    else
                    {
                        throw new Exception("Anahtarcı bilgisi alınamadı.Hata:" + resultAssembler.Result);
                    }

                }

                if (preImage.Contains("new_firstname") && preImage["new_firstname"] != null)
                {
                    customerFirstName = preImage["new_firstname"].ToString();
                }

                if (preImage.Contains("new_lastname") && preImage["new_lastname"] != null)
                {
                    customerLastName = preImage["new_lastname"].ToString();
                }

                if (preImage.Contains("new_mobilephone") && preImage["new_mobilephone"] != null)
                {
                    customerMobilePhone = preImage["new_mobilephone"].ToString();
                }

                if (!string.IsNullOrWhiteSpace(customerFirstName) && !string.IsNullOrWhiteSpace(customerLastName))
                {
                    customerFullName = customerFirstName + " " + customerLastName;
                }

                if (entity.Contains("statuscode") && entity["statuscode"] != null)
                {
                    statusCode = ((OptionSetValue)entity["statuscode"]).Value;
                }

                if (preImage.Contains("statuscode") && preImage["statuscode"] != null)
                {
                    oldStatusCode = ((OptionSetValue)preImage["statuscode"]).Value;
                }
                #endregion

                if (statusCode != oldStatusCode && (statusCode == (int)SteelDoorStatus.CrmConfirmed || statusCode == (int)SteelDoorStatus.Result_Negative))
                {
                    string statusText = string.Empty;

                    if (statusCode == (int)SteelDoorStatus.CrmConfirmed)
                    {
                        statusText = "Kale Onayladı";
                    }
                    else if (statusCode == (int)SteelDoorStatus.Result_Negative)
                    {
                        statusText = "Sonuç Olumsuz";
                    }

                    string customerSmsText = @"Sayın {0}, {1} müşterisine ait Kale Çelik Kapı Formu durumu {2} olarak değişmiştir. Bilgilerinize.";

                    Entity ent = new Entity("new_sms");
                    ent["new_message"] = string.Format(customerSmsText, assemblerFullName, customerFullName, statusText);
                    ent["new_phonenumber"] = assemblerInfo.MobilePhoneNumber;
                    ent["new_name"] = customerFirstName + " " + customerLastName + "|Çelik Kapı Formu Durum Değişimi";
                    ent["new_userid"] = user;

                    service.Create(ent);
                }

                if (entity.Contains("statuscode") && entity["statuscode"] != null
                    && ((OptionSetValue)entity["statuscode"]).Value == (int)SteelDoorStatus.CrmConfirmed)
                {
                    MsCrmResultObject limitRes = ScoreHelper.GetScoreLimitsByType(ScoreType.SteelDoor, sda);

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
                                ScoreType = ScoreType.SteelDoor
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
