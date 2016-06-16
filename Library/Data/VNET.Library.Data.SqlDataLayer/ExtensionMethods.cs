using GK.Library.Entities;
using GK.Library.Entities.CrmEntities;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace GK.Library.Data.SqlDataLayer
{
    public static class ExtensionMethods
    {
        public static EntityReference ToCrmEntityReference(this EntityReferenceWrapper entityReferenceWrapper)
        {
            EntityReference returnValue = new EntityReference()
            {
                LogicalName = entityReferenceWrapper.LogicalName,
                Id = entityReferenceWrapper.Id,
                Name = entityReferenceWrapper.Name
            };

            return returnValue;
        }

        public static OptionSetValue ToCrmOptionSetValue(this OptionSetValueWrapper optionSetValueWrapper)
        {
            OptionSetValue returnValue = new OptionSetValue((int)optionSetValueWrapper.AttributeValue);

            return returnValue;
        }

        public static Entity ToCrmEntity(this object entityObject)
        {
            Entity returnValue = null;

            if (entityObject == null)
            {
                return null;
            }

            System.Reflection.MemberInfo info = entityObject.GetType();

            var schemaAttr = info.GetCustomAttributes(typeof(CrmSchemaName), false).OfType<CrmSchemaName>().FirstOrDefault();

            if (schemaAttr != null)
            {
                string entityName = schemaAttr.SchemaName;
                returnValue = new Entity(entityName);
            }
            else
            {
                return null;
            }

            PropertyInfo[] properties = entityObject.GetType().GetProperties();

            foreach (PropertyInfo p in properties)
            {
                var attrFiledName = p.GetCustomAttributes(typeof(CrmFieldName), false).OfType<CrmFieldName>().FirstOrDefault();
                var attrFiledDataType = p.GetCustomAttributes(typeof(CrmFieldDataType), false).OfType<CrmFieldDataType>().FirstOrDefault();

                if (attrFiledName == null || attrFiledDataType == null)
                    continue;

                string fieldName = attrFiledName.FieldName;
                CrmDataType fieldDataType = attrFiledDataType.CrmDataType;

                var objectValue = entityObject.GetType().GetProperty(p.Name).GetValue(entityObject, null);

                if (objectValue != null)
                {
                    object crmFieldValue = objectValue.ToCrmType(fieldDataType);

                    if (objectValue.GetType() == typeof(Guid) && (Guid)crmFieldValue == Guid.Empty)
                        continue;

                    returnValue[fieldName] = crmFieldValue;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// <para>EntityReferenceWrapper ve OptionSetValueWrapper class'ları için özel metot içerir.</para>
        /// <para>EntityReferenceWrapper için IdFieldName'e ek olarak IdFieldName+Name ve IdFieldName+TypeName(zorunlu) olmalıdır.</para>
        /// <para>Örn: IdFieldName=AccountId ise AccountIdName ve AccountIdTypeName alanları olmalıdır.</para>
        /// <para>OptionSetValueWrapper için text olarak Value alınmak istenirse, filedName+Value alanı eklenmelidir.</para>
        /// <para>Örn: StatusCode=StatusCode ise, string value istenirse StatusCodeValue string alanı eklenmelidir.</para>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<TSource> ToList<TSource>(this DataTable dataTable) where TSource : new()
        {
            //TODO:GetAllProperties first persormance
            var dataList = new List<TSource>();

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return dataList;
            }

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            var objFieldNames = (from PropertyInfo aProp in typeof(TSource).GetProperties(flags)
                                 select new
                                 {
                                     Name = aProp.Name
                                     ,
                                     Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                                 }).ToList();

            var dataTblFieldNames = (from DataColumn aHeader in dataTable.Columns
                                     select new
                                     {
                                         Name = aHeader.ColumnName
                                         ,
                                         Type = aHeader.DataType
                                     }).ToList();
            var commonFields = objFieldNames.Select(x => x.Name).Intersect(dataTblFieldNames.Select(x => x.Name)).ToList();

            foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
            {
                var aTSource = new TSource();
                foreach (var aField in commonFields)
                {
                    if (dataRow[aField] != DBNull.Value)
                    {
                        PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField);
                        Type propertyType = Nullable.GetUnderlyingType(propertyInfos.PropertyType) ?? propertyInfos.PropertyType;

                        try
                        {
                            if (propertyType.IsEnum)
                            {
                                object enumValue = Enum.ToObject(propertyType, dataRow[aField]);
                                propertyInfos.SetValue(aTSource, enumValue, null);
                            }
                            else if (propertyType == typeof(EntityReferenceWrapper))
                            {
                                EntityReferenceWrapper erValue = new EntityReferenceWrapper { Id = (Guid)dataRow[aField] };

                                if (dataRow.Table.Columns.Contains(aField + "Name") && dataRow[aField + "Name"] != DBNull.Value)
                                {
                                    erValue.Name = dataRow[aField + "Name"].ToString();
                                }

                                if (dataRow.Table.Columns.Contains(aField + "TypeName") && dataRow[aField + "TypeName"] != DBNull.Value)
                                {
                                    erValue.LogicalName = dataRow[aField + "TypeName"].ToString();
                                }

                                propertyInfos.SetValue(aTSource, erValue, null);
                            }
                            else if (propertyType == typeof(OptionSetValueWrapper))
                            {
                                if (dataRow.Table.Columns.Contains(aField + "Value") && dataRow[aField + "Value"] != DBNull.Value)
                                {
                                    propertyInfos.SetValue(aTSource, new OptionSetValueWrapper() { AttributeValue = (int)dataRow[aField], Value = dataRow[aField + "Value"].ToString() }, null);
                                }
                                else
                                {
                                    propertyInfos.SetValue(aTSource, new OptionSetValueWrapper() { AttributeValue = (int)dataRow[aField] }, null);
                                }
                            }
                            else
                            {
                                propertyInfos.SetValue(aTSource, dataRow[aField], null);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                if (objFieldNames.Count == 0)
                {
                    dataList.Add((TSource)dataRow[0]);

                    continue;
                }
                else
                {
                    dataList.Add(aTSource);
                }
            }
            return dataList;
        }

        public static object ToCrmType(this object objectValue, CrmDataType crmDataType)
        {
            object returnValue = objectValue;

            if (returnValue == null)
                return returnValue;


            switch (crmDataType)
            {
                case CrmDataType.UNIQUEIDENTIFIER:
                    if (objectValue.GetType() == typeof(string))
                    {
                        returnValue = new Guid(objectValue.ToString());
                    }
                    break;
                case CrmDataType.STRING:
                    returnValue = returnValue.ToString();
                    break;
                case CrmDataType.INT:
                    returnValue = Convert.ToInt32(returnValue);
                    break;
                case CrmDataType.SMALLINT:
                    returnValue = Convert.ToInt16(returnValue);
                    break;
                case CrmDataType.DATETIME:
                    returnValue = Convert.ToDateTime(returnValue);
                    break;
                case CrmDataType.ENTITYREFERENCE:
                    if (objectValue.GetType() == typeof(EntityReferenceWrapper))
                    {
                        returnValue = ((EntityReferenceWrapper)objectValue).ToCrmEntityReference();
                    }
                    break;
                case CrmDataType.OPTIONSETVALUE:
                    if (objectValue.GetType() == typeof(OptionSetValueWrapper))
                    {
                        returnValue = ((OptionSetValueWrapper)objectValue).ToCrmOptionSetValue();
                    }
                    break;
                case CrmDataType.MONEY:
                    returnValue = new Money(Convert.ToDecimal(objectValue));
                    break;
                case CrmDataType.DECIMAL:
                    returnValue = Convert.ToDecimal(objectValue);
                    break;
                case CrmDataType.BOOL:
                    returnValue = Convert.ToBoolean(objectValue);
                    break;
                case CrmDataType.ACTIVITYPARTY:
                    if (objectValue.GetType() == typeof(EntityReferenceWrapper))
                    {
                        Entity party = new Entity("activityparty");

                        party["partyid"] = ((EntityReferenceWrapper)objectValue).ToCrmEntityReference();

                        returnValue = new Entity[] { party };
                    }
                    break;
                case CrmDataType.DOUBLE:
                    returnValue = Convert.ToDouble(returnValue);
                    break;
                default:
                    break;
            }

            return returnValue;
        }

        public static List<KeyValuePair<TKey, TValue>> ToKeyValueList<TKey, TValue>(this DataTable dataTable)
        {
            var dataList = new List<KeyValuePair<TKey, TValue>>();

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return dataList;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                if (row["Key"] != DBNull.Value && row["Value"] != DBNull.Value)
                {
                    TKey key = (TKey)row["Key"];
                    TValue value = (TValue)row["Value"];

                    dataList.Add(new KeyValuePair<TKey, TValue>(key, value));
                }
            }

            return dataList;
        }
    }
}
