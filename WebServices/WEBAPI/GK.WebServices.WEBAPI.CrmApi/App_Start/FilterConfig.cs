using GK.WebServices.WEBAPI.CrmApi.ActionFilter;
using System.Web;
using System.Web.Mvc;

namespace GK.WebServices.WEBAPI.CrmApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
