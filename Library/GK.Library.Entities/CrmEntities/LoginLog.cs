using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Entities.CrmEntities
{
    [CrmSchemaName("new_loginlog")]
    public class LoginLog : EntityBase
    {
        [CrmFieldDataType(CrmDataType.UNIQUEIDENTIFIER)]
        [CrmFieldName("new_loginlogid")]
        public Guid Id { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_name")]
        public string Name { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_userid")]
        public EntityReferenceWrapper UserId { get; set; }

        [CrmFieldDataType(CrmDataType.DATETIME)]
        [CrmFieldName("new_logindata")]
        public DateTime? LoginDate { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_ipaddress")]
        public string IpAddress { get; set; }

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
