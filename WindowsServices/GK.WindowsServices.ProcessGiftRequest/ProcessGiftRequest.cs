using GK.Library.Business;
using GK.Library.Utility;
using GK.ServiceLibrary.GiftServiceLayer;
using GK.ServiceLibrary.GiftServiceLayer.Interface;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace GK.WindowsServices.ProcessGiftRequest
{
    public partial class ProcessGiftRequest : ServiceBase
    {
        private System.Timers.Timer _timer;
        private const int SERVICE_INTERVAL = 60 * 1000;
        private SqlDataAccess _sda;
        private IOrganizationService _service;

        public ProcessGiftRequest()
        {
            InitializeComponent();
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            FileLogHelper.LogFunction(this.GetType().Name, "Timer Elapsed", @Globals.FileLogPath);

            _timer.Enabled = false;
            _timer.Stop();

            List<Guid> lstPortalIds = PortalHelper.GetPortalList(_sda);

            foreach (Guid portalId in lstPortalIds)
            {
                ProcessRequests(portalId);
            }

            _timer.Enabled = true;
            _timer.Start();
        }

        internal void DebugService(string[] args)
        {
            //interlinkService.OperactiveOrder oo = new interlinkService.OperactiveOrder();

            //interlinkService.sp_WsCatalogProductList_Result[] asd = oo.ProductList("Interlink-Service2q2k", 1875, 1445);

            //string result = oo.OrderAdd("Interlink-Service2q2k", 1445, DateTime.Now.ToLongDateString(), asd[1].UrunKod, 2, "TEST ADDRESS1", "İstanbul", "MecidiyeKöy", "34", "VOLKAN1", "SERTER1"
            //      , string.Empty, string.Empty, "505", "1639659", "VOLKAN1", "O1231", "PUAN KULLANMA1", (int)asd[1].KatalogId, "c1231");

            //bool hasError = GiftHelper.InterlinkOrderErrorCodes.TryGetValue(result, out result);

            ////1049694
            //string orderStatus = oo.OrderFinality("Interlink-Service2q2k", Convert.ToInt32(result), 1445);

            StartOperation();

            _timer_Elapsed(null, null);
        }

        protected override void OnStart(string[] args)
        {
            FileLogHelper.LogFunction(this.GetType().Name, "Service Started", @Globals.FileLogPath);

            StartOperation();
        }

        protected override void OnStop()
        {
            StopOperation();

            FileLogHelper.LogFunction(this.GetType().Name, "Service Stopped", @Globals.FileLogPath);
        }

        private void StartOperation()
        {
            _sda = new SqlDataAccess();
            _sda.openConnection(Globals.ConnectionString);

            _service = MSCRM.GetOrgService(true);

            _timer = new System.Timers.Timer();
            _timer.Interval = SERVICE_INTERVAL;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            _timer.Elapsed += _timer_Elapsed;

            _timer.Start();
        }

        private void StopOperation()
        {
            _timer.Enabled = false;
            _timer.Stop();
        }

        private void ProcessRequests(Guid portalId)
        {
            MsCrmResultObject resultRequestList = GiftHelper.GetGiftReuqestListByStatus(portalId, GiftStatus.Confirmed, _sda);

            if (resultRequestList.Success)
            {
                try
                {
                    List<UserGiftRequest> lstRequests = resultRequestList.GetReturnObject<List<UserGiftRequest>>();

                    FileLogHelper.LogFunction(this.GetType().Name, "RequestCount:" + lstRequests.Count.ToString(), @Globals.FileLogPath);

                    foreach (UserGiftRequest req in lstRequests)
                    {
                        MsCrmResult result = SendToServiceBirIleri(req);

                        if (result.Success)
                        {
                            req.OrderCode = result.Result;
                            req.Status = new OptionSetValueWrapper() { AttributeValue = (int)GiftStatus.ServiceSent };
                        }
                        else
                        {
                            req.ErrorDesc = result.Result;
                            req.Status = new OptionSetValueWrapper() { AttributeValue = (int)GiftStatus.ServiceError };

                            FileLogHelper.LogFunction(this.GetType().Name, "SendToService::" + result.Result, @Globals.FileLogPath);
                        }

                        GiftHelper.UpdateGiftRequest(req, _service);
                    }
                }
                catch (Exception ex)
                {
                    FileLogHelper.LogFunction(this.GetType().Name, ex.Message, @Globals.FileLogPath);
                }
            }
            else
            {
                FileLogHelper.LogFunction(this.GetType().Name, resultRequestList.Result, @Globals.FileLogPath);
            }
        }

        private MsCrmResult SendToService(UserGiftRequest request)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                string cityCode = string.Empty;

                MsCrmResultObject resultGetGiftInfo = GiftHelper.GetGiftInfo(request.GiftId.Id, _sda);
                MsCrmResultObject resultUserInfo = PortalUserHelper.GetPortalUserDetail(request.PortalId.Id, request.UserId.Id, _sda);

                if (!resultGetGiftInfo.Success)
                {
                    returnValue.Result = resultGetGiftInfo.Result;

                    return returnValue;
                }

                if (!resultUserInfo.Success)
                {
                    returnValue.Result = resultUserInfo.Result;

                    return returnValue;
                }

                Gift gift = resultGetGiftInfo.GetReturnObject<Gift>();
                PortalUser userInfo = resultUserInfo.GetReturnObject<PortalUser>();

                StringBuilder sb = new StringBuilder();
                bool checkFailed = false;

                if (userInfo.ContactInfo.CityId == null)
                {
                    sb.AppendLine("İl bilgisi eksik");
                    checkFailed = true;
                }

                if (userInfo.ContactInfo.TownId == null)
                {
                    sb.AppendLine("İlçe bilgisi eksik");
                    checkFailed = true;
                }

                if (string.IsNullOrWhiteSpace(userInfo.ContactInfo.MobilePhone))
                {
                    sb.AppendLine("Cep telefonu bilgisi eksik.");
                    checkFailed = true;
                }

                TelephoneNumber telNo = ValidationHelper.CheckTelephoneNumber(userInfo.ContactInfo.MobilePhone);

                if (!telNo.isFormatOK)
                {
                    sb.AppendLine("Telefon numarası formatı hatalı.");
                    checkFailed = true;
                }


                if (checkFailed)
                {
                    returnValue.Result = sb.ToString();

                    return returnValue;
                }

                cityCode = ContactHelper.GetCityCode(userInfo.ContactInfo.CityId.Id, _sda);

                interlinkService.OperactiveOrder oo = new interlinkService.OperactiveOrder();

                string result = oo.OrderAdd("Interlink-Service2q2k", Globals.InterlinkPartnerId, DateTime.Now.ToLongDateString()
                    , gift.GiftCode, 1, userInfo.ContactInfo.AddressDetail, userInfo.ContactInfo.CityId.Name, userInfo.ContactInfo.TownId.Name, cityCode
                    , userInfo.ContactInfo.FirstName, userInfo.ContactInfo.LastName, string.Empty, string.Empty
                    , telNo.phoneCode, telNo.phoneNo, userInfo.ContactInfo.Title, request.Id.ToString(), "Portal PUAN KULLANIMI"
                    , Globals.InterlinkCatalogId, userInfo.PortalUserId.ToString());

                //result = "0001";

                FileLogHelper.LogFunction(this.GetType().Name, "Result:" + result + ",Id:" + request.Id.ToString(), @Globals.FileLogPath);

                string errorText = string.Empty;

                bool hasError = GiftHelper.InterlinkOrderErrorCodes.TryGetValue(result, out errorText);

                if (string.IsNullOrWhiteSpace(errorText))
                {
                    returnValue.Result = result;
                    returnValue.Success = true;
                }
                else
                {
                    returnValue.Result = errorText;
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.StackTrace;
            }

            return returnValue;
        }

        private MsCrmResult SendToServiceBirIleri(UserGiftRequest request)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {
                string cityCode = string.Empty;

                MsCrmResultObject resultGetGiftInfo = GiftHelper.GetGiftInfo(request.GiftId.Id, _sda);
                MsCrmResultObject resultUserInfo = PortalUserHelper.GetPortalUserDetail(request.PortalId.Id, request.UserId.Id, _sda);

                if (!resultGetGiftInfo.Success)
                {
                    returnValue.Result = resultGetGiftInfo.Result;

                    return returnValue;
                }

                if (!resultUserInfo.Success)
                {
                    returnValue.Result = resultUserInfo.Result;

                    return returnValue;
                }

                Gift gift = resultGetGiftInfo.GetReturnObject<Gift>();
                PortalUser userInfo = resultUserInfo.GetReturnObject<PortalUser>();

                StringBuilder sb = new StringBuilder();
                bool checkFailed = false;

                if (userInfo.ContactInfo.CityId == null)
                {
                    sb.AppendLine("İl bilgisi eksik");
                    checkFailed = true;
                }

                if (userInfo.ContactInfo.TownId == null)
                {
                    sb.AppendLine("İlçe bilgisi eksik");
                    checkFailed = true;
                }

                if (string.IsNullOrWhiteSpace(userInfo.ContactInfo.MobilePhone))
                {
                    sb.AppendLine("Cep telefonu bilgisi eksik.");
                    checkFailed = true;
                }

                TelephoneNumber telNo = ValidationHelper.CheckTelephoneNumber(userInfo.ContactInfo.MobilePhone);

                if (!telNo.isFormatOK)
                {
                    sb.AppendLine("Telefon numarası formatı hatalı.");
                    checkFailed = true;
                }


                if (checkFailed)
                {
                    returnValue.Result = sb.ToString();

                    return returnValue;
                }

                NameValueCollection formData = new NameValueCollection();

                GiftServiceInfo giftServiceInfo = new GiftServiceInfo();
                giftServiceInfo.api_key = "6r9ZTok8zp4yZxKq";
                formData["api_key"] = "6r9ZTok8zp4yZxKq";

                giftServiceInfo.id = request.Id.ToString();
                formData["id"] = request.Id.ToString();

                giftServiceInfo.created_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                formData["created_at"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                giftServiceInfo.name = userInfo.ContactInfo.FirstName;
                formData["name"] = userInfo.ContactInfo.FirstName;

                giftServiceInfo.surname = userInfo.ContactInfo.LastName;
                formData["surname"] = userInfo.ContactInfo.LastName;

                giftServiceInfo.address = userInfo.ContactInfo.AddressDetail;
                formData["address"] = userInfo.ContactInfo.AddressDetail;

                giftServiceInfo.city = userInfo.ContactInfo.CityId.Name;
                formData["city"] = userInfo.ContactInfo.CityId.Name;

                giftServiceInfo.district = userInfo.ContactInfo.TownId.Name;
                formData["district"] = userInfo.ContactInfo.TownId.Name;

                giftServiceInfo.tc = userInfo.ContactInfo.IdentityNumber;
                formData["tc"] = userInfo.ContactInfo.IdentityNumber;

                giftServiceInfo.product_category = gift.CategoryId.Name;
                formData["product_category"] = gift.CategoryId.Name;

                giftServiceInfo.product_name = gift.Name;
                formData["product_name"] = gift.Name;

                giftServiceInfo.product_quantity = "1";
                formData["product_quantity"] = "1";

                giftServiceInfo.tel = userInfo.ContactInfo.MobilePhone;
                formData["tel"] = userInfo.ContactInfo.MobilePhone;

                giftServiceInfo.status = "Servise Gönderildi";
                formData["status"] = "Servise Gönderildi";

                //var js = new JavaScriptSerializer();
                //string json = js.Serialize(giftServiceInfo);

                string formDataStr = SerializePostData(formData);

                ISendGiftRequestService giftReqService = new SendGiftRequestService();

                GiftServiceResult serviceResult = giftReqService.SendGiftRequest(formDataStr);

                FileLogHelper.LogFunction(this.GetType().Name, "Result:" + serviceResult.message + ",Id:" + request.Id.ToString(), @Globals.FileLogPath);

                string errorText = string.Empty;

                if (serviceResult.message.ToLower() == "success")
                {
                    returnValue.Result = serviceResult.message;
                    returnValue.Success = true;
                }
                else
                {
                    returnValue.Result = serviceResult.message;
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.StackTrace;
            }

            return returnValue;
        }

        private string SerializePostData(NameValueCollection formData)
        {
            var parameters = new StringBuilder();

            foreach (string key in formData.Keys)
            {
                parameters.AppendFormat("{0}={1}&",
                    HttpUtility.UrlEncode(key),
                    HttpUtility.UrlEncode(formData[key]));
            }

            return parameters.Remove(parameters.Length - 1, 1).ToString(); // remove the last '&'
        }
    }
}
