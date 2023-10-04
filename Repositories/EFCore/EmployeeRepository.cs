using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateEmployee(Employee employee) => Create(employee);

        public void DeleteEmployee(Employee employee)=>Delete(employee);

        //public async Task<PageList<Employee>> GetAllaAsync(EmployeeParameters employeeParameters, bool trackChanges, string search)
        //{
        //   var employee=await GetAll(trackChanges).Search(search).ToListAsync();
        //    return PageList<Employee>.ToPageList(employee, employeeParameters._PageSize, employeeParameters.PageNumber);
        //}
    

        public async Task<PageList<Employee>> GetAllAsync(EmployeeParameters employeeParameters, bool trackChanges)
        {
            var employee = await GetAll(trackChanges).EmployeeFilter(employeeParameters.MaxId,employeeParameters.MinId).Sort(employeeParameters.OrderBy).ToListAsync();
            return PageList<Employee>.ToPageList(employee, employeeParameters._PageSize, employeeParameters.PageNumber);
        }

        //BU FİLTRELEMELİ LİSTELEME
        //public async Task<PageList<Employee>> GetAllEAsync(EmployeeParameters employeeParameters, bool trackChanges)
        //{
        //    //var employee = await Find(x=> 
        //    //((x.Id>=employeeParameters.MinId) && (x.Id<=employeeParameters.MaxId))
        //    //,trackChanges).OrderBy(x => x.Id).ToListAsync();
        //    //return PageList<Employee>.ToPageList(employee, employeeParameters._PageSize, employeeParameters.PageNumber);

        //    var employee = await GetAll(trackChanges).EmployeeFilter(employeeParameters.MaxId,employeeParameters.MinId).OrderBy(x => x.Id).ToListAsync();
        //    return PageList<Employee>.ToPageList(employee, employeeParameters._PageSize, employeeParameters.PageNumber);

        //}


        public async Task<Employee> GetByIdAsync(int id, bool trackChanges) => await Find(x => x.Id.Equals(id),trackChanges).SingleOrDefaultAsync();

        public void UpdateEmployee(Employee employee)=> Update(employee);
    }
}
