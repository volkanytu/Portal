using GK.Library.Entities.CrmEntities;
using GK.Library.Entities.CustomEntities;
using GK.Library.Facade.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GK.WebServices.WEBAPI.CrmApi.Controllers
{
    public class UserController : ApiController, IBaseController<User>
    {
        private IUserFacade _userFacade;

        public UserController(IUserFacade userFacade)
        {
            _userFacade = userFacade;
        }

        public List<User> Get()
        {
            throw new NotImplementedException();
        }

        public User Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Library.Entities.CustomEntities.Paged<User> Get(int pageSize, int page)
        {
            throw new NotImplementedException();
        }

        public string Create(User entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(User entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public ResponseContainer<SessionData> LoginUser(User userData)
        {
            return _userFacade.LoginUser(userData);
        }
    }
}
