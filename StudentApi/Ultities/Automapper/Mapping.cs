using AutoMapper;
using Entities.DTO;
using Entities.Models;

namespace StudentApi.Ultities.Automapper
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<Employee,EmployeeDtoForUpdate>().ReverseMap();
            CreateMap<Employee,EmployeeDtoForCreate>().ReverseMap();
        }
    }
}
