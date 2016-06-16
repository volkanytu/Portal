using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Entities.CrmEntities
{
    [CrmSchemaName("annotation")]
    public class Annotation : EntityBase
    {
        [CrmFieldDataType(CrmDataType.UNIQUEIDENTIFIER)]
        [CrmFieldName("annotationid")]
        public Guid Id { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("subject")]
        public string Name { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("objectid")]
        public EntityReferenceWrapper ObjectId { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("documentbody")]
        public string DocumentBody { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("filename")]
        public string FileName { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("mimetype")]
        public string MimeType { get; set; }

        [CrmFieldDataType(CrmDataType.STRING)]
        [CrmFieldName("notetext")]
        public string NoteText { get; set; }

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
