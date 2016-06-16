using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Entities.CrmEntities
{
    public class EntityBase
    {
        [CrmFieldDataType(CrmDataType.OPTIONSETVALUE)]
        [CrmFieldName("statecode")]
        public OptionSetValueWrapper State { get; set; }

        [CrmFieldDataType(CrmDataType.OPTIONSETVALUE)]
        [CrmFieldName("statuscode")]
        public OptionSetValueWrapper Status { get; set; }

        [CrmFieldDataType(CrmDataType.ENTITYREFERENCE)]
        [CrmFieldName("ownerid")]
        public EntityReferenceWrapper OwnerId { get; set; }

        [CrmFieldDataType(CrmDataType.DATETIME)]
        [CrmFieldName("createdon")]
        public DateTime? CreatedOn
        {
            get
            {
                return _createdOn;
            }
            set
            {
                _createdOn = value;

                if (value != null)
                {
                    CreatedOnString = ((DateTime)value).ToString("dd.MM.yyyy HH:mm");
                }
            }
        }

        [CrmFieldDataType(CrmDataType.DATETIME)]
        [CrmFieldName("modifiedon")]
        public DateTime? ModifiedOn
        {
            get
            {
                return _modifieddOn;
            }
            set
            {
                _modifieddOn = value;

                if (value != null)
                {
                    ModifiedOnString = ((DateTime)value).ToString("dd.MM.yyyy HH:mm");
                }
            }
        }

        public string CreatedOnString { get; set; }
        private DateTime? _createdOn { get; set; }

        public string ModifiedOnString { get; set; }
        private DateTime? _modifieddOn { get; set; }
    }
}
