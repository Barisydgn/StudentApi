using AutoMapper;
using Entities.DTO;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presentation.ActionFilters;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/Employee")]
    [ApiVersion("1.0")]
   [ServiceFilter(typeof(LogFilterAttribute))]//METODLARDA LOG KAYDI İÇİN YAZDIK BUNU
   //[ServiceFilter(typeof(ValidationFilterAttribute))]//400 VE 422 HATA KODU İÇİN METOD ÇALIŞMADAN ÖNCE BAKTIK
    [ResponseCache(CacheProfileName = "5mins")]
    public class EmployeeControllers:ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public EmployeeControllers(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper;
        }

        [HttpGet("Listele")]
        public async Task<IActionResult> EmployeeList([FromQuery]EmployeeParameters employeeParameters)
        {
            var pageResult = await _serviceManager.EmployeeService.GetAllEmpAsync(employeeParameters,false);
            //if (pageResult is null)
            //    throw new BadRequestException("Personel Listelenemedi");

            Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(pageResult.metaData));
            return Ok(pageResult.expando);
        }

        [HttpHead("Listele2")]
        public async Task<IActionResult> EmployeeList2([FromQuery] EmployeeParameters employeeParameters)
        {
            var employee = await _serviceManager.EmployeeService.GetAllEmpAsync(employeeParameters, false);

            return Ok(employee.expando);
        }

        [HttpGet("Getir/{id:int}")]
        public async Task<IActionResult> GetOneEmployeeAsync([FromRoute(Name ="id")]int id)
        {
            var employee = await _serviceManager.EmployeeService.GetByIdAsync(id, true);
            if (employee is null)
                throw new EmployeeNotFoundException($"Girdiğiniz {id} ye ait personel bulunamamıştır");
            return Ok(employee);
        }

       
        [HttpPost("Ekleme")]
        public  async Task<IActionResult> AddEmployeeAsync([FromBody] EmployeeDtoForCreate employee)
        {
            var employ= await     _serviceManager.EmployeeService.CreateEmployeeAsync(employee);
            return StatusCode(201, employ);
        }

      
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployeeAsync([FromRoute(Name ="id")]int id)
        {
            //var employee = await _serviceManager.EmployeeService.GetByIdAsync(id, true);
            //if (employee is null)
            //    throw new EmployeeNotFoundException($"{id} ye ait Personel Bulunamamıştır");
       await     _serviceManager.EmployeeService.DeleteEmployeeAsync(id, false);
           
            return NoContent();
        }

      
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmployeeAsync([FromBody] EmployeeDtoForUpdate employeeDtoForUpdate, [FromRoute(Name ="id")] int id)
        {
            
            await _serviceManager.EmployeeService.UpdateEmployeeAsync(id, employeeDtoForUpdate, true);
            return NoContent();
        }


        //ALTTAKİ METODDA APİ İLETİŞİM SEÇENEKLERİNE İZİN VERDİK
        [HttpOptions]
        public IActionResult GetEmployeeOptions()
        {
            Response.Headers.Add("Allow", "GET , POST , PUT , DELETE , OPTİONS, HEAD , PATCH");
            return Ok();
        }

       
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<EmployeeDtoForUpdate> employeePatch)
        {
            if (employeePatch is null)
                throw new BadRequestException("Girdiğiniz veriler null geldi");

            var result = _serviceManager.EmployeeService.GetOneBookForPatchAsync(id, false);
            employeePatch.ApplyTo(result.Result.employeeDtoForUpdate, ModelState);
            TryValidateModel(result.Result.employeeDtoForUpdate);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _serviceManager.EmployeeService.SaveChangesForPatchAsync(result.Result.employeeDtoForUpdate, result.Result.employee);

            return NoContent();

        }

    }
}
