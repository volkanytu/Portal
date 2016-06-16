using GK.Library.Data.SqlDataLayer;
using GK.Library.Entities.CrmEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Business
{
    public class ContactBusiness : BaseBusiness<Contact>
    {
        private BaseDao<Contact> _baseDao;

        public ContactBusiness(BaseDao<Contact> baseDao)
            : base(baseDao)
        {
            _baseDao = baseDao;
        }
    }
}
