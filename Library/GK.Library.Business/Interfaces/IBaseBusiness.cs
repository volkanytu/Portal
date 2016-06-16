using System;
namespace GK.Library.Business.Interfaces
{
    public interface IBaseBusiness<TEntity>
     where TEntity : new()
    {
        void AssociateIn(Guid Id, GK.Library.Entities.CrmEntities.EntityReferenceWrapper sourceEntity, string relationShipName);
        void AssociateTo(Guid Id, GK.Library.Entities.CrmEntities.EntityReferenceWrapper targetEntity, string relationShipName);
        void Delete(Guid Id);
        TEntity Get(Guid Id);
        GK.Library.Entities.CustomEntities.Paged<TEntity> Get(int pageSize, int page);
        System.Collections.Generic.List<TEntity> GetList();
        Guid Insert(TEntity entity);
        void Update(TEntity entity);
    }
}
