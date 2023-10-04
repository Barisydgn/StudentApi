using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/Employee")]
    [ApiVersion("2.0")]
    [ServiceFilter(typeof(LogFilterAttribute))]//METODLARDA LOG KAYDI İÇİN YAZDIK BUNU
    [ServiceFilter(typeof(ValidationFilterAttribute))]//400 VE 422 HATA KODU İÇİN METOD ÇALIŞMADAN ÖNCE BAKTIK
    [ResponseCache(CacheProfileName = "5mins")]
    public class EmployeeV2Controllers:ControllerBase
    {
    }
}
