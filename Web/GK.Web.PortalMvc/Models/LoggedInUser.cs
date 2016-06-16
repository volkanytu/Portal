using GK.Library.Entities.CrmEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GK.Web.PortalMvc.Models
{
    public class LoggedInUser
    {
        public static SessionData Current
        {
            get { return (SessionData)HttpContext.Current.Session["User"]; }
            set { HttpContext.Current.Session["User"] = value; }
        }
    }
}