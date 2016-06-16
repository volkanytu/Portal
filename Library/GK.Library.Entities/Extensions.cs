using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GK.Library.Entities.CrmEntities;
using Newtonsoft.Json;

namespace GK.Library.Entities
{
    public static class Extensions
    {
        public static string SerializeToJSON(this object value)
        {

            return JsonConvert.SerializeObject(value);
        }

        public static EntityReferenceWrapper ToEntityReferenceWrapper(this object entityObject)
        {
            EntityReferenceWrapper returnValue = null;

            if (entityObject == null)
            {
                return null;
            }

            System.Reflection.MemberInfo info = entityObject.GetType();

            var schemaAttr = info.GetCustomAttributes(typeof(CrmSchemaName), false).OfType<CrmSchemaName>().FirstOrDefault();

            if (schemaAttr != null)
            {
                string entityName = schemaAttr.SchemaName;

                var id = entityObject.GetType().GetProperty("Id").GetValue(entityObject, null);
                var name = entityObject.GetType().GetProperty("Name").GetValue(entityObject, null);

                if (id != null && name != null)
                {
                    returnValue = new EntityReferenceWrapper();

                    returnValue.Id = (Guid)id;
                    returnValue.Name = name.ToString();
                    returnValue.LogicalName = entityName;
                }

                return returnValue;
            }
            else
            {
                return null;
            }
        }

        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name != null)
            {
                var field = type.GetField(name);
                if (field != null)
                {
                    var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                        return attr.Description;
                }
            }
            return value.ToString();
        }
    }
}
