using GK.Library.Business;
using GK.Library.Business.Interfaces;
using GK.Library.Entities.CrmEntities;
using GK.Library.Entities.CustomEntities;
using GK.WebServices.WEBAPI.CrmApi.MockData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using DependencyResolver = System.Web.Mvc.DependencyResolver;

namespace GK.WebServices.WEBAPI.CrmApi.Controllers
{
    public class EducationController : ApiController, IBaseController<Education>
    {
        private EducationMock _educationMock;
        private IBaseBusiness<Portal> _basePortalBusiness;

        public EducationController(IBaseBusiness<Portal> basePortalBusiness)
        {
            _basePortalBusiness = basePortalBusiness;
        }

        [HttpGet]
        public List<Education> Get()
        {
            _basePortalBusiness.Get(1, 2);

            Education test = (Education)RequestContext.RouteData.Values["Test"];
            return _educationMock.GetEducationList();
        }

        [HttpGet]
        public Paged<Education> Get(int pageSize, int page)
        {
            List<Education> eduList = _educationMock.GetEducationList();

            Paged<Education> returnValue = new Paged<Education>();
            returnValue.ItemCount = eduList.Count;
            returnValue.ItemList = eduList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return returnValue;
        }

        [HttpGet]
        public Education Get(Guid id)
        {
            return _educationMock.GetEducationList().FirstOrDefault();
        }

        [HttpPost]
        public string Create(Education entity)
        {
            return entity.Name;
        }

        [HttpPut]
        public bool Update(Education entity)
        {
            return true;
        }

        [HttpDelete]
        public bool Delete(Guid id)
        {
            return true;
        }
    }
}
