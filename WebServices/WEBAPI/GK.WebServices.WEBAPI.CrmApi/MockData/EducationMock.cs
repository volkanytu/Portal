using GK.Library.Entities.CrmEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GK.WebServices.WEBAPI.CrmApi.MockData
{
    public class EducationMock
    {
        public List<Education> GetEducationList()
        {
            List<Education> returnValue = new List<Education>();
            returnValue.Add(new Education()
            {
                Id = Guid.NewGuid(),
                Content = "Test content_I",
                CreatedOn = DateTime.Now,
                ImageUrl = "ImagePath",
                Name = "Test_I",
                State = new OptionSetValueWrapper() { AttributeValue = (int)Education.StateCode.ACTIVE },
                Status = new OptionSetValueWrapper() { AttributeValue = (int)Education.StatusCode.PASSIVE },
                Summary = "Summary Test_I"
            });

            returnValue.Add(new Education()
            {
                Id = Guid.NewGuid(),
                Content = "Test content_II",
                CreatedOn = DateTime.Now,
                ImageUrl = "ImagePath_II",
                Name = "Test_II",
                State = new OptionSetValueWrapper() { AttributeValue = (int)Education.StateCode.ACTIVE },
                Status = new OptionSetValueWrapper() { AttributeValue = (int)Education.StatusCode.PASSIVE },
                Summary = "Summary Test_II"
            });

            returnValue.Add(new Education()
            {
                Id = Guid.NewGuid(),
                Content = "Test content_III",
                CreatedOn = DateTime.Now,
                ImageUrl = "ImagePath_III",
                Name = "Test_III",
                State = new OptionSetValueWrapper() { AttributeValue = (int)Education.StateCode.ACTIVE },
                Status = new OptionSetValueWrapper() { AttributeValue = (int)Education.StatusCode.PASSIVE },
                Summary = "Summary Test_III"
            });

            returnValue.Add(new Education()
            {
                Id = Guid.NewGuid(),
                Content = "Test content_IV",
                CreatedOn = DateTime.Now,
                ImageUrl = "ImagePath_IV",
                Name = "Test_IV",
                State = new OptionSetValueWrapper() { AttributeValue = (int)Education.StateCode.ACTIVE },
                Status = new OptionSetValueWrapper() { AttributeValue = (int)Education.StatusCode.PASSIVE },
                Summary = "Summary Test_IV"
            });

            return returnValue;
        }
    }
}