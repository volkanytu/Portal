using GK.Library.Data.SqlDataLayer;
using GK.Library.Entities.CrmEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Business
{
    public class RoleBusiness : BaseBusiness<Role>
    {
        private BaseDao<Role> _baseDao;

        public RoleBusiness(BaseDao<Role> baseDao)
            : base(baseDao)
        {
            _baseDao = baseDao;
        }
    }
}
