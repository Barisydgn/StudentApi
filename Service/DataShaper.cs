using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        public PropertyInfo[] PropertyInfos { get; set; }
        public DataShaper()
        {
            PropertyInfos=typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldsString)
        {
            var getRequiredPro = GetRequiredProperties(fieldsString);
            return FetchDataForEntities(entities, getRequiredPro);
        }

        public ExpandoObject ShapeData(T entity, string fieldsString)
        {
            var getRequiredPro=GetRequiredProperties(fieldsString);
            return FetchDataForEntity(entity, getRequiredPro);
        }

        private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
        {
            var requiredFields=new List<PropertyInfo>();
            if (!string.IsNullOrEmpty(fieldsString))
            {
                var fields=fieldsString.Split(',',StringSplitOptions.RemoveEmptyEntries);
                foreach (var field in fields)
                {
                    var property = PropertyInfos.FirstOrDefault(x => x.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));
                    if (property is null)
                        continue;
                    requiredFields.Add(property);
                }
            }
            else
            {
                requiredFields=PropertyInfos.ToList();
            }
            return requiredFields;
        }

        private ExpandoObject FetchDataForEntity(T entity,IEnumerable<PropertyInfo> requiredProperties)
        {
            var shaped = new ExpandoObject();
            foreach (var properties in requiredProperties)
            {
                var objectPropertyValue = properties.GetValue(entity);
                shaped.TryAdd(properties.Name, objectPropertyValue);
            }
            return shaped;
        }

        private IEnumerable<ExpandoObject> FetchDataForEntities(IEnumerable<T> entities,IEnumerable<PropertyInfo> propertyInfos)
        {
            var shaped = new List<ExpandoObject>();
            foreach (var entity in entities)
            {
                var objectValue=FetchDataForEntity(entity,propertyInfos);
                shaped.Add(objectValue);
            }
            return shaped;
        }

    }
}
