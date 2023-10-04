using Entities.DTO;
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        Task<(IEnumerable<ExpandoObject> expando, MetaData metaData)> GetAllEmpAsync(EmployeeParameters employeeParameters,bool trackChanges);
        Task<Employee> GetByIdAsync(int id,bool trackChanges);
        Task <Employee> CreateEmployeeAsync(EmployeeDtoForCreate EmployeeDtoForCreate);
        Task UpdateEmployeeAsync(int id,EmployeeDtoForUpdate EmployeeDtoForUpdate, bool trackChanges);
        Task  DeleteEmployeeAsync(int id,bool trackChanges);

        Task<(EmployeeDtoForUpdate employeeDtoForUpdate, Employee employee)> GetOneBookForPatchAsync(int id, bool trackChanges);

        Task SaveChangesForPatchAsync(EmployeeDtoForUpdate employeeDtoForUpdate,Employee employee);

    }
}
