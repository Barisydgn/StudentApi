﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class UserAuthentication
    {
        [Required(ErrorMessage = "Kullanıcı Adı Boş Geçilemez")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Şifre Boş Geçilemez")]
        public string? Password { get; set; }
    }
}
