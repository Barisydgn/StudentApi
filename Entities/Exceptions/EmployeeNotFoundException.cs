using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class EmployeeNotFoundException:NotFoundException
    {
        public EmployeeNotFoundException(string message):base($"Verdiğiniz  personel bulunamamıştır")
        {

        }
    }
}
