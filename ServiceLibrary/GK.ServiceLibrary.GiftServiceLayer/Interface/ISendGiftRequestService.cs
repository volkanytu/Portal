using GK.Library.Utility;
using System;

namespace GK.ServiceLibrary.GiftServiceLayer.Interface
{
    public interface ISendGiftRequestService
    {
        GiftServiceResult SendGiftRequest(string json);
    }
}


