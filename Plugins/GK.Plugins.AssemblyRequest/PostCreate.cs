using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using GK.Library.Business;
using GK.Library.Utility;

namespace GK.Plugins.AssemblyRequest
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

                EntityReference userId = null;
                EntityReference cityId = null;
                EntityReference townId = null;
                AssemblerInfo assemblerInfo = null;

                string customerFirstName = null;
                string customerLastName = null;
                string customerMobilePhone = null;
                string customerFullName = null;
                string assemblerFullName = null;

                if (entity.Contains("new_userid") && entity["new_userid"] != null)
                {
                    userId = (EntityReference)entity["new_userid"];

                    MsCrmResultObj<AssemblerInfo> resultAssembler = AssemblyRequestHelper.GetAssemblerInfo(userId.Id, sda);

                    if (resultAssembler.Success)
                    {
                        assemblerInfo = resultAssembler.ReturnObject;
                    }
                    else
                    {
                        throw new Exception("Anahtarcı bilgisi alınamadı.Hata:" + resultAssembler.Result);
                    }
                }

                if (entity.Contains("new_cityid") && entity["new_cityid"] != null)
                {
                    cityId = (EntityReference)entity["new_cityid"];
                }

                if (entity.Contains("new_townid") && entity["new_townid"] != null)
                {
                    townId = (EntityReference)entity["new_townid"];
                }

                if (entity.Contains("new_firstname") && entity["new_firstname"] != null)
                {
                    customerFirstName = entity["new_firstname"].ToString();
                }

                if (entity.Contains("new_lastname") && entity["new_lastname"] != null)
                {
                    customerLastName = entity["new_lastname"].ToString();
                }

                if (entity.Contains("new_mobilephone") && entity["new_mobilephone"] != null)
                {
                    customerMobilePhone = entity["new_mobilephone"].ToString();
                }

                if (!string.IsNullOrWhiteSpace(customerFirstName) && !string.IsNullOrWhiteSpace(customerLastName))
                {
                    customerFullName = customerFirstName + " " + customerLastName;
                }

                if (assemblerInfo != null && !string.IsNullOrWhiteSpace(assemblerInfo.FirstName) && !string.IsNullOrWhiteSpace(assemblerInfo.LastName))
                {
                    assemblerFullName = assemblerInfo.FirstName + " " + assemblerInfo.LastName;
                }

                #endregion

                #region | CREATE SMSs |

                if (!string.IsNullOrWhiteSpace(customerMobilePhone) && !string.IsNullOrWhiteSpace(customerFirstName)
                        && !string.IsNullOrWhiteSpace(customerLastName) && assemblerInfo != null)
                {
                    string customerSmsText = @"Sayın {0}, www.kalekilitesatis.com.tr sitemizden yaptığınız alışveriş için teşekkür ederiz. Montaj için anahtarcı bilgileri: {1}, Tel: {2}";

                    Entity ent = new Entity("new_sms");
                    ent["new_message"] = string.Format(customerSmsText, customerFullName, assemblerFullName, assemblerInfo.MobilePhoneNumber);
                    ent["new_phonenumber"] = customerMobilePhone;
                    ent["new_name"] = customerFirstName + " " + customerLastName + "|Montaj Talebi Mesajı";

                    service.Create(ent);
                }

                if (!string.IsNullOrWhiteSpace(assemblerInfo.MobilePhoneNumber) && !string.IsNullOrWhiteSpace(customerFirstName)
                        && !string.IsNullOrWhiteSpace(customerLastName) && assemblerInfo != null && userId != null)
                {
                    string customerSmsText = @"Sayın {0}, www.kalekilitesatis.com.tr sitemizden satışı yapılan ürünün montajı için müşteri bilgileri: {1} Tel: {2} şeklindedir. Müşterimizle irtibata geçmenizi rica ederiz.";

                    Entity ent = new Entity("new_sms");
                    ent["new_message"] = string.Format(customerSmsText, assemblerFullName, customerFullName, customerMobilePhone);
                    ent["new_phonenumber"] = assemblerInfo.MobilePhoneNumber;
                    ent["new_name"] = customerFirstName + " " + customerLastName + "|Montaj Talebi Mesajı";
                    ent["new_userid"] = userId;

                    service.Create(ent);
                }


                #endregion

                #region | CREATE NPS Survey |

                Entity entSurvey = new Entity("new_npssurvey");
                entSurvey["new_name"] = customerFullName + "|" + DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                entSurvey["new_asseblyrequestid"] = entity.ToEntityReference();

                service.Create(entSurvey);

                #endregion
            }
            catch (Exception ex)
            {
                //LOG
                throw new InvalidPluginExecutionException(ex.Message + "->>>" + ex.StackTrace);
            }
            finally
            {
                if (sda != null)
                    sda.closeConnection();
            }
        }
    }
}
