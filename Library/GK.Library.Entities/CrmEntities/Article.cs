using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Entities.CrmEntities
{
    [CrmSchemaName("new_article")]
    public class Article : EntityBase
    {
        [CrmFieldDataType(CrmDataType.UNIQUEIDENTIFIER)]
        [CrmFieldName("new_articleid")]
        public Guid Id { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_name")]
        public string Name { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_summary")]
        public string Summary { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_content")]
        public string Content { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_imageurl")]
        public string ImageUrl { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_categoryid")]
        public EntityReferenceWrapper CategoryId { get; set; }

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
