using GK.Library.Business.Interfaces;
using GK.Library.Data.SqlDataLayer;
using GK.Library.Data.SqlDataLayer.Interfaces;
using GK.Library.Entities.CrmEntities;
using GK.Library.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Business
{
    public abstract class BaseBusiness<TEntity> : IBaseBusiness<TEntity> where TEntity : new()
    {
        private IBaseDao<TEntity> _baseDao;

        public BaseBusiness(IBaseDao<TEntity> baseDao)
        {
            _baseDao = baseDao;
        }

        public Guid Insert(TEntity entity)
        {
            return _baseDao.Insert(entity);
        }

        public void Update(TEntity entity)
        {
            _baseDao.Update(entity);
        }

        public void Delete(Guid Id)
        {
            _baseDao.Delete(Id);
        }

        public virtual TEntity Get(Guid Id)
        {
            return _baseDao.Get(Id);
        }

        public virtual List<TEntity> GetList()
        {
            return _baseDao.GetList();
        }

        public void AssociateTo(Guid Id, EntityReferenceWrapper targetEntity, string relationShipName)
        {
            _baseDao.AssociateTo(Id, targetEntity, relationShipName);
        }

        public void AssociateIn(Guid Id, EntityReferenceWrapper sourceEntity, string relationShipName)
        {
            _baseDao.AssociateIn(Id, sourceEntity, relationShipName);
        }

        public Paged<TEntity> Get(int pageSize, int page)
        {
            List<TEntity> allItems = GetList();

            Paged<TEntity> returnValue = new Paged<TEntity>()
            {
                ItemCount = allItems.Count,
                ItemList = allItems.Skip((page - 1) * pageSize).Take(pageSize).ToList()
            };

            return returnValue;
        }
    }
}
