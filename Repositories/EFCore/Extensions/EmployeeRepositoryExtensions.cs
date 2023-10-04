using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Extensions
{
    public static class EmployeeRepositoryExtensions
    {
        public static IQueryable<Employee> EmployeeFilter(this IQueryable<Employee> employees, uint maxId, uint minId) => employees.Where(x => x.Id >= minId && x.Id <= maxId);

        public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return employees;
            var lower = search.Trim().ToLower();
            return employees.Where(x => x.Name.ToLower().Contains(search));
        }


        public static IQueryable<Employee> Sort(this IQueryable<Employee> employees,string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return employees.OrderBy(x => x.Id);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);

            if(orderQuery is null)
                return employees.OrderBy(x => x.Id);

            return employees.OrderBy<Employee>(orderQuery);
        }
    }
}
