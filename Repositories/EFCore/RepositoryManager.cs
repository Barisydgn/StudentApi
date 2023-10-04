using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly IEmployeeRepository _employeeRepository;

        public RepositoryManager(RepositoryContext context, IEmployeeRepository employeeRepository)
        {
            _context = context;
            _employeeRepository = employeeRepository;
        }

        public IEmployeeRepository EmployeeRepository => _employeeRepository;

        public async Task SaveAsync()
        {
             await _context.SaveChangesAsync();
        }
    }
}
