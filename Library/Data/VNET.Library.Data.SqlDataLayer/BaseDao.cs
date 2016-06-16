using GK.Library.Data.SqlDataLayer.Interfaces;
using GK.Library.Entities.CrmEntities;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Data.SqlDataLayer
{
    public abstract class BaseDao<TEntity> : IBaseDao<TEntity> where TEntity : new()
    {
        private IMsCrmAccess _msCrmAccess;
        private ISqlAccess _sqlAccess;
        //private string _entityName;
        //private string _getQuery;
        //private string _getListQuery;

        public BaseDao(IMsCrmAccess msCrmAccess, ISqlAccess sqlAccess)
        {
            _msCrmAccess = msCrmAccess;
            _sqlAccess = sqlAccess;
            //_entityName = entityName;
            //_getQuery = getQuery;
            //_getListQuery = getListQuery;
        }

        public Guid Insert(TEntity entity)
        {
            IOrganizationService service = _msCrmAccess.GetAdminService();

            Entity ent = entity.ToCrmEntity();

            return service.Create(ent);
        }

        public void Update(TEntity entity)
        {
            IOrganizationService service = _msCrmAccess.GetAdminService();

            Entity ent = entity.ToCrmEntity();

            service.Update(ent);
        }

        public void Delete(Guid Id)
        {
            IOrganizationService service = _msCrmAccess.GetAdminService();

            service.Delete(EntityName, Id);
        }

        public virtual TEntity Get(Guid Id)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id",Id)
            };

            DataTable dt = _sqlAccess.GetDataTable(GetQuery, parameters);

            return dt.ToList<TEntity>().FirstOrDefault();
        }

        public virtual List<TEntity> GetList()
        {
            DataTable dt = _sqlAccess.GetDataTable(GetListQuery);

            return dt.ToList<TEntity>();
        }

        public void SetState(Guid Id, int stateCode, int statusCode)
        {
            IOrganizationService service = _msCrmAccess.GetAdminService();

            SetStateRequest setStateReq = new SetStateRequest();

            setStateReq.EntityMoniker = new EntityReference(EntityName, Id);
            setStateReq.State = new OptionSetValue(stateCode);
            setStateReq.Status = new OptionSetValue(statusCode);

            SetStateResponse response = (SetStateResponse)service.Execute(setStateReq);
        }

        public void AssociateTo(Guid Id, EntityReferenceWrapper targetEntity, string relationShipName)
        {
            IOrganizationService service = _msCrmAccess.GetAdminService();

            AssociateRequest request = new AssociateRequest
            {
                Target = targetEntity.ToCrmEntityReference(),
                RelatedEntities = new EntityReferenceCollection
                {
                    new EntityReference(EntityName, Id)
                },
                Relationship = new Relationship(relationShipName)
            };

            service.Execute(request);
        }

        public void AssociateIn(Guid Id, EntityReferenceWrapper sourceEntity, string relationShipName)
        {
            IOrganizationService service = _msCrmAccess.GetAdminService();

            AssociateRequest request = new AssociateRequest
            {
                Target = new EntityReference(EntityName, Id),
                RelatedEntities = new EntityReferenceCollection
                {
                    sourceEntity.ToCrmEntityReference()
                },
                Relationship = new Relationship(relationShipName)
            };

            service.Execute(request);
        }

        public abstract string EntityName { get; }
        public abstract string GetQuery { get; }
        public abstract string GetListQuery { get; }

    }
}
