using GK.Library.Business.Interfaces;
using GK.Library.Data.SqlDataLayer;
using GK.Library.Data.SqlDataLayer.Interfaces;
using GK.Library.Entities.CrmEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Business
{
    public class AnnotationBusiness : IAnnotationBusiness
    {
        private BaseDao<Annotation> _baseDao;
        private IAnnotationDao _annotationDao;

        public AnnotationBusiness(BaseDao<Annotation> baseDao, IAnnotationDao annotationDao)
        {
            _baseDao = baseDao;
            _annotationDao = annotationDao;
        }

        public List<Annotation> GetObjectAnnotationList(Guid objectId)
        {
            return _annotationDao.GetObjectAnnotationList(objectId);
        }
    }
}
