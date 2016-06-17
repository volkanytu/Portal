using GK.Library.Entities.CrmEntities;
using GK.Library.Entities.CustomEntities;
using GK.WebServices.WEBAPI.CrmApi.MockData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GK.WebServices.WEBAPI.CrmApi.Controllers
{
    public class PortalController : ApiController, IBaseController<Portal>
    {
        private PortalMock _portalMock;

        public PortalController()
        {
            _portalMock = new PortalMock();
        }

        public List<Portal> Get()
        {
            return _portalMock.GetPortalList();
        }

        public Portal Get(int id)
        {
            return _portalMock.GetPortal();
        }

        public Paged<Portal> Get(int pageSize, int page)
        {
            throw new NotImplementedException();
        }

        public string Create(Portal entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(Portal entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
