using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Extensions
{
    public static class OrderQueryBuilder
    {
        public static String CreateOrderQuery<T>(string orderByQueryString)
        {
            var orderParam=orderByQueryString.Trim().Split(',');

            var propertyInfos=typeof(T).GetProperties(System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.Instance);

            var orderQuerybuilder=new StringBuilder();

            foreach (var param in orderParam)
            {
                if(string.IsNullOrWhiteSpace(param))
                    continue;

                var PropertyFromQueryName = param.Split(" ")[0];

                var objectProperty = propertyInfos.SingleOrDefault(x => x.Name.Equals(PropertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if(objectProperty is null)
                    continue;

                var direction = param.EndsWith("desc") ? "descending" : "ascending";

                orderQuerybuilder.Append($"{objectProperty.Name.ToString()} {direction},");
            }

            var orderQuery= orderQuerybuilder.ToString().TrimEnd(',',' ');
            return orderQuery;
        }
    }
}
