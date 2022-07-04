using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAdminApi.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ведите Ваш Email:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите Ваш пароль:")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
