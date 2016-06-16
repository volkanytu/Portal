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
    public class AnnotationBusiness : BaseBusiness<Annotation>, IAnnotationBusiness
    {
        private IBaseDao<Annotation> _baseAnnotationDao;
        private IAnnotationDao _annotationDao;

        public AnnotationBusiness(IBaseDao<Annotation> baseAnnotationDao, IAnnotationDao annotationDao)
            : base(baseAnnotationDao)
        {
            _baseAnnotationDao = baseAnnotationDao;
            _annotationDao = annotationDao;
        }

        public List<Annotation> GetObjectAnnotationList(Guid objectId)
        {
            return _annotationDao.GetObjectAnnotationList(objectId);
        }
    }
}
