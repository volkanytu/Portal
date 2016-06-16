using GK.Library.Data.SqlDataLayer;
using GK.Library.Entities.CrmEntities;
using GK.Library.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.Business
{
    public class VideoBusiness : BaseBusiness<Video>
    {
        private BaseDao<Video> _baseDao;

        public VideoBusiness(BaseDao<Video> baseDao)
            : base(baseDao)
        {
            _baseDao = baseDao;
        }

        public Paged<Video> GetByCategoryId(Guid categoryId, int pageSize, int page)
        {
            List<Video> allItems = GetList().Where(x => x.CategoryId.Id == categoryId).ToList();

            Paged<Video> returnValue = new Paged<Video>()
            {
                ItemCount = allItems.Count,
                ItemList = allItems.Skip((page - 1) * pageSize).Take(pageSize).ToList()
            };

            return returnValue;
        }
    }
}
