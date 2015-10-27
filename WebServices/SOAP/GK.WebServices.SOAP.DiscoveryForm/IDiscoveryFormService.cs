using GK.Library.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace GK.WebServices.SOAP.DiscoveryFormService
{
    [ServiceContract]
    public interface IDiscoveryFormService
    {
        [OperationContract]
        MsCrmResult ConfirmForm(string token, int formCode);

        [OperationContract]
        MsCrmResult GetToken(string portalId, string userName, string password);
    }
}
