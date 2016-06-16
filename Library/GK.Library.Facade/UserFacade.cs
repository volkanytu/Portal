using GK.Library.Business.Interfaces;
using GK.Library.Entities.CrmEntities;
using GK.Library.Entities.CustomEntities;
using GK.Library.Facade.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Facade
{
    public class UserFacade : IUserFacade
    {
        private IUserBusiness _userBusiness;

        public UserFacade(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        public ResponseContainer<SessionData> LoginUser(User userData)
        {
            return _userBusiness.LoginUser(userData);
        }

    }
}
