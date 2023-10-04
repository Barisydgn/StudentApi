using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IEmployeeRepository:IRepositoryBase<Employee>
    {
       void CreateEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
        Task<PageList<Employee>> GetAllAsync(EmployeeParameters employeeParameters,bool trackChanges);
        //FİLTRELEME İÇİN
        //Task<PageList<Employee>> GetAllEAsync(EmployeeParameters employeeParameters,bool trackChanges);
        ////ARAMA İÇİN
        //Task<PageList<Employee>> GetAllaAsync(EmployeeParameters employeeParameters,bool trackChanges,string search);
        Task<Employee> GetByIdAsync(int id,bool trackChanges);


      
      

    }
}
