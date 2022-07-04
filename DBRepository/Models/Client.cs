using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaskAdminApi.Models
{
    public class Client
    {
        public int Id { get; set; }

        // Личное имя 
        [Required(ErrorMessage = "Введите наименование компании-заказчика")]
        [StringLength(50)]
        [Display(Name = "Компания:")]
        public string Client_Name { get; set; }

        // Фамилия  
        [Required(ErrorMessage = "Введите имя контактного лица")]
        [StringLength(100)]
        [Display(Name = "Контактное лицо:")]
        public string Client_Contact_Person { get; set; }

        // Электронная почта  
        [Required(ErrorMessage = "Введите адрес электронной почты")]
        [Display(Name = "Адрес электронной почты:")]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Client_Contact_Email { get; set; }

        // Телефонный номер обратной связи    
        [Required(ErrorMessage = "Введите телефонный номер")]
        [Display(Name = "Телефонный номер для обратной связи:")]
        [StringLength(12)]
        public string Client_Contact_Phone { get; set; }

        // Список прикрепелнных задач
        public virtual ICollection<Service> Services { get; set; }

        public List<ServicesForClient> ServicesForClients { get; set; }


    }
}
