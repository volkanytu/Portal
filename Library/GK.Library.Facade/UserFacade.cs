using GK.Library.Business.Interfaces;
using GK.Library.Entities.CrmEntities;
using GK.Library.Entities.CustomEntities;
using GK.Library.Facade.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK.Library.Entities;

namespace GK.Library.Facade
{
    public class UserFacade : IUserFacade
    {
        private IUserBusiness _userBusiness;
        private IBaseBusiness<SessionData> _baseSessionBusiness;

        private const int TIMEOUT_MIN = 20;

        public UserFacade(IUserBusiness userBusiness, IBaseBusiness<SessionData> baseSessionBusiness)
        {
            _userBusiness = userBusiness;
            _baseSessionBusiness = baseSessionBusiness;
        }

        public ResponseContainer<SessionData> LoginUser(User userData)
        {
            var result = new ResponseContainer<SessionData>();

            ResponseContainer<bool> loginResult = _userBusiness.CheckLogin(userData);

            if (loginResult.Data)
            {
                var session = CreateSession(userData);
                result.Data = session;

                result.SetSuccess();
            }
            else
            {
                result.SetMessage(ResponseMessageDefinitionEnum.AuthenticationFailure);
            }

            return result;
        }

        private SessionData CreateSession(User userData)
        {
            SessionData session = new SessionData()
            {
                Name = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"),
                ExpireDate = DateTime.Now.AddMinutes(TIMEOUT_MIN),
                IsAuthenticated = true,
                UserId = userData.ToEntityReferenceWrapper()
            };

            _baseSessionBusiness.Insert(session);

            return session;
        }

    }
}
