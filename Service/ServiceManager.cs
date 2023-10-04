using AutoMapper;
using Entities.Models;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IEmployeeService> _employeeService;
        private readonly Lazy<IAuthenticationService> _authenticationService;



        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, ILogService logService, IDataShaper<Employee> dataShaper,IConfiguration configuration)
        {
            _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager, mapper, logService, dataShaper));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(configuration, logService));
        }

        public IEmployeeService EmployeeService => _employeeService.Value;  
        public IAuthenticationService AuthenticationService=> _authenticationService.Value;
        
    }
}
