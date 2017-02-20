using GK.Library.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace GK.WebServices.SOAP.EmosService
{
    [ServiceContract]
    public interface IEmosService
    {
        [OperationContract]
        MsCrmResult GetToken(string userName, string password);

        [OperationContract]
        MsCrmResultObj<List<City>> GetCitites(string token);

        [OperationContract]
        MsCrmResultObj<List<Town>> GetTowns(string token, Guid cityId);

        [OperationContract]
        MsCrmResultObj<List<AssemblerInfo>> GetAssemblerList(string token, Guid cityId, Guid townId);

        [OperationContract]
        MsCrmResult CreateAsseblyRequest(string token, AssemblyRequestInfo requestInfo);

        [OperationContract]
        MsCrmResultObj<List<Town>> GetAllTowns(string token);

        [OperationContract]
        MsCrmResultObj<List<AssemblerInfo>> GetAllAssemblerList(string token);

        [OperationContract]
        MsCrmResultObj<List<AssemblerInfo>> GetAllMemberList(string token);

        [OperationContract]
        MsCrmResultObj<AssemblerInfo> GetUser(string token, string emailaddress, string password);
    }
}
