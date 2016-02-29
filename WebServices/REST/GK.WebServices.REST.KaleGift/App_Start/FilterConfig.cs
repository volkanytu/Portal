using System.Web;
using System.Web.Mvc;

namespace GK.WebServices.REST.KaleGift
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}