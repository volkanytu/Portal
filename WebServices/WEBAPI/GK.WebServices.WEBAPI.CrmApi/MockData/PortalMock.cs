using GK.Library.Entities.CrmEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GK.WebServices.WEBAPI.CrmApi.MockData
{
    public class PortalMock
    {
        public List<Portal> GetPortalList()
        {
            List<Portal> returnValue = new List<Portal>();

            returnValue.Add(new Portal()
            {
                Name = "PORTAL_I",
                CreatedOn = DateTime.Now
            });

            returnValue.Add(new Portal()
            {
                Name = "PORTAL_II",
                CreatedOn = DateTime.Now
            });

            return returnValue;
        }

        public Portal GetPortal()
        {
            return new Portal()
            {
                Name = "PORTAL_I",
                CreatedOn = DateTime.Now
            };
        }
    }
}