using GK.Library.Utility;
using GK.ServiceLibrary.GiftServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace GK.ServiceLibrary.GiftServiceLayer
{
    public class SendGiftRequestService : BaseAdminService, ISendGiftRequestService
    {
        public SendGiftRequestService()
            : base()
        {

        }

        public GiftServiceResult SendGiftRequest(string json)
        {
            string requestUrl = "http://kabukistanbul.com/kalekilit/api/query";

            GiftServiceResult returnValue = base.GetResponseOfWebRequest<GiftServiceResult>(requestUrl, json, base.GetCurrentMethod());

            return returnValue;
        }
    }
}

