using GK.Library.ConfigManager.Interfaces;
using GK.Library.Entities.CrmEntities;
using GK.Library.Entities.CustomEntities;
using GK.Library.Facade.Interfaces;
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
        private IPortalFacade _portalFacade;
        private IConfigs _configs;

        public PortalController(IPortalFacade portalFacade, IConfigs configs)
        {
            _portalFacade = portalFacade;
            _configs = configs;

            _portalMock = new PortalMock();
        }

        public List<Portal> Get()
        {
            throw new NotImplementedException();
            //return _portalMock.GetPortalList();
        }

        public Portal Get(Guid id)
        {
            return _portalFacade.Get(id);

            //return _portalMock.GetPortal();
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

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
