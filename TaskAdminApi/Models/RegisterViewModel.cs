using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAdminApi.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите Ваш Email для регистрации:")]
        [Display(Name = "Email для обратной связи:")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль:")]
        [Display(Name = "Пароль:")]
        [StringLength(40, ErrorMessage = "Пароль должен содержать от 8 до 40 символов:", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтверждение пароля:")]
        [StringLength(40, ErrorMessage = "Пароль должен содержать от 8 до 40 символов:", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
