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

namespace GK.WebServices.SOAP.EmosService
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class EmosService : IEmosService
    {
        SqlDataAccess _sda = null;
        IOrganizationService _service = null;

        public MsCrmResult GetToken(string userName, string password)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    _sda = new SqlDataAccess();
                    _sda.openConnection(Globals.ConnectionString);

                    returnValue = LoginHelper.LoginControl(new Guid(Globals.DefaultPortalId), userName, password, _sda);

                    //returnValue.Success = true;

                    if (returnValue.Success)
                    {
                        Guid systemUserId = returnValue.CrmId;

                        _service = MSCRM.GetOrgService(true);
                        string ipAddress = HttpContext.Current.Request.UserHostAddress;

                        MsCrmResult logResult = LoginHelper.LogLogIn(returnValue.CrmId, new Guid(Globals.DefaultPortalId), DateTime.Now, ipAddress, _service);

                        returnValue.Result = Guid.NewGuid().ToString().Replace("-", "");

                        MsCrmResult sessionResult = SetUserSession(returnValue.Result, new Guid(Globals.DefaultPortalId), systemUserId);

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

        public MsCrmResultObj<List<City>> GetCitites(string token)
        {
            MsCrmResultObj<List<City>> returnValue = new MsCrmResultObj<List<City>>();

            LoginSession ls = new LoginSession();

            try
            {
                #region | CHECK SESSION |
                MsCrmResultObj<LoginSession> sessionResult = GetUserSession(token);

                if (!sessionResult.Success)
                {
                    returnValue.Result = sessionResult.Result;
                    return returnValue;
                }
                else
                {
                    ls = sessionResult.ReturnObject;
                }

                #endregion

                _sda = new SqlDataAccess();
                _sda.openConnection(Globals.ConnectionString);

                returnValue = LocationHelper.GetCities(_sda);
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
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

        public MsCrmResultObj<List<Town>> GetTowns(string token, Guid cityId)
        {
            MsCrmResultObj<List<Town>> returnValue = new MsCrmResultObj<List<Town>>();

            LoginSession ls = new LoginSession();

            try
            {
                #region | CHECK SESSION |
                MsCrmResultObj<LoginSession> sessionResult = GetUserSession(token);

                if (!sessionResult.Success)
                {
                    returnValue.Result = sessionResult.Result;
                    return returnValue;
                }
                else
                {
                    ls = sessionResult.ReturnObject;
                }

                #endregion

                _sda = new SqlDataAccess();
                _sda.openConnection(Globals.ConnectionString);

                returnValue = LocationHelper.GetTowns(cityId, _sda);
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
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

        public MsCrmResultObj<List<Town>> GetAllTowns(string token)
        {
            MsCrmResultObj<List<Town>> returnValue = new MsCrmResultObj<List<Town>>();

            LoginSession ls = new LoginSession();

            try
            {
                #region | CHECK SESSION |
                MsCrmResultObj<LoginSession> sessionResult = GetUserSession(token);

                if (!sessionResult.Success)
                {
                    returnValue.Result = sessionResult.Result;
                    return returnValue;
                }
                else
                {
                    ls = sessionResult.ReturnObject;
                }

                #endregion

                _sda = new SqlDataAccess();
                _sda.openConnection(Globals.ConnectionString);

                returnValue = LocationHelper.GetAllTowns(_sda);
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
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

        public MsCrmResultObj<List<AssemblerInfo>> GetAssemblerList(string token, Guid cityId, Guid townId)
        {
            MsCrmResultObj<List<AssemblerInfo>> returnValue = new MsCrmResultObj<List<AssemblerInfo>>();

            LoginSession ls = new LoginSession();

            try
            {
                #region | CHECK SESSION |
                MsCrmResultObj<LoginSession> sessionResult = GetUserSession(token);

                if (!sessionResult.Success)
                {
                    returnValue.Result = sessionResult.Result;
                    return returnValue;
                }
                else
                {
                    ls = sessionResult.ReturnObject;
                }

                #endregion

                _sda = new SqlDataAccess();
                _sda.openConnection(Globals.ConnectionString);

                returnValue = AssemblyRequestHelper.GetAssemblerList(new Guid(Globals.DefaultPortalId), cityId, townId, _sda);
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
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

        public MsCrmResultObj<List<AssemblerInfo>> GetAllAssemblerList(string token)
        {
            MsCrmResultObj<List<AssemblerInfo>> returnValue = new MsCrmResultObj<List<AssemblerInfo>>();

            LoginSession ls = new LoginSession();

            try
            {
                #region | CHECK SESSION |
                MsCrmResultObj<LoginSession> sessionResult = GetUserSession(token);

                if (!sessionResult.Success)
                {
                    returnValue.Result = sessionResult.Result;
                    return returnValue;
                }
                else
                {
                    ls = sessionResult.ReturnObject;
                }

                #endregion

                _sda = new SqlDataAccess();
                _sda.openConnection(Globals.ConnectionString);

                returnValue = AssemblyRequestHelper.GetAllAssemblerList(new Guid(Globals.DefaultPortalId), _sda);
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
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

        public MsCrmResult CreateAsseblyRequest(string token, AssemblyRequestInfo requestInfo)
        {
            MsCrmResult returnValue = new MsCrmResult();

            LoginSession ls = new LoginSession();

            try
            {
                #region | CHECK SESSION |
                MsCrmResultObj<LoginSession> sessionResult = GetUserSession(token);

                if (!sessionResult.Success)
                {
                    returnValue.Result = sessionResult.Result;
                    return returnValue;
                }
                else
                {
                    ls = sessionResult.ReturnObject;
                }

                #endregion

                _service = MSCRM.GetOrgService(true);

                //_sda = new SqlDataAccess();
                //_sda.openConnection(Globals.ConnectionString);

                #region | DATA VALIDATION |

                if (string.IsNullOrWhiteSpace(requestInfo.FirstName))
                {
                    returnValue.Result = "Ad alanı boş olamaz.";

                    return returnValue;
                }

                if (string.IsNullOrWhiteSpace(requestInfo.LastName))
                {
                    returnValue.Result = "Soyad alanı boş olamaz.";

                    return returnValue;
                }

                if (string.IsNullOrWhiteSpace(requestInfo.MobilePhoneNumber))
                {
                    returnValue.Result = "Cep Telefonu alanı boş olamaz.";

                    return returnValue;
                }

                if (!ValidationHelper.CheckTelephoneNumber(requestInfo.MobilePhoneNumber).isFormatOK)
                {
                    returnValue.Result = "Cep Telefonu formatı hatalıdır. Örn:+90-5xx-xxxxxxx şeklinde olmalıdır.";

                    return returnValue;
                }

                if (string.IsNullOrWhiteSpace(requestInfo.EmailAddress))
                {
                    returnValue.Result = "Email adresi alanı boş olamaz.";

                    return returnValue;
                }

                if (string.IsNullOrWhiteSpace(requestInfo.AddressDetail))
                {
                    returnValue.Result = "Adres detayı alanı boş olamaz.";

                    return returnValue;
                }

                if (requestInfo.RequestCityId == null || requestInfo.RequestCityId == Guid.Empty)
                {
                    returnValue.Result = "İl alanı boş olamaz.";

                    return returnValue;
                }

                if (requestInfo.RequestTownId == null || requestInfo.RequestTownId == Guid.Empty)
                {
                    returnValue.Result = "İlçe alanı boş olamaz.";

                    return returnValue;
                }

                #endregion

                requestInfo.Name = requestInfo.Name + "|" + requestInfo.LastName + "|" + DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                returnValue = AssemblyRequestHelper.CreateAssemblyRequest(requestInfo, _service);
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
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

        private MsCrmResult SetUserSession(string token, Guid portalId, Guid portalUserId)
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

        private MsCrmResultObj<LoginSession> GetUserSession(string token)
        {
            MsCrmResultObj<LoginSession> returnValue = new MsCrmResultObj<LoginSession>();

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
