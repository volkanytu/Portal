using GK.Library.Data.SqlDataLayer;
using GK.Library.Data.SqlDataLayer.Interfaces;
using GK.Library.Entities.CrmEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Business
{
    public class PortalBusiness : BaseBusiness<Portal>
    {
        private IBaseDao<Portal> _basePortalDao;

        public PortalBusiness(IBaseDao<Portal> basePortalDao)
            : base(basePortalDao)
        {
            _basePortalDao = basePortalDao;
        }
    }
}
