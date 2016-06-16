using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.Entities.CrmEntities
{
    [CrmSchemaName("new_user")]
    public class User : EntityBase
    {
        [CrmFieldDataType(CrmDataType.UNIQUEIDENTIFIER)]
        [CrmFieldName("new_userid")]
        public Guid Id { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_name")]
        public string Name { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_contactid")]
        public EntityReferenceWrapper ContactId { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_imageurl")]
        public string ImageUrl { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_password")]
        public string Password { get; set; }

        public Contact Contact { get; set; }
        public List<Role> Roles { get; set; }

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
