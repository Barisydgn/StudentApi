using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class EmployeeDtoForUpdate
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Ad boş geçilemez")]
        [MaxLength(30)]
        [MinLength(2)]
        public string? Name { get; set; }
        [Required(ErrorMessage ="Soyad boş geçilemez")]
        [MaxLength(30)]
        [MinLength(2)]
        public string? Surname { get; set; }
    }
}
