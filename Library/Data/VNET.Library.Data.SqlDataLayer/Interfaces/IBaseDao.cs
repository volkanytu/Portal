using GK.Library.Entities.CrmEntities;
using System;
using System.Collections.Generic;
namespace GK.Library.Data.SqlDataLayer.Interfaces
{
    public interface IBaseDao<TEntity>
     where TEntity : new()
    {
        void AssociateIn(Guid Id, EntityReferenceWrapper sourceEntity, string relationShipName);
        void AssociateTo(Guid Id, EntityReferenceWrapper targetEntity, string relationShipName);
        void Delete(Guid Id);
        string EntityName { get; }
        TEntity Get(Guid Id);
        List<TEntity> GetList();
        string GetListQuery { get; }
        string GetQuery { get; }
        Guid Insert(TEntity entity);
        void SetState(Guid Id, int stateCode, int statusCode);
        void Update(TEntity entity);
    }
}
