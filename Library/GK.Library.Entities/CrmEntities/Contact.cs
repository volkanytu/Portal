using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.Entities.CrmEntities
{
    [CrmSchemaName("contact")]
    public class Contact : EntityBase
    {
        [CrmFieldDataType(CrmDataType.UNIQUEIDENTIFIER)]
        [CrmFieldName("contactid")]
        public Guid Id { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("fullname")]
        public string Name { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("firstname")]
        public string FirstName { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("lastname")]
        public string LastName { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("title")]
        public string Title { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("emailaddress1")]
        public string EmailAddress { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("mobilephone")]
        public string MobilePhone { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("telephone1")]
        public string WorkPhone { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_identitynumber")]
        public string IdentityNumber { get; set; }

        [CrmFieldDataType(CrmDataType.OPTIONSETVALUE)]
        [CrmFieldName("gendercode")]
        public OptionSetValueWrapper Gender { get; set; }

        [CrmFieldDataType(CrmDataType.DATETIME)]
        [CrmFieldName("birthdate")]
        public DateTime? BirthDate { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("description")]
        public string Description { get; set; }

        public enum StateCode
        {
            ACTIVE = 0,
            PASSIVE = 1,
        }

        public enum StatusCode
        {
            ACTIVE = 1,
            PASSIVE = 2,
        }
    }
}
