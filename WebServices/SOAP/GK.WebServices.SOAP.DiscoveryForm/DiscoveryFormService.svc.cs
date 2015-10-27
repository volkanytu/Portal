using GK.Library.Business;
using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace GK.WebServices.SOAP.DiscoveryFormService
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DiscoveryFormService : IDiscoveryFormService
    {
        SqlDataAccess _sda = null;
        IOrganizationService _service = null;

        public MsCrmResult ConfirmForm(string token, int formCode)
        {
            MsCrmResult returnValue = new MsCrmResult();

            LoginSession ls = new LoginSession();

            try
            {
                _sda = new SqlDataAccess();
                _sda.openConnection(Globals.ConnectionString);

                #region | CHECK SESSION |
                MsCrmResultObject sessionResult = GetUserSession(token);

                if (!sessionResult.Success)
                {
                    returnValue.Result = sessionResult.Result;
                    return returnValue;
                }
                else
                {
                    ls = (LoginSession)sessionResult.ReturnObject;
                }

                #endregion

                MsCrmResultObject resultFormInfo = DiscoveryFormHelper.GetDiscoveryFormInfo(formCode, _sda);

                if (resultFormInfo.Success)
                {
                    DiscoveryForm formInfo = (DiscoveryForm)resultFormInfo.ReturnObject;

                    formInfo.Status = new OptionSetValueWrapper() { AttributeValue = (int)DiscoveryFormStatus.LotusConfirmed };
                }
                else
                {
                    returnValue.Result = resultFormInfo.Result;
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public MsCrmResult GetToken(string portalId, string userName, string password)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(portalId))
                {
                    _sda = new SqlDataAccess();
                    _sda.openConnection(Globals.ConnectionString);

                    returnValue = LoginHelper.LoginControl(new Guid(portalId), userName, password, _sda);

                    //returnValue.Success = true;

                    if (returnValue.Success)
                    {
                        Guid systemUserId = returnValue.CrmId;

                        IOrganizationService service = MSCRM.GetOrgService(true);
                        string ipAddress = HttpContext.Current.Request.UserHostAddress;

                        MsCrmResult logResult = LoginHelper.LogLogIn(returnValue.CrmId, new Guid(portalId), DateTime.Now, ipAddress, service);

                        returnValue.Result = Guid.NewGuid().ToString().Replace("-", "");

                        MsCrmResult sessionResult = SetUserSession(returnValue.Result, new Guid(portalId), systemUserId);

                        if (!sessionResult.Success)
                        {
                            return sessionResult;
                        }
                    }
                }
                else
                {
                    returnValue.Result = "Eksik Parametre.";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            finally
            {
                if (_sda != null)
                {
                    _sda.closeConnection();
                }
            }

            return returnValue;
        }

        public MsCrmResult SetUserSession(string token, Guid portalId, Guid portalUserId)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                LoginSession ls = new LoginSession()
                {
                    Token = token,
                    PortalId = portalId,
                    PortalUserId = portalUserId
                };

                HttpContext.Current.Cache.Add(token, ls, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 20, 0), System.Web.Caching.CacheItemPriority.Normal, null);

                returnValue.Success = true;
                returnValue.Result = "Oturum bilgileri işlendi";
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public MsCrmResultObject GetUserSession(string token)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            try
            {
                var session = HttpContext.Current.Cache[token];

                if (session != null)
                {
                    LoginSession ls = (LoginSession)session;

                    returnValue.Success = true;
                    returnValue.Result = "Session bilgisi çekildi.";
                    returnValue.ReturnObject = ls;
                }
                else
                {
                    returnValue.Result = "M001";
                }

            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }
    }
}
