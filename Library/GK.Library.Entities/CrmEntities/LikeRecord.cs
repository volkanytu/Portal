using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Entities.CrmEntities
{
    [CrmSchemaName("new_likerecord")]
    public class LikeRecord : EntityBase
    {
        [CrmFieldDataType(CrmDataType.UNIQUEIDENTIFIER)]
        [CrmFieldName("new_likerecordid")]
        public Guid Id { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("new_name")]
        public string Name { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_userid")]
        public EntityReferenceWrapper UserId { get; set; }

        [CrmFieldDataType(CrmDataType.BOOL)]
        [CrmFieldName("new_liketype")]
        public bool? IsLiked { get; set; }

        [CrmFieldDataType(CrmDataType.BOOL)]
        [CrmFieldName("new_isimpropercontent")]
        public bool? IsImproperContent { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_educationid")]
        public EntityReferenceWrapper EducationId { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_articleid")]
        public EntityReferenceWrapper ArticleId { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("new_graffitiid")]
        public EntityReferenceWrapper GraffitiId { get; set; }

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
