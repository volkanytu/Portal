using GK.Library.Data.SqlDataLayer;
using GK.Library.Entities.CrmEntities;
using GK.Library.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.Business
{
    public class ArticleBusiness : BaseBusiness<Article>
    {
        private BaseDao<Article> _baseDao;

        public ArticleBusiness(BaseDao<Article> baseDao)
            : base(baseDao)
        {
            _baseDao = baseDao;
        }

        public Paged<Article> GetByCategoryId(Guid categoryId, int pageSize, int page)
        {
            List<Article> allItems = GetList().Where(x => x.CategoryId.Id == categoryId).ToList();

            Paged<Article> returnValue = new Paged<Article>()
            {
                ItemCount = allItems.Count,
                ItemList = allItems.Skip((page - 1) * pageSize).Take(pageSize).ToList()
            };

            return returnValue;
        }
    }
}
