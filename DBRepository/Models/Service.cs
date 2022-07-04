using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskAdminApi.Models
{
    public class Service
    {
        public int Id { get; set; }

        // Наименование услуги 
        [Required(ErrorMessage = "Введите наименование услуги:")]
        [StringLength(40)]
        [Display(Name = "Наименование услуги:")]
        public string Service_Name { get; set; }

        // Описание услуги
        [StringLength(200)]
        [Display(Name = "Описание услуги:")]
        public string Service_Description { get; set; }

        // Исчислять затраченное время в минутах
        [DefaultValue(false)]
        public bool Service_Time_Type_Minutes { get; set; }

        // Исчислять время в часах
        [DefaultValue(false)]
        public bool Service_Time_Type_Hours { get; set; }

        // Список клиентов для данной услуги
        public virtual ICollection<Client> Clients { get; set; }

        // Свзяка для промежуточной таблицы
        public List<ServicesForClient> ServicesForClients { get; set; }
    }
}
