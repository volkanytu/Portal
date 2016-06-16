using GK.Library.Entities.CrmEntities;
using System;
using System.Collections.Generic;

namespace GK.Library.Data.SqlDataLayer.Interfaces
{
    public interface IAnnotationDao
    {
       List<Annotation> GetObjectAnnotationList(Guid objectId);
    }
}
