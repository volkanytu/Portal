using GK.Library.Entities.CrmEntities;
using GK.Library.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace GK.WebServices.WEBAPI.CrmApi.Controllers
{
    public interface IBaseController<TEntity>
    {
        List<TEntity> Get();
        TEntity Get(Guid id);
        Paged<TEntity> Get(int pageSize, int page);
        string Create(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(Guid id);
    }
}
