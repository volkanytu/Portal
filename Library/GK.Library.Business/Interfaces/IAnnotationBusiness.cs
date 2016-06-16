using GK.Library.Entities.CrmEntities;
using System;
using System.Collections.Generic;

namespace GK.Library.Business.Interfaces
{
    public interface IAnnotationBusiness
    {
        List<Annotation> GetObjectAnnotationList(Guid objectId);
    }
}
