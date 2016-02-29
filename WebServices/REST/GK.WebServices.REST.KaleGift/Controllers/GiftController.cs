using GK.Library.Business;
using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GK.WebServices.REST.KaleGift.Controllers
{
    public class GiftController : ApiController
    {
        private string API_KEY = "6r9ZTok8zp4yZxKq";

        public string Get()
        {
            return "NOT IMPLETEMENTED";
        }

        public GiftServiceResult Post(GiftUpdateInfo val)
        {
            GiftServiceResult returnValue = new GiftServiceResult();

            if (string.IsNullOrWhiteSpace(val.api_key) || string.IsNullOrWhiteSpace(val.id))
            {
                returnValue.message = "Failure";

                return returnValue;
            }

            if (val.api_key != "6r9ZTok8zp4yZxKq")
            {
                returnValue.message = "Not authorized";

                return returnValue;
            }

            try
            {
                IOrganizationService service = MSCRM.GetOrgService(true);

                UserGiftRequest request = new UserGiftRequest();
                request.Id = new Guid(val.id);

                request.ShippingCompany = val.shipping_company;
                request.ShippingNo = val.shipping_no;
                request.Note = val.note;
                request.ServiceStatus = val.status;

                GiftHelper.UpdateGiftRequest(request, service);

                returnValue.message = "Success";
            }
            catch (Exception ex)
            {
                returnValue.message = "Failure | Ex:" + ex.Message;
            }

            return returnValue;
        }
    }
}
