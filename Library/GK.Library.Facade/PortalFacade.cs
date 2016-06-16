using GK.Library.Business;
using GK.Library.Business.Interfaces;
using GK.Library.Entities.CrmEntities;
using GK.Library.Facade.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Facade
{
    public class PortalFacade : IPortalFacade
    {
        private IBaseBusiness<Portal> _portalBaseBusiness;
        private IAnnotationBusiness _annotationBusiness;

        private const string ANNOTATION_LOGO_SUBJECT = "logo";
        private const string ANNOTATION_LOGIN_SUBJECT = "login";

        public PortalFacade(IBaseBusiness<Portal> portalBaseBusiness, IAnnotationBusiness annotationBusiness)
        {
            _portalBaseBusiness = portalBaseBusiness;
            _annotationBusiness = annotationBusiness;
        }

        public Portal Get(Guid Id)
        {
            Portal portal = _portalBaseBusiness.Get(Id);

            FillPortalImageAnnotations(portal);

            return portal;
        }

        public Annotation GetLogoAnnotation(Guid Id)
        {
            List<Annotation> portalAnnotationList = _annotationBusiness.GetObjectAnnotationList(Id);

            return portalAnnotationList
                .Where(x => x.Name.Contains(ANNOTATION_LOGO_SUBJECT))
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefault();
        }

        public Annotation GetLoginAnnotation(Guid Id)
        {
            List<Annotation> portalAnnotationList = _annotationBusiness.GetObjectAnnotationList(Id);

            return portalAnnotationList
                .Where(x => x.Name.Contains(ANNOTATION_LOGIN_SUBJECT))
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefault();
        }

        private void FillPortalImageAnnotations(Portal portal)
        {
            List<Annotation> portalAnnotationList = _annotationBusiness.GetObjectAnnotationList(portal.Id);

            portal.LogoImage = portalAnnotationList.Where(x => x.Name.Contains(ANNOTATION_LOGO_SUBJECT))
                                   .OrderByDescending(x => x.CreatedOn)
                                   .FirstOrDefault();

            portal.LoginImage = portalAnnotationList.Where(x => x.Name.Contains(ANNOTATION_LOGIN_SUBJECT))
                                   .OrderByDescending(x => x.CreatedOn)
                                   .FirstOrDefault();
        }
    }
}
