using GK.Library.Entities;
using GK.Library.Entities.CrmEntities;
using GK.Library.Entities.CustomEntities;
using GK.WebServices.WEBAPI.CrmApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace GK.WebServices.WEBAPI.CrmApi.ActionFilter
{
    public class SessionKeyAuthorizeAttribute : AuthorizeAttribute
    {
        public SessionKeyAuthorizeAttribute()
        {

        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, new Response(ResponseMessageDefinitionEnum.AuthorizationFailure));
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext.ControllerContext.Controller.GetType() == typeof(EducationController))
            {
                actionContext.RequestContext.RouteData.Values.Add("test", new Education() { Name = "VOLKAN" });
                return true;
            }

            return false;
        }
    }
}