using GK.Library.Business.Interfaces;
using GK.Library.Data.SqlDataLayer;
using GK.Library.Data.SqlDataLayer.Interfaces;
using GK.Library.Entities;
using GK.Library.Entities.CrmEntities;
using GK.Library.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Business
{
    public class UserBusiness : BaseBusiness<User>, IUserBusiness
    {
        private IBaseDao<User> _baseUserDao;
        private IBaseDao<SessionData> _baseSessionDao;
        private IUserDao _userDao;

        private const int TIMEOUT_MIN = 20;

        public UserBusiness(IBaseDao<User> baseUserDao, IBaseDao<SessionData> baseSessionDao, IUserDao userDao)
            : base(baseUserDao)
        {
            _baseUserDao = baseUserDao;
            _baseSessionDao = baseSessionDao;
            _userDao = userDao;
        }

        public ResponseContainer<SessionData> LoginUser(User userData)
        {
            var result = new ResponseContainer<SessionData>();

            var user = _userDao.GetByName(userData.Name);
            if (user != null && user.Password == userData.Password)
            {
                var session = CreateSession(userData);
                result.Data = session; ;

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

            _baseSessionDao.Insert(session);

            return session;
        }
    }
}
