using GK.Library.Data.SqlDataLayer.Interfaces;
using GK.Library.Entities.CrmEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Business
{
    public class SessionBusiness : BaseBusiness<SessionData>
    {
        private IBaseDao<SessionData> _baseSessionDao;

        public SessionBusiness(IBaseDao<SessionData> baseSessionDao)
            : base(baseSessionDao)
        {
            _baseSessionDao = baseSessionDao;
        }
    }
}
