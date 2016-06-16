using GK.Library.Entities.CrmEntities;
using System;
namespace GK.Library.Facade.Interfaces
{
    public interface IPortalFacade
    {
        Portal Get(Guid Id);
        Annotation GetLoginAnnotation(Guid Id);
        Annotation GetLogoAnnotation(Guid Id);
    }
}
