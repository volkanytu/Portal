using GK.Library.Data.SqlDataLayer;
using GK.Library.Entities.CrmEntities;
using GK.Library.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.Business
{
    public class EducationBusiness : BaseBusiness<Education>
    {
        private BaseDao<Education> _baseDao;

        public EducationBusiness(BaseDao<Education> baseDao)
            : base(baseDao)
        {
            _baseDao = baseDao;
        }

        public Paged<Education> GetByCategoryId(Guid categoryId, int pageSize, int page)
        {
            List<Education> allItems = GetList().Where(x => x.CategoryId.Id == categoryId).ToList();

            Paged<Education> returnValue = new Paged<Education>()
            {
                ItemCount = allItems.Count,
                ItemList = allItems.Skip((page - 1) * pageSize).Take(pageSize).ToList()
            };

            return returnValue;
        }
    }
}
