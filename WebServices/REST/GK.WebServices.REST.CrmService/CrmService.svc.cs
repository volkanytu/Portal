using GK.Library.Business;
using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using GK.WebServices.REST.CrmService.DataPortApi;
using System.Data;

namespace GK.WebServices.REST.CrmService
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CrmService : ICrmService
    {
        SqlDataAccess sda = null;
        MessageServices mService = null;

        #region | SESSION OPERATIONS |

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

                HttpContext.Current.Session.Add(token, ls);

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
                var session = HttpContext.Current.Session[token];

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

        public MsCrmResultObject GetSmsCodeSession(string token)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            try
            {
                var session = HttpContext.Current.Session[token];

                if (session != null)
                {
                    PasswordSession ls = (PasswordSession)session;

                    returnValue.Success = true;
                    returnValue.Result = "Session bilgisi çekildi.";
                    returnValue.ReturnObject = ls;
                }
                else
                {
                    returnValue.Result = "M095";
                }

            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public string CheckSession(string token)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            returnValue = GetUserSession(token);

            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = ser.Serialize(returnValue);

            return json;
        }

        public MsCrmResult CloseSession(string token)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                HttpContext.Current.Session.Remove(token);

                returnValue.Success = true;
                returnValue.Result = "M092";
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public MsCrmResult SetPasswordSession(string token, string portalUserId, string portalId, string phoneNumber)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                PasswordSession ls = new PasswordSession()
                {
                    Token = token,
                    PortalId = portalId,
                    PortalUserId = portalUserId,
                    PhoneNumber = phoneNumber,
                    SmsCode = SMSHelper.CreateSMSPassword(6)

                };

                HttpContext.Current.Session.Add(token, ls);

                returnValue.Success = true;
                returnValue.Result = ls.SmsCode;
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        #endregion

        string GetSession(SmsConfiguration smsConf)
        {
            string returnValue = string.Empty;

            Session ses = new Session();

            ses.AccountNumber = smsConf.AccountNumber;
            ses.UserName = smsConf.UserName;
            ses.Password = smsConf.Password;

            string session = mService.Register(ses);

            returnValue = session;

            return returnValue;
        }

        SendSmsResult SendSms(string sessionId, SmsConfiguration smsConf, SendSmsRecord sendSms)
        {
            SendSmsResult returnValue = new SendSmsResult();

            smsService.smsservice smsApi = new smsService.smsservice();

            //string[] result = smsApi.SmsInsert_1_N("kalekulup", "cilingir123", null, null, new string[] { sendSms.PhoneNumber }, sendSms.Content);
            string[] result = smsApi.SmsInsert_1_N(smsConf.UserName, smsConf.Password, null, null, new string[] { sendSms.PhoneNumber }, sendSms.Content);

            //SendSMSRequest req = new SendSMSRequest();

            //req.DeleteDate = "";
            //req.GroupID = "0";
            //req.SendDate = "";

            //req.SessionID = sessionId;
            //req.Operator = (Operators)(int)smsConf.Operator;
            //req.Isunicode = Unicode.Yes;
            //req.Orginator = smsConf.Orginator;
            //req.ShortNumber = smsConf.ShortNumber;

            //MessageList mList = new MessageList();

            //List<Content> cList = new List<Content>();
            //List<GSM> gsmList = new List<GSM>();

            //Content cnt = new Content()
            //{
            //    Value = sendSms.Content
            //};

            //GSM gsm = new GSM()
            //{
            //    Value = sendSms.PhoneNumber
            //};


            //cList.Add(cnt);
            //gsmList.Add(gsm);

            //mList.ContentList = cList.ToArray();
            //mList.GSMList = gsmList.ToArray();

            //req.MessageList = mList;
            //SendMessageResponse resp = mService.SendSMS(req);

            returnValue.SendSmsCrmId = sendSms.Id;
            returnValue.StatusCode = result[0];

            //if (resp.Results != null)
            //    returnValue.MessageId = resp.Results[0].MessageID;


            return returnValue;
        }

        public string GetPortal(string url)
        {
            FileLogHelper.LogEvent("VOLKAN TEST", @"C:\DO\");
            Thread.Sleep(1000);
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    MsCrmResult getPortalIdResult = new MsCrmResult();

                    getPortalIdResult = PortalHelper.GetPortalId(url, sda);

                    if (getPortalIdResult.Success)
                    {
                        returnValue = PortalHelper.GetPortalInfo(getPortalIdResult.CrmId, sda);
                    }
                    else
                    {
                        returnValue.Result = getPortalIdResult.Result;
                    }
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!(-GetPortal)";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetPortal";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = ser.Serialize(returnValue);

            return json;
        }

        public string GetPortalInfo(string portalId)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                if (!string.IsNullOrEmpty(portalId))
                {
                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = PortalHelper.GetPortalInfo(new Guid(portalId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetPortalInfo";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetPortalInfo";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = ser.Serialize(returnValue);

            return json;
        }

        public string GetEntityComments(string token, string entityId, string entityName, int start, int end)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = CommentHelper.GetEntityComments(new Guid(entityId), entityName, start, end, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetEntityComments";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        #region | PORTAL USER OPERATIONS |

        public MsCrmResult UpdateWelcomePageGenerated(string token)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    returnValue = PortalUserHelper.UpdateWelcomePageGenerated(ls.PortalUserId, service);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-UpdateWelcomePageGenerated";
                }

            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public MsCrmResult UpdateContractApprove(string token)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    returnValue = PortalUserHelper.UpdateContractApprove(ls.PortalUserId, service);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-UpdateContractApprove";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public string GetUserInfo(string token)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = PortalUserHelper.GetPortalUserDetail(ls.PortalId, ls.PortalUserId, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetUserInfo";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetUserInfo";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public MsCrmResult SendMyPassword(string userName)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = PortalUserHelper.SendContactPassword(userName, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "Kulanıcı adı bilgisi boş!-SendMyPassword";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-SendMyPassword";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }
            return returnValue;
        }

        public MsCrmResult UpdateMyProfile(string token, string newPassword, string oldPassword, Contact contact)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token) && contact != null)
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);
                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = ContactHelper.CreateOrUpdateProfile(contact, service);

                    if (!string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(oldPassword))
                    {
                        returnValue = PortalUserHelper.UpdateUserPassword(ls.PortalUserId, newPassword, oldPassword, service, sda);

                        if (returnValue.Success)
                        {
                            returnValue.Result = "M012"; //"Profile bilgileriniz ve " + returnValue.Result;
                        }
                    }
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Kulanıcı bilgisi boş!-UpdateMyProfile";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-UpdateMyProfile";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }
            return returnValue;
        }

        public MsCrmResult CheckPhoneNumberMatch(string userName, string phoneNumber, string portalId)
        {
            Thread.Sleep(1000);
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(portalId))
                {
                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = PortalUserHelper.CheckPhoneNumberMatch(userName, phoneNumber, portalId, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;

            }
            finally
            {
                if (sda != null)
                    sda.closeConnection();
            }
            return returnValue;
        }

        public MsCrmResult SendPasswordSMS(string portalUserId, string portalId, string phoneNumber, string contactId)
        {
            Thread.Sleep(1000);
            MsCrmResult returnValue = new MsCrmResult();
            string token = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(portalUserId) && !string.IsNullOrEmpty(portalId) && !string.IsNullOrEmpty(phoneNumber))
                {
                    IOrganizationService service = MSCRM.GetOrgService(true);
                    mService = new MessageServices();

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    MsCrmResultObject resultSmsConf = SMSHelper.GetSmsConfigurationInfo(new Guid(Globals.SmsConfigurationDoluHayatId), sda);

                    if (!resultSmsConf.Success)
                    {
                        returnValue.Result = resultSmsConf.Result;
                        return returnValue;
                    }

                    SmsConfiguration smsConf = (SmsConfiguration)resultSmsConf.ReturnObject;

                    token = Guid.NewGuid().ToString().Replace("-", "");// resultSmsRecord.CrmId.ToString().Replace("-", "");

                    returnValue = SetPasswordSession(token, portalUserId, portalId, phoneNumber);

                    if (returnValue.Success)
                    {
                        SendSmsRecord smsRec = new SendSmsRecord()
                        {
                            Name = "Şifre Hatırlatma",
                            PhoneNumber = phoneNumber,
                            SmsConfiguration = smsConf,
                            PrefferedSendDate = DateTime.Now.AddHours(-2),
                            Portal = new EntityReference()
                            {
                                Id = new Guid(portalId),
                                LogicalName = "new_portal"
                            },
                            Contact = new EntityReference("contact", new Guid(contactId)),
                            Content = "Kale Anahtarcılar Kulübü Doğrulama Kodunuz: " + returnValue.Result,
                        };

                        //MsCrmResult resultSmsRecord = SMSHelper.CreateOrUpdateSendSmsRecord(smsRec, service);

                        //if (!resultSmsRecord.Success)
                        //{
                        //    returnValue = resultSmsRecord;
                        //    return returnValue;
                        //}

                        string sessionId = GetSession(smsConf);
                        SendSmsResult resultSms = SendSms(sessionId, smsConf, smsRec);

                        if (!resultSms.StatusCode.Contains("ERR"))
                        {
                            smsRec.IsSent = true;

                            smsRec.MessageId = resultSms.MessageId;
                            smsRec.ResultCode = resultSms.StatusCode;
                            smsRec.StatusCode = 100000000; //İşlendi

                            MsCrmResult resultSmsRecord = SMSHelper.CreateOrUpdateSendSmsRecord(smsRec, service);

                            returnValue = resultSmsRecord;
                            returnValue.Result = token;
                            returnValue.Success = true;
                        }
                        else
                        {
                            smsRec.Error = resultSms.StatusCode;
                            returnValue.Result = resultSms.StatusCode;
                        }
                    }
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003";
                }

            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            finally
            {
                if (sda != null)
                    sda.closeConnection();
            }

            return returnValue;
        }

        public MsCrmResult ConfirmPasswordCode(string token, string code)
        {
            Thread.Sleep(1000);
            MsCrmResult returnValue = new MsCrmResult();
            PasswordSession ls = new PasswordSession();

            try
            {
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(code))
                {
                    MsCrmResultObject sessionResult = GetSmsCodeSession(token);

                    if (!sessionResult.Success)
                    {
                        returnValue.Result = sessionResult.Result;
                        return returnValue;
                    }
                    else
                    {
                        ls = (PasswordSession)sessionResult.ReturnObject;

                        if (ls.SmsCode == code)
                        {
                            returnValue.Success = true;
                            returnValue.Result = "Kod doğrulandı.";
                        }
                        else
                        {
                            returnValue.Result = "CM118";
                        }
                    }
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003";
                }

            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public MsCrmResult UpdateUserPassword(string token, string newPassword)
        {
            MsCrmResult returnValue = new MsCrmResult();
            PasswordSession ls = new PasswordSession();

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(newPassword))
            {
                MsCrmResultObject sessionResult = GetSmsCodeSession(token);

                IOrganizationService service = MSCRM.GetOrgService(true);

                if (!sessionResult.Success)
                {
                    returnValue.Result = sessionResult.Result;
                    return returnValue;
                }
                else
                {
                    ls = (PasswordSession)sessionResult.ReturnObject;

                    returnValue = PortalUserHelper.UpdateUserPassword(new Guid(ls.PortalUserId), newPassword, string.Empty, service, sda);
                }
            }
            else
            {
                returnValue.Success = false;
                returnValue.Result = "M003";
            }

            return returnValue;
        }

        public MsCrmResult RegisterUser(Contact contact)
        {
            MsCrmResult returnValue = new MsCrmResult();

            #region | VALIDATION |

            if (string.IsNullOrWhiteSpace(contact.FirstName))
            {
                returnValue.Result = "Ad alanı boş olamaz";
                return returnValue;
            }

            if (string.IsNullOrWhiteSpace(contact.LastName))
            {
                returnValue.Result = "Soyadı alanı boş olamaz";
                return returnValue;
            }

            if (string.IsNullOrWhiteSpace(contact.MobilePhone))
            {
                returnValue.Result = "Telefon Numarası alanı boş olamaz";
                return returnValue;
            }
            else
            {
                TelephoneNumber telNo = ValidationHelper.CheckTelephoneNumber(contact.MobilePhone);

                if (!telNo.isFormatOK)
                {
                    returnValue.Success = false;
                    returnValue.Result = "Cep telefonu formatı hatalıdır.";

                    return returnValue;
                }
            }

            if (string.IsNullOrWhiteSpace(contact.EmailAddress))
            {
                returnValue.Result = "Email alanı boş olamaz";
                return returnValue;
            }

            if (contact.CityId == null || contact.CityId.Id == Guid.Empty)
            {
                returnValue.Result = "İl alanı boş olamaz";
                return returnValue;
            }

            if (contact.TownId == null || contact.TownId.Id == Guid.Empty)
            {
                returnValue.Result = "İlçe alanı boş olamaz";
                return returnValue;
            }

            if (string.IsNullOrWhiteSpace(contact.AddressDetail))
            {
                returnValue.Result = "Adres detayı alanı boş olamaz.";
                return returnValue;
            }

            #endregion


            try
            {
                IOrganizationService service = MSCRM.GetOrgService(true);

                List<Entity> toPartyList = new List<Entity>();
                List<Entity> fromPartyList = new List<Entity>();

                EntityReference erKamil = new EntityReference("contact", new Guid("D30F27E1-AC2F-E511-80C4-000D3A216510"));
                EntityReference erTest = new EntityReference("contact", new Guid("5B65085C-662E-E511-80C4-000D3A216510"));

                string subject = "Yeni Üyelik Başvurusu";

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("Merhabalar,</br>");
                sb.AppendLine("Yeni üyelik başvurusu bilgileri aşağıdaki gibidir: </br></br>");

                sb.AppendLine("<strong><u>Kişisel Bilgiler</u></strong></br>");
                sb.AppendLine("<strong>Ad:</strong>" + contact.FirstName + "</br>");
                sb.AppendLine("<strong>Soyad:</strong>" + contact.LastName + "</br>");
                sb.AppendLine("<strong>Firma Adı:</strong>" + contact.Title + "</br>");
                sb.AppendLine("<strong>Cinsiyet:</strong>" + (contact.Gender != null ? (contact.Gender == 1 ? "Bay" : "Bayan") : "---") + "</br>");
                sb.AppendLine("<strong>Cep Telefonu:</strong>" + contact.MobilePhone + "</br>");
                sb.AppendLine("<strong>Email:</strong>" + contact.EmailAddress + "</br></br>");

                sb.AppendLine("<strong><u>Adres Bilgileri</u></strong></br>");
                sb.AppendLine("<strong>İl:</strong>" + contact.CityId.Name + "</br>");
                sb.AppendLine("<strong>İlçe:</strong>" + contact.TownId.Name + "</br>");
                sb.AppendLine("<strong>Adres Detayı:</strong>" + contact.AddressDetail + "</br>");

                sb.AppendLine("</br></br></br>");
                sb.AppendLine("İyi çalışmalar...");


                Entity fromParty = new Entity("activityparty");
                fromParty["partyid"] = new EntityReference("systemuser", new Guid(Globals.AdminId));

                fromPartyList.Add(fromParty);

                Entity toParty1 = new Entity("activityparty");
                toParty1["partyid"] = erKamil;

                Entity toParty2 = new Entity("activityparty");
                toParty2["partyid"] = erTest;

                Entity toParty3 = new Entity("activityparty");
                toParty3["addressused"] = "uye@kaleanahtarcilarkulubu.com.tr";

                toPartyList.Add(toParty1);
                toPartyList.Add(toParty2);
                toPartyList.Add(toParty3);

                GeneralHelper.SendMail(Guid.Empty, string.Empty, fromPartyList.ToArray(), toPartyList.ToArray(), subject, sb.ToString(), service);

                returnValue.Success = true;

            }
            catch (Exception ex)
            {

                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        #endregion

        #region | GRAFFITI OPERATIONS |

        public string GetGraffitiList(string token, int commentCount, int start, int end)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = GraffitiHelper.GetGraffities(ls.PortalId, commentCount, start, end, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetGraffitiList";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetGraffitiList";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public string GetGraffitiComments(string token, string graffitiId, int start, int end)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(graffitiId) && !string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = CommentHelper.GetEntityComments(new Guid(graffitiId), "new_graffiti", start, end, sda); //CommentHelper.GetGraffitiComments(new Guid(graffitiId), start, end, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetGraffitiList";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetGraffitiComments";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public MsCrmResult SaveGraffiti(string token, Graffiti graffiti)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (graffiti.HasMedia == false && (string.IsNullOrWhiteSpace(graffiti.Description)))
                {
                    returnValue.Result = "Duvar yazısı paylaşmak için içerik veya resim içeriği eklemelisiniz.";

                    return returnValue;
                }

                if (!string.IsNullOrEmpty(token))
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);
                    returnValue = GraffitiHelper.SaveOrUpdateGraffiti(graffiti, service);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-SaveGraffiti";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-SaveGraffiti";
            }
            return returnValue;
        }

        #endregion

        #region | EDUCATION OPERATIONS |
        public string GetEducationList(string token, string categoryId, int start, int end)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = EducationHelper.GetEducationList(ls.PortalId, ls.PortalUserId, start, end, new Guid(categoryId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetEducationList";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetEducationList";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public string GetEducationInfo(string token, string educationId)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(educationId))
                {
                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = EducationHelper.GetEducationInfo(new Guid(educationId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetEducationInfo";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetEducationInfo";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public MsCrmResult LogEducation(string token, string educationId)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(educationId))
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    returnValue = EducationHelper.LogEducation(ls.PortalUserId, ls.PortalId, new Guid(educationId), service);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-LogEducation";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-LogEducation";
            }
            finally
            {

            }

            return returnValue;
        }

        public string GetEducationCategoryList(string token)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = EducationHelper.GetEducationCategoryList(ls.PortalId, ls.PortalUserId, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetEducationCategoryList";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetEducationCategoryList";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public string GetEducationCategoryInfo(string token, string categoryId)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = EducationHelper.GetEducationCategoryInfo(new Guid(categoryId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetEducationCategoryList";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetEducationCategoryInfo";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        #endregion

        #region | ARTICLE OPERATIONS |

        public string GetarticleList(string token, string categoryId, int start, int end)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = ArticleHelper.GetArticleList(ls.PortalId, ls.PortalUserId, start, end, new Guid(categoryId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetarticleList";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetarticleList";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public string GetArticleInfo(string token, string articleId)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                #region | CHECK SESSION |
                MsCrmResultObject sessionResult = GetUserSession(token);

                if (!sessionResult.Success)
                {
                    json = ser.Serialize(sessionResult);
                    return json;
                }
                else
                {
                    ls = (LoginSession)sessionResult.ReturnObject;
                }

                #endregion

                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(articleId))
                {
                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = ArticleHelper.GetArticleInfo(new Guid(articleId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetArticleInfo";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetArticleInfo";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public MsCrmResult LogArticle(string token, string articleId)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(articleId))
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    returnValue = ArticleHelper.LogArticle(ls.PortalUserId, ls.PortalId, new Guid(articleId), service);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-LogArticle";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-LogArticle";
            }
            finally
            {

            }

            return returnValue;
        }

        public string GetArticleCategoryList(string token)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = ArticleHelper.GetArticleCategoryList(ls.PortalId, ls.PortalUserId, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetArticleCategoryList";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetArticleCategoryList";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public string GetArticleCategoryInfo(string token, string categoryId)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = ArticleHelper.GetArticleCategoryInfo(new Guid(categoryId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetArticleCategoryInfo";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        #endregion

        #region | VIDEOS OPERATIONS |

        public string GetVideoList(string token, string categoryId, int start, int end)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    Guid category = Guid.Empty;

                    if (categoryId != null && categoryId != "")
                    {
                        category = new Guid(categoryId);
                    }

                    returnValue = VideoHelper.GetVideoList(ls.PortalId, ls.PortalUserId, start, end, category, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetVideoList";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetVideoList";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public string GetVideoInfo(string token, string videoId)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(videoId) && !string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = VideoHelper.GetVideoInfo(new Guid(videoId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetVideoInfo";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetVideoInfo";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public MsCrmResult LogVideo(string token, string videoId)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    returnValue = VideoHelper.LogVideo(ls.PortalUserId, ls.PortalId, new Guid(videoId), service);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-LogVideo";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-LogVideo";
            }
            finally
            {

            }

            return returnValue;
        }

        public string GetVideoCategoryList(string token)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = VideoHelper.GetVideoCategoryList(ls.PortalId, ls.PortalUserId, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetVideoCategoryList";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetVideoCategoryList";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public string GetVideoCategoryInfo(string token, string categoryId)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = VideoHelper.GetVideoCategoryInfo(new Guid(categoryId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetVideoCategoryInfo";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        #endregion

        #region | ANNOUNCEMENT OPERATIONS |
        public string GetAnnouncementList(string token, int start, int end)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = AnnouncementHelper.GetAnnouncementList(ls.PortalId, ls.PortalUserId, start, end, sda);

                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetAnnouncementList";
                }

            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetAnnouncementList";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public string GetAnnouncementInfo(string token, string announcementId)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                #region | CHECK SESSION |
                MsCrmResultObject sessionResult = GetUserSession(token);

                if (!sessionResult.Success)
                {
                    json = ser.Serialize(sessionResult);
                    return json;
                }
                else
                {
                    ls = (LoginSession)sessionResult.ReturnObject;
                }

                #endregion

                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(announcementId))
                {
                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = AnnouncementHelper.GetAnnouncementInfo(new Guid(announcementId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetAnnouncementList";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetAnnouncementList";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        #endregion

        #region | FILE UPLOAD OPERATIONS -ASHX'E ÇEKİLDİ |

        public MsCrmResult SaveAnnotation(Annotation note, string relationName)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                IOrganizationService service = MSCRM.GetOrgService(true);

                note.FilePath = Guid.NewGuid() + "." + note.FileName.Split('.')[1].ToLower();
                returnValue = AttachmentFileHelper.CreateAttachmentFile(note, service);

                if (returnValue.Success)
                {

                    byte[] data = Convert.FromBase64String(note.File);
                    File.WriteAllBytes(HostingEnvironment.MapPath("~/../../../Web/GK.Web.GkPortal/attachments") + "/" + note.FilePath, data);
                    //File.WriteAllBytes(HostingEnvironment.MapPath("~files") + "/" + note.FilePath, data);


                    note.AttachmentFile = new EntityReference("new_portal_attachment_file", returnValue.CrmId);
                    returnValue = AttachmentFileHelper.AssociateAttachmentToEntity(returnValue.CrmId, note.Object, relationName, service);
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public MsCrmResult SaveObjectProfileImage(Annotation note, string fieldName)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                IOrganizationService service = MSCRM.GetOrgService(true);

                note.FilePath = Guid.NewGuid() + "." + note.FileName.Split('.')[1].ToLower();

                byte[] data = Convert.FromBase64String(note.File);
                File.WriteAllBytes(HostingEnvironment.MapPath("~/../../../Web/GK.Web.GkPortal/attachments") + "/" + note.FilePath, data);
                //File.WriteAllBytes(HostingEnvironment.MapPath("~files") + "/" + note.FilePath, data);

                Entity ent = new Entity(note.Object.LogicalName.ToLower());
                ent[note.Object.LogicalName.ToLower() + "id"] = note.Object.Id;
                ent[fieldName] = note.FilePath;

                service.Update(ent);

                returnValue.Success = true;
                returnValue.Result = note.FilePath;
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public MsCrmResult DeleteProfileImage(string entityId, string entityName, string fieldName, string fileName)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                IOrganizationService service = MSCRM.GetOrgService(true);

                string filePath = HostingEnvironment.MapPath("~/../../../Web/GK.Web.GkPortal/attachments") + "/" + fileName;

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);

                    Entity ent = new Entity(entityName);
                    ent[entityName + "id"] = new Guid(entityId);
                    ent[fieldName] = null;

                    service.Update(ent);
                }

                returnValue.Success = true;
                returnValue.Result = "Delete success!";
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        #endregion

        #region | QUESTION OPERATIONS |

        public string GetQuestionLevels(string token)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = QuestionHelper.GetQuestionLevels(ls.PortalId, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetQuestionLevels";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetQuestionLevels";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public string GetQuestionInfo(string questionId)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                if (!string.IsNullOrEmpty(questionId))
                {
                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = QuestionHelper.GetQuestionInfo(new Guid(questionId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetQuestionInfo";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetQuestionInfo";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = ser.Serialize(returnValue);

            return json;
        }

        public string SelectQuestion(string token, string questionLevelId)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(questionLevelId))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = QuestionHelper.SelectQuestion(ls.PortalUserId, ls.PortalId, new Guid(questionLevelId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-SelectQuestion";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-SelectQuestion";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public MsCrmResult SaveAnswer(string token, Answer answer)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    if (!answer.IsRefreshOrBack && !answer.IsTimeOverlap && answer.Choice != null && answer.Choice.Id != Guid.Empty)
                    {
                        List<QuestionChoices> choices = (List<QuestionChoices>)QuestionHelper.GetQuestionChoices(answer.Question.Id, sda).ReturnObject;

                        var query = (from a in choices
                                     where
                                     a.IsCorrect == true
                                     &&
                                     a.Id == answer.Choice.Id
                                     select a).ToList();

                        if (query.Count > 0)
                        {
                            answer.IsCorrect = true;
                        }
                        else
                        {
                            answer.IsCorrect = false;
                        }
                    }

                    returnValue = AnswerHelper.SaveOrUpdateAnswer(answer, service);

                    if (returnValue.Success)
                    {
                        if (!answer.IsRefreshOrBack && !answer.IsTimeOverlap && answer.Choice != null && answer.Choice.Id != Guid.Empty)
                        {
                            if (answer.IsCorrect)
                            {
                                returnValue.Result = "Cevabınız doğru " + (answer.IsTrust ? (2 * answer.Point).ToString() : answer.Point.ToString()) + " puan kazandınız!";
                            }
                            else
                            {
                                returnValue.Success = false;
                                returnValue.Result = "Cevabınız yanlış.<br /><strong>" + (answer.IsTrust ? answer.Point.ToString() + " puan kaybettiniz." : string.Empty) + "</strong>";
                            }

                        }
                        else
                        {
                            returnValue.Success = false;
                            returnValue.Result = (answer.IsTimeOverlap ? "<p>Zaman doldu!<br /><strong>" : "Sayfadan çıktınız!<br /><strong>") + (answer.IsTrust ? answer.Point.ToString() + " puan kaybettiniz." : string.Empty) + "</strong></p>";
                        }
                    }
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-SaveAnswer";
                }

            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-SaveAnswer";
            }
            finally
            {
                if (sda != null)
                    sda.closeConnection();
            }
            return returnValue;
        }

        public string GetUserCubeStatus(string token)
        {
            //Thread.Sleep(1000);
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = QuestionHelper.GetUserCubeStatus(ls.PortalUserId, ls.PortalId, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetUserCubeStatus";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetUserCubeStatus";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public string GetCubeStatusList(string token)
        {
            //Thread.Sleep(1000);
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = QuestionHelper.GetCubeStatusList(ls.PortalId, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetCubeStatusList";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetCubeStatusList";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public MsCrmResult HasUserQuestionLimit(string token)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = QuestionHelper.HasUserQuestionLimit(ls.PortalUserId, ls.PortalId, sda);

                    if (!returnValue.Success)
                    {
                        returnValue.Result = "M034"; //"Limitiniz dolmuştur. <br /> İşlem limitleri hakkında bilgi almak için ilgili birim ile görüşünüz.";
                    }
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-HasUserQuestionLimit";
                }

            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-HasUserQuestionLimit";
            }
            finally
            {
                if (sda != null)
                    sda.closeConnection();
            }
            return returnValue;
        }

        public string GetUserPointDetail(string token)
        {
            //Thread.Sleep(1000);
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = ScoreHelper.GetUserScoreDetails(ls.PortalId, ls.PortalUserId, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetCubeStatusList";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetUserPointDetail";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }
        #endregion

        #region | LOGIN OPERATIONS |

        public MsCrmResult CheckLogin(string portalId, string userName, string password)
        {
            //Thread.Sleep(1000);
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(portalId))
                {
                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = LoginHelper.LoginControl(new Guid(portalId), userName, password, sda);

                    if (returnValue.Success)
                    {
                        Guid systemUserId = returnValue.CrmId;

                        IOrganizationService service = MSCRM.GetOrgService(true);
                        string ipAddress = HttpContext.Current.Request.UserHostAddress;

                        MsCrmResult logResult = LoginHelper.LogLogIn(returnValue.CrmId, new Guid(portalId), DateTime.Now, ipAddress, service);

                        if (logResult.Success)
                        {
                            //returnValue.Result = logResult.CrmId.ToString();
                            returnValue.Result = Guid.NewGuid().ToString().Replace("-", "");
                            returnValue.CrmId = logResult.CrmId;

                            MsCrmResult sessionResult = SetUserSession(returnValue.Result, new Guid(portalId), systemUserId);

                            if (!sessionResult.Success)
                            {
                                returnValue.Result = "M036"; //"Oturum bilginiz sisteme işlenemedi";
                            }
                            else
                            {
                                returnValue.Success = true;
                            }
                        }
                    }

                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M037"; //"Kullanıcı adı ve şifre boş olamaz!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-CheckLogin";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }
            return returnValue;
        }

        public MsCrmResult UpdateLogoutDate(string loginLogId)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                if (!string.IsNullOrEmpty(loginLogId))
                {
                    IOrganizationService service = MSCRM.GetOrgService(true);

                    returnValue = LoginHelper.LogLogOut(new Guid(loginLogId), DateTime.Now, service);

                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            finally
            {

            }

            return returnValue;
        }

        #endregion

        #region | FRIENDSHIP OPERATIONS |

        public string GetFriendUserInfo(string token, string userId)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = PortalUserHelper.GetPortalUserDetail(ls.PortalId, new Guid(userId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetFriendUserInfo";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetFriendUserInfo";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public MsCrmResult CreateFriendshipRequest(string token, FriendshipRequest request)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-CreateFriendshipRequest";
                }

                IOrganizationService service = MSCRM.GetOrgService(true);

                returnValue = FriendshipHelper.CreateFriendshipRequest(request, service);
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public MsCrmResult CheckIsUserYourFriend(string token, string selectedUserId)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(selectedUserId))
                {
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

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = FriendshipHelper.CheckIsUserYourFriend(ls.PortalId, ls.PortalUserId, new Guid(selectedUserId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-CheckIsUserYourFriend";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-CheckIsUserYourFriend";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            return returnValue;
        }

        public string HasUserRequestWithYou(string token, string selectedUserId)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(selectedUserId))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion


                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = FriendshipHelper.HasUserRequestWithYou(ls.PortalId, ls.PortalUserId, new Guid(selectedUserId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-HasUserRequestWithYou";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-HasUserRequestWithYou";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public MsCrmResult CloseFriendshipRequest(string token, string requestId, int statusCode)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    returnValue = FriendshipHelper.CloseFriendshipRequest(new Guid(requestId), (FriendshipRequestStatus)statusCode, service);

                    if (returnValue.Success)
                    {
                        FriendshipRequest req = new FriendshipRequest();
                        MsCrmResultObject reqResult = FriendshipHelper.GetFriendshipRequestInfo(new Guid(requestId), sda);

                        if (reqResult.Success && ((FriendshipRequestStatus)statusCode) == FriendshipRequestStatus.Accepted)
                        {
                            req = (FriendshipRequest)reqResult.ReturnObject;

                            Friendship fr = new Friendship();
                            fr.PartyOne = req.From;
                            fr.PartyTwo = req.To;
                            fr.Portal = req.Portal;
                            fr.FriendshipRequest = new EntityReference("new_friendshiprequest", req.Id);

                            returnValue = FriendshipHelper.CreateFriendship(fr, service);
                        }
                    }
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-CloseFriendshipRequest";
                }

            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            finally
            {
                if (sda != null)
                    sda.closeConnection();
            }
            return returnValue;
        }

        public MsCrmResult CloseFriendship(string token, string friendshipId)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    returnValue = FriendshipHelper.CloseFriendship(new Guid(friendshipId), service);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-CloseFriendship";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public string GetUserFriendList(string token)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = FriendshipHelper.GetUserFriendList(ls.PortalId, ls.PortalUserId, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetUserFriendList";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            finally
            {
                if (sda != null)
                    sda.closeConnection();
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        #endregion

        #region | FORUM OPERATIONS |
        public string GetUserForums(string token)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                #region | CHECK SESSION |
                MsCrmResultObject sessionResult = GetUserSession(token);

                if (!sessionResult.Success)
                {
                    json = ser.Serialize(sessionResult);
                    return json;
                }
                else
                {
                    ls = (LoginSession)sessionResult.ReturnObject;
                }

                #endregion

                if (!string.IsNullOrEmpty(token))
                {
                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = ForumHelper.GetUserForums(ls.PortalId, ls.PortalUserId, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetUserForums";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            finally
            {
                if (sda != null)
                    sda.closeConnection();
            }

            json = ser.Serialize(returnValue);

            return json;

        }

        public string GetForumSubjects(string token, string forumId)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                #region | CHECK SESSION |
                MsCrmResultObject sessionResult = GetUserSession(token);

                if (!sessionResult.Success)
                {
                    json = ser.Serialize(sessionResult);
                    return json;
                }
                else
                {
                    ls = (LoginSession)sessionResult.ReturnObject;
                }

                #endregion

                if (!string.IsNullOrEmpty(token))
                {
                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = ForumHelper.GetForumSubjects(new Guid(forumId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetForumSubjects";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            finally
            {
                if (sda != null)
                    sda.closeConnection();
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public string GetForumSubjectInfo(string token, string forumSubjectId)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                #region | CHECK SESSION |
                MsCrmResultObject sessionResult = GetUserSession(token);

                if (!sessionResult.Success)
                {
                    json = ser.Serialize(sessionResult);
                    return json;
                }
                else
                {
                    ls = (LoginSession)sessionResult.ReturnObject;
                }

                #endregion

                if (!string.IsNullOrEmpty(token))
                {
                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = ForumHelper.GetForumSubjectInfo(new Guid(forumSubjectId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetForumSubjectInfo";
                }

            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            finally
            {
                if (sda != null)
                    sda.closeConnection();
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public MsCrmResult CreateForumSubject(string token, ForumSubject forumSubject)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    forumSubject.Portal = new EntityReference("new_portal", ls.PortalId);

                    returnValue = ForumHelper.CreateForumSubject(forumSubject, service);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-CreateForumSubject";

                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }
        #endregion

        #region | PAGE CONTENT OPERATIONS |
        public MsCrmResult GetPageContent(string token, int pageNo)
        {
            //Thread.Sleep(1000);
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = PageContentHelper.GetPageContent(ls.PortalId, (PageNames)pageNo, sda);

                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre hatası.-GetPageContent";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetPageContent";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }
            return returnValue;
        }
        #endregion

        #region | SURVEY OPERATIONS |

        public string SelectSurvey(string token)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = SurveyHelper.SelectSurvey(ls.PortalId, ls.PortalUserId, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-SelectSurvey";
                }

            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public MsCrmResult AnswerSurvey(string token, SurveyAnswer answer)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    returnValue = SurveyHelper.AnswerSurvey(answer, service);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-AnswerSurvey";
                }

            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }
        #endregion

        #region | LIKE OPERATIONS |

        public MsCrmResult LikeEntity(string token, string entityId, string entityName)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = LikeHelper.IsUserLikedBefore(new Guid(entityId), entityName, ls.PortalUserId, sda);

                    if (!returnValue.Success)
                    {
                        IOrganizationService service = MSCRM.GetOrgService(true);

                        returnValue = LikeHelper.LikeEntity(ls.PortalId, ls.PortalUserId, new Guid(entityId), entityName, service);
                    }
                    else
                    {
                        returnValue.Success = false;
                    }
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-LikeEntity";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public MsCrmResult DislikeEntity(string token, string entityId, string entityName)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = LikeHelper.IsUserLikedBefore(new Guid(entityId), entityName, ls.PortalUserId, sda);

                    if (!returnValue.Success)
                    {
                        IOrganizationService service = MSCRM.GetOrgService(true);

                        returnValue = LikeHelper.DislikeEntity(ls.PortalId, ls.PortalUserId, new Guid(entityId), entityName, service);
                    }
                    else
                    {
                        returnValue.Success = false;
                    }
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-LikeEntity";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public MsCrmResult ReportImproperContent(string token, string entityId, string entityName)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    returnValue = LikeHelper.ReportImproperContent(ls.PortalId, ls.PortalUserId, new Guid(entityId), entityName, service);

                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-LikeEntity";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            return returnValue;
        }

        public string GetEntityLikeInfo(string token, string entityId, string entityName)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = LikeHelper.GetEntityLikeInfo(new Guid(entityId), entityName, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetEntityLikeInfo";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            finally
            {
                if (sda != null)
                    sda.closeConnection();
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        #endregion

        public string SearchContact(string token, string key)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = ContactHelper.SearchContact(ls.PortalId, ls.PortalUserId, key, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-SearchContact";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            finally
            {
                if (sda != null)
                    sda.closeConnection();
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public MsCrmResult SaveComment(string token, Comment comment)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    comment.Portal = new EntityReference("new_portal", ls.PortalId);
                    returnValue = CommentHelper.SaveOrUpdateComment(comment, service);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-SaveComment";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-SaveComment";
            }
            return returnValue;
        }

        public string GetAnnotationDetail(string objectId)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();
            try
            {
                if (!string.IsNullOrEmpty(objectId))
                {
                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = AnnotationHelper.GetAnnotationDetailByObject(new Guid(objectId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "Eksik parametre!-GetAnnotationDetail";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetAnnotationDetail";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = ser.Serialize(returnValue);

            return json;
        }

        public MsCrmResult SaveArticle(Article article)
        {
            MsCrmResult returnValue = new MsCrmResult();
            try
            {
                IOrganizationService service = MSCRM.GetOrgService(true);
                returnValue = ArticleHelper.SaveOrUpdateArticle(article, service);
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-SaveArticle";
            }
            return returnValue;
        }

        public List<Annotation> GetAnnotations(string objectId)
        {
            List<Annotation> returnList = new List<Annotation>();
            try
            {
                if (!string.IsNullOrEmpty(objectId))
                {
                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnList = AnnotationHelper.GetAnnotationListByObject(new Guid(objectId), sda);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }
            return returnList;
        }

        public string GetModuleRecordCounts(string token)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = GeneralHelper.GetModuleRecordCount(ls.PortalId, ls.PortalUserId, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message;
            }
            finally
            {
                if (sda != null)
                    sda.closeConnection();
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        #region | POINT CODE OPERATIONS |

        public MsCrmResult UsePointCode(string token, string pointCode)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    MsCrmResultObject resultGetPointCodeInfo = PointCodeHelper.GetPointCodeInfo(pointCode, sda);

                    if (resultGetPointCodeInfo.Success)
                    {
                        PointCode pCode = resultGetPointCodeInfo.GetReturnObject<PointCode>();

                        if (pCode.Status.AttributeValue == 1) //Etkin ise
                        {
                            returnValue = PointCodeHelper.PassiveCode(pCode.Id, service);

                            if (returnValue.Success)
                            {
                                UserCodeUsage userCodeUsage = new UserCodeUsage();
                                userCodeUsage.Name = pointCode;
                                userCodeUsage.Point = pCode.Point;
                                userCodeUsage.PointCodeId = new EntityReferenceWrapper() { Id = pCode.Id, Name = pCode.Name, LogicalName = "new_pointcode" };
                                userCodeUsage.PortalId = new EntityReferenceWrapper() { Id = ls.PortalId, LogicalName = "new_portal" };
                                userCodeUsage.UserId = new EntityReferenceWrapper() { Id = ls.PortalUserId, LogicalName = "new_user" };

                                returnValue = PointCodeHelper.CreateUserCodeUsage(userCodeUsage, service);

                                Score sc = new Score()
                                {
                                    Point = pCode.Point,
                                    Portal = userCodeUsage.PortalId.ToCrmEntityReference(),
                                    User = userCodeUsage.UserId.ToCrmEntityReference(),
                                    ScoreType = ScoreType.PointCode
                                };

                                MsCrmResult scoreRes = ScoreHelper.CreateScore(sc, service);
                            }
                        }
                        else
                        {
                            returnValue.Result = "Girmiş olduğunuz kod önceden kullanılmıştır.";
                        }
                    }
                    else
                    {
                        returnValue.Result = resultGetPointCodeInfo.Result;
                    }
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-CreateForumSubject";

                }

            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        #endregion

        #region | GIFT OPERATIONS |

        public string GetGiftCategoryList(string token)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();

            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = GiftHelper.GetGiftCategoryList(ls.PortalId, ls.PortalUserId, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetGiftCategoryList";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetGiftCategoryList";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public string GetGiftList(string token, string categoryId, string sortType)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = GiftHelper.GetGiftList(ls.PortalId, ls.PortalUserId, new Guid(categoryId), sortType, sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetarticleList";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetGiftList";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public string GetGiftInfo(string token, string giftId)
        {
            MsCrmResultObject returnValue = new MsCrmResultObject();

            LoginSession ls = new LoginSession();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    #region | CHECK SESSION |
                    MsCrmResultObject sessionResult = GetUserSession(token);

                    if (!sessionResult.Success)
                    {
                        json = ser.Serialize(sessionResult);
                        return json;
                    }
                    else
                    {
                        ls = (LoginSession)sessionResult.ReturnObject;
                    }

                    #endregion

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    returnValue = GiftHelper.GetGiftInfo(new Guid(giftId), sda);
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-GetarticleList";
                }
            }
            catch (Exception ex)
            {
                returnValue.Success = false;
                returnValue.Result = ex.Message + "-GetGiftInfo";
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            json = ser.Serialize(returnValue);

            return json;
        }

        public MsCrmResult CreateUserGiftRequest(string token, string giftId)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    MsCrmResultObject resultGetGiftInfo = GiftHelper.GetGiftInfo(new Guid(giftId), sda);

                    MsCrmResultObject resultUserInfo = PortalUserHelper.GetPortalUserDetail(ls.PortalId, ls.PortalUserId, sda);

                    int userPoint = ScoreHelper.GetUserScore(ls.PortalId, ls.PortalUserId, sda);

                    if (resultGetGiftInfo.Success)
                    {
                        Gift gift = resultGetGiftInfo.GetReturnObject<Gift>();

                        PortalUser userInfo = resultUserInfo.GetReturnObject<PortalUser>();

                        TelephoneNumber telNo = ValidationHelper.CheckTelephoneNumber(userInfo.ContactInfo.MobilePhone);

                        if (!telNo.isFormatOK)
                        {
                            returnValue.Success = false;
                            returnValue.Result = "Cep telefonu bilginiz eksik veya formatı hatalıdır. Profilim bölümünden Cep telefpnu bilginizi güncelleyebilirsiniz.";

                            return returnValue;
                        }

                        #region | POINT CONTROL |
                        if (userPoint < gift.Point)
                        {
                            returnValue.Success = false;
                            returnValue.Result = "Mevcut puanınız: " + userPoint.ToString() + ", hediye talebi için yeterli değildir.";

                            return returnValue;
                        }
                        #endregion

                        #region | ADDRESS CONTROL |
                        if (userInfo.ContactInfo.CityId == null || userInfo.ContactInfo.TownId == null || string.IsNullOrEmpty(userInfo.ContactInfo.AddressDetail))
                        {
                            returnValue.Success = false;
                            returnValue.Result = "Adres bilgilerinizin tam olması gerekmektedir. Lütfen Profilim sayfasından Adres Bilgilerinizi güncelleyiniz";

                            return returnValue;
                        }
                        #endregion

                        UserGiftRequest req = new UserGiftRequest();
                        req.Name = gift.Name + "-" + DateTime.Now.ToString("dd.MM.yyyy");
                        req.PortalId = new EntityReferenceWrapper() { Id = ls.PortalId, LogicalName = "new_portal" };
                        req.UserId = new EntityReferenceWrapper() { Id = ls.PortalUserId, LogicalName = "new_user" };
                        req.GiftId = new EntityReferenceWrapper() { Id = gift.Id, Name = gift.Name, LogicalName = "new_gift" };
                        req.Point = gift.Point;

                        returnValue = GiftHelper.CreateUserGiftRequest(req, service);

                        if (returnValue.Success)
                        {
                            Score sc = new Score()
                            {
                                Point = gift.Point * -1,
                                Portal = req.PortalId.ToCrmEntityReference(),
                                User = req.UserId.ToCrmEntityReference(),
                                ScoreType = ScoreType.UsePoint
                            };

                            MsCrmResult scoreRes = ScoreHelper.CreateScore(sc, service);
                        }

                    }
                    else
                    {
                        returnValue.Result = resultGetGiftInfo.Result;
                    }
                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!-CreateForumSubject";

                }

            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        #endregion

        #region | MESSAGE OPERATIONS |

        public List<MessageInfo> GetOldMessages(string portalId, string from, string to)
        {
            List<MessageInfo> messageInfos = new List<MessageInfo>();
            try
            {
                if (!string.IsNullOrEmpty(portalId))
                {
                    this.sda = new SqlDataAccess();
                    this.sda.openConnection(Globals.ConnectionString);
                    messageInfos = MessageHelper.GetMessagesBetweenUsers(new Guid(portalId), new Guid(from), new Guid(to), this.sda);
                }
            }
            finally
            {
                if (this.sda != null)
                {
                    this.sda.closeConnection();
                }
            }
            return messageInfos;
        }

        public MsCrmResult UpdateMessageAsSeen(string messageId)
        {
            MsCrmResult msCrmResult = new MsCrmResult();
            try
            {
                MessageInfo mInfo = new MessageInfo()
                {
                    Id = new Guid(messageId),
                    StatusCode = new OptionSetValueWrapper()
                    {
                        AttributeValue = (int)MessageInfo.Status.Seen,
                        Value = "Görüldü"
                    }
                };

                IOrganizationService orgService = MSCRM.GetOrgService(true, null, null);
                msCrmResult = MessageHelper.UpdateMessage(mInfo, orgService);
            }
            catch (Exception exception)
            {
                msCrmResult.Result = exception.Message;
            }
            return msCrmResult;
        }

        public string GetUserRecentContacts(string token)
        {
            List<MessageInfo> userRecentMessages = null;

            LoginSession loginSession = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    MsCrmResultObject userSession = this.GetUserSession(token);
                    if (userSession.Success)
                    {
                        loginSession = (LoginSession)userSession.ReturnObject;

                        sda = new SqlDataAccess();
                        sda.openConnection(Globals.ConnectionString);

                        userRecentMessages = MessageHelper.GetUserRecentContacts(loginSession.PortalId, loginSession.PortalUserId, sda);
                    }
                }
            }
            finally
            {
                if (this.sda != null)
                {
                    this.sda.closeConnection();
                }
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();

            string json = ser.Serialize(userRecentMessages);

            return json;
        }

        public List<SelectValue> SearchUserTokenize(string token, string search)
        {
            List<SelectValue> selectValues = null;

            LoginSession loginSession = new LoginSession();
            try
            {
                try
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        MsCrmResultObject userSession = this.GetUserSession(token);
                        if (userSession.Success)
                        {
                            loginSession = (LoginSession)userSession.ReturnObject;

                            sda = new SqlDataAccess();
                            sda.openConnection(Globals.ConnectionString);

                            string str = loginSession.PortalId.ToString();
                            Guid portalUserId = loginSession.PortalUserId;

                            selectValues = FriendshipHelper.SearchFriend(str, portalUserId.ToString(), search, sda);
                        }
                        else
                        {
                            return selectValues;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            return selectValues;
        }

        public MessageInfo CreateMessage(string token, MessageInfo message)
        {
            message.Content = System.Uri.UnescapeDataString(message.Content);
            message.Name = System.Uri.UnescapeDataString(message.FromId.Name) + "|" + System.Uri.UnescapeDataString(message.ToId.Name);

            MessageInfo messageInfo = new MessageInfo();
            LoginSession loginSession = new LoginSession();

            try
            {
                try
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        IOrganizationService orgService = MSCRM.GetOrgService(true, null, null);
                        this.sda = new SqlDataAccess();
                        this.sda.openConnection(Globals.ConnectionString);

                        MsCrmResult msCrmResult = MessageHelper.CreateMessage(message, orgService);

                        if (msCrmResult.Success)
                        {
                            messageInfo = MessageHelper.GetMessageInfo(msCrmResult.CrmId, this.sda);
                        }
                    }
                }
                catch (Exception exception)
                {
                }
            }
            finally
            {
                if (this.sda != null)
                {
                    this.sda.closeConnection();
                }
            }
            return messageInfo;
        }

        public MsCrmResultObj<List<Message>> GetUnReadMessages(string token, string requestId)
        {
            MsCrmResultObj<List<Message>> returnValue = new MsCrmResultObj<List<Message>>();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token) || !string.IsNullOrEmpty(requestId))
                {
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

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);


                    List<Message> lstMessages = MessageHelper.GetUnReadMessages(ls.PortalUserId, sda);

                    returnValue.ReturnObject = lstMessages;

                    returnValue.Success = true;

                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            return returnValue;
        }


        #endregion

        #region | ADDRESS OPERATIONS |

        public List<EntityReference> GetCities()
        {
            List<EntityReference> returnValue = new List<EntityReference>();

            try
            {
                sda = new SqlDataAccess();
                sda.openConnection(Globals.ConnectionString);

                #region | SQL QUERY |

                string sqlQuery = @"SELECT
                                    c.new_cityId AS Id
                                    ,c.new_name AS Name
                                    ,'new_city' AS LogicalName
                                FROM
                                new_city AS c (NOLOCK)";
                #endregion

                DataTable dt = sda.getDataTable(sqlQuery);

                returnValue = dt.ToList<EntityReference>();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (sda != null)
                    sda.closeConnection();
            }

            return returnValue;
        }

        public List<EntityReference> GetTowns(string cityId)
        {
            List<EntityReference> returnValue = new List<EntityReference>();

            try
            {
                sda = new SqlDataAccess();
                sda.openConnection(Globals.ConnectionString);

                #region | SQL QUERY |

                string sqlQuery = @"SELECT
                                    t.new_townId AS Id
                                    ,t.new_name AS Name
                                    ,'new_town' AS LogicalName
                                FROM
                                new_town AS t (NOLOCK) WHERE t.new_cityId='{0}'";
                #endregion

                DataTable dt = sda.getDataTable(string.Format(sqlQuery, cityId));

                returnValue = dt.ToList<EntityReference>();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (sda != null)
                    sda.closeConnection();
            }

            return returnValue;
        }
        #endregion

        #region | DISCOVERY FORM OPERATIONS |

        public MsCrmResult SaveDiscoveryForm(string token, DiscoveryForm discForm)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    #region | VALIDATION |

                    if (string.IsNullOrEmpty(discForm.FirstName))
                    {
                        returnValue.Result = "Ad alanı boş olamaz";
                        return returnValue;
                    }

                    if (string.IsNullOrEmpty(discForm.LastName))
                    {
                        returnValue.Result = "Soyadı alanı boş olamaz";
                        return returnValue;
                    }

                    if (string.IsNullOrEmpty(discForm.PhoneNumber))
                    {
                        returnValue.Result = "Telefon Numarası alanı boş olamaz";
                        return returnValue;
                    }

                    if (string.IsNullOrEmpty(discForm.Email))
                    {
                        returnValue.Result = "Email alanı boş olamaz";
                        return returnValue;
                    }

                    if (discForm.CityId == null || discForm.CityId.Id == Guid.Empty)
                    {
                        returnValue.Result = "İl alanı boş olamaz";
                        return returnValue;
                    }

                    if (discForm.TownId == null || discForm.TownId.Id == Guid.Empty)
                    {
                        returnValue.Result = "İlçe alanı boş olamaz";
                        return returnValue;
                    }

                    //if (discForm.HomeType == null || discForm.HomeType.AttributeValue == null || discForm.HomeType.AttributeValue == 0)
                    //{
                    //    returnValue.Result = "Konut Tipi alanı boş olamaz";
                    //    return returnValue;
                    //}

                    //if (discForm.VisitDate == null)
                    //{
                    //    returnValue.Result = "En Uygun Tarih alanı boş olamaz";
                    //    return returnValue;
                    //}

                    //if (discForm.VisitHour == null || discForm.VisitHour.AttributeValue == null || discForm.VisitHour.AttributeValue == 0)
                    //{
                    //    returnValue.Result = "En Uygun Saat alanı boş olamaz";
                    //    return returnValue;
                    //}

                    //if (discForm.InformedBy == null || discForm.InformedBy.AttributeValue == null || discForm.InformedBy.AttributeValue == 0)
                    //{
                    //    returnValue.Result = "Kale 7/24'ü nereden duydunuz? alanı boş olamaz";
                    //    return returnValue;
                    //}

                    #endregion

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    discForm.Name = discForm.UserId.Name + "|" + DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                    returnValue = DiscoveryFormHelper.CreateDiscoveryForm(discForm, service);

                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        public MsCrmResultObj<List<DiscoveryForm>> GetUserDiscoveryFormList(string token)
        {
            MsCrmResultObj<List<DiscoveryForm>> returnValue = new MsCrmResultObj<List<DiscoveryForm>>();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);


                    returnValue = DiscoveryFormHelper.GetUserDiscoveryFormList(ls.PortalUserId, sda);

                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            return returnValue;
        }
        #endregion

        #region | ASSEMBLY REQUESTS |

        public MsCrmResultObj<List<AssemblyRequestInfo>> GetRequests(string token, string userId)
        {
            MsCrmResultObj<List<AssemblyRequestInfo>> returnValue = new MsCrmResultObj<List<AssemblyRequestInfo>>();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token) || !string.IsNullOrEmpty(userId))
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);


                    returnValue = AssemblyRequestHelper.GetAssemblyRequestList(new Guid(userId), sda);

                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            return returnValue;
        }

        public MsCrmResultObj<AssemblyRequestInfo> GetRequestInfo(string token, string requestId)
        {
            MsCrmResultObj<AssemblyRequestInfo> returnValue = new MsCrmResultObj<AssemblyRequestInfo>();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token) || !string.IsNullOrEmpty(requestId))
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);


                    returnValue = AssemblyRequestHelper.GetAssemblyRequestInfo(new Guid(requestId), sda);

                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            return returnValue;
        }

        public MsCrmResult CompleteRequest(string token, string requestId)
        {
            MsCrmResult returnValue = new MsCrmResult();
            LoginSession ls = new LoginSession();

            try
            {
                if (!string.IsNullOrEmpty(token) || !string.IsNullOrEmpty(requestId))
                {
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

                    IOrganizationService service = MSCRM.GetOrgService(true);

                    sda = new SqlDataAccess();
                    sda.openConnection(Globals.ConnectionString);

                    AssemblyRequestInfo req = new AssemblyRequestInfo()
                    {
                        Id = new Guid(requestId),
                        StatusCode = new OptionSetValueWrapper()
                        {
                            AttributeValue = (int)AssemblyRequestStatus.Completed
                        }
                    };

                    returnValue = AssemblyRequestHelper.UpdateAssemblyRequest(req, service);

                }
                else
                {
                    returnValue.Success = false;
                    returnValue.Result = "M003"; //"Eksik parametre!";
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.Message;
            }
            finally
            {
                if (sda != null)
                {
                    sda.closeConnection();
                }
            }

            return returnValue;
        }

        public MsCrmResult AnswerNpsSurvey(string npsSurveyId, int suggest, int suggestPoint)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                IOrganizationService service = MSCRM.GetOrgService(true);

                returnValue = AssemblyRequestHelper.AnswerNpsSurvey(new Guid(npsSurveyId), suggest, suggestPoint, service);
            }
            catch (Exception ex)
            {
                returnValue.HasException = true;
                returnValue.Result = ex.Message;
            }

            return returnValue;
        }

        #endregion
    }
}
