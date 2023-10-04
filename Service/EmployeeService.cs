using AutoMapper;
using Entities.DTO;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ILogService _logger;
        private readonly IDataShaper<Employee> _shaper;

        public EmployeeService(IRepositoryManager repositoryManager, IMapper mapper, ILogService logger, IDataShaper<Employee> shaper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _logger = logger;
            _shaper = shaper;
        }

        public async Task<Employee> CreateEmployeeAsync(EmployeeDtoForCreate EmployeeDtoForCreate)
        {
            if(EmployeeDtoForCreate is null) throw new ArgumentNullException(nameof(EmployeeDtoForCreate));
            //_repositoryManager.EmployeeRepository.CreateEmployee(employee);
            var employe = _mapper.Map<Employee>(EmployeeDtoForCreate);
             _repositoryManager.EmployeeRepository.CreateEmployee(employe);
            await _repositoryManager.SaveAsync();
            return employe;
        }

        public async Task DeleteEmployeeAsync(int id, bool trackChanges)
        {
            var employee = await _repositoryManager.EmployeeRepository.GetByIdAsync(id, true);
            if (employee is null) throw new EmployeeNotFoundException($"{id} böyle bir personel yok");
            _repositoryManager.EmployeeRepository.DeleteEmployee(employee);
          await  _repositoryManager.SaveAsync();
        }

        public async Task <(IEnumerable<ExpandoObject> expando, MetaData metaData)> GetAllEmpAsync(EmployeeParameters employeeParameters,bool trackChanges)
        {
            if (!employeeParameters.ValipIdRange)
                throw new BadRequestException($"Id değerlerini düzgün şekilde giriniz");

            var Metadata = await _repositoryManager.EmployeeRepository.GetAllAsync(employeeParameters, trackChanges);

            var emp = _mapper.Map<IEnumerable<Employee>>(Metadata);
            var shapedData = _shaper.ShapeData(emp, employeeParameters.Fields);
            return (shapedData, Metadata.MetaData);
        }

        public async Task<Employee> GetByIdAsync(int id, bool trackChanges)
        {
            var employee = await _repositoryManager.EmployeeRepository.GetByIdAsync(id, false);
            if(employee is null)
                throw new EmployeeNotFoundException($"{id} ye ait personel bulunmamaktadır");
            return  employee;
        }

        public async Task<(EmployeeDtoForUpdate employeeDtoForUpdate, Employee employee)> GetOneBookForPatchAsync(int id, bool trackChanges)
        {
            var emp=await _repositoryManager.EmployeeRepository.GetByIdAsync(id,trackChanges);
            if (emp is null)
                throw new EmployeeNotFoundException($"{id} Aradığınız personel bulunamadı");
            var empDto = _mapper.Map<EmployeeDtoForUpdate>(emp);
            return (empDto, emp);
        }

        public async Task SaveChangesForPatchAsync(EmployeeDtoForUpdate employeeDtoForUpdate, Employee employee)
        {
            _mapper.Map(employee,employeeDtoForUpdate);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateEmployeeAsync(int id, EmployeeDtoForUpdate empDto, bool trackChanges)
        {
            var employeDto = await _repositoryManager.EmployeeRepository.GetByIdAsync(id, trackChanges);
            //if(employeDto is null) throw new EmployeeNotFoundException($"{id} ye ait personel bulunmamaktadır");
          var employee=  _mapper.Map<Employee>(empDto);
            _repositoryManager.EmployeeRepository.UpdateEmployee(employee);
          await  _repositoryManager.SaveAsync();
        }
    }
}
